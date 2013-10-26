namespace IdSharp.Utils
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GenericBinder<T> where T: INotifyPropertyChanged
    {
        public GenericBinder(T component)
        {
            this.m_Component = component;
            this.m_PropertyDescriptorCollection = TypeDescriptor.GetProperties(this.m_Component);
            this.m_Component.PropertyChanged += new PropertyChangedEventHandler(this.Component_PropertyChanged);
            this.m_PropertyRetrievers = new Dictionary<string, MethodInvoker>();
            this.m_PropertyControls = new Dictionary<string, Control>();
            INotifyInvalidData data1 = component as INotifyInvalidData;
            if (data1 != null)
            {
                data1.InvalidData += new InvalidDataEventHandler(this.NotifyInvalidData_InvalidData);
            }
        }

        public GenericBinder(T component, IErrorProvider errorProvider) : this(component)
        {
            this.m_ErrorProvider = errorProvider;
        }

        public void Bind(IBindableControl bindableControl, string propertyName)
        {
            MethodInvoker invoker1 = null;
            EventHandler handler1 = null;
            MethodInvoker invoker2 = null;
            EventHandler handler2 = null;
            <>c__DisplayClass8<T> class1 = new <>c__DisplayClass8<T>();
            class1.bindableControl = bindableControl;
            class1.<>4__this = this;
            Guard.ArgumentNotNull(class1.bindableControl, "bindableControl");
            Guard.ArgumentNotNullOrEmptyString(propertyName, "property");
            int num1 = propertyName.IndexOf('.');
            if (num1 >= 0)
            {
                propertyName.Substring(num1 + 1, (propertyName.Length - num1) - 1);
                propertyName = propertyName.Substring(0, num1);
            }
            class1.tmpPropertyDescriptor = this.m_PropertyDescriptorCollection.Find(propertyName, false);
            if (class1.tmpPropertyDescriptor == null)
            {
                throw new ArgumentException(string.Format("'{0}' is not a valid property of '{1}'", propertyName, typeof(T).FullName), "property");
            }
            this.m_PropertyControls.Add(propertyName, class1.bindableControl.Control);
            if (class1.tmpPropertyDescriptor.PropertyType == typeof(string))
            {
                if (invoker1 == null)
                {
                    invoker1 = new MethodInvoker(class1.<Bind>b__0);
                }
                this.m_PropertyRetrievers.Add(propertyName, invoker1);
                if (handler1 == null)
                {
                    handler1 = new EventHandler(class1.<Bind>b__1);
                }
                class1.bindableControl.Validated += handler1;
            }
            else if (class1.tmpPropertyDescriptor.PropertyType == typeof(bool))
            {
                if (invoker2 == null)
                {
                    invoker2 = new MethodInvoker(class1.<Bind>b__2);
                }
                this.m_PropertyRetrievers.Add(propertyName, invoker2);
                if (handler2 == null)
                {
                    handler2 = new EventHandler(class1.<Bind>b__3);
                }
                class1.bindableControl.Validated += handler2;
            }
            else if (class1.tmpPropertyDescriptor.PropertyType.FindInterfaces(new System.Reflection.TypeFilter(this.TypeFilter), "System.Collections.IEnumerable").Length == 0)
            {
                throw new ArgumentException(string.Format("Control '{0}' cannot be bound to property '{1}' because it is not of a convertable type", class1.bindableControl.Name, propertyName));
            }
        }

        private void Component_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            MethodInvoker invoker1;
            if (this.m_PropertyRetrievers.TryGetValue(e.PropertyName, out invoker1))
            {
                invoker1();
            }
        }

        private void NotifyInvalidData_InvalidData(object sender, InvalidDataEventArgs e)
        {
            Control control1;
            if (this.m_PropertyControls.TryGetValue(e.Property, out control1))
            {
                this.m_ErrorProvider.SetError(control1, e.Message, ErrorType.Warning);
            }
        }

        private bool TypeFilter(System.Type type, object filterCriteria)
        {
            return type.FullName.StartsWith((string) filterCriteria);
        }


        private T m_Component;
        private IErrorProvider m_ErrorProvider;
        private Dictionary<string, Control> m_PropertyControls;
        private PropertyDescriptorCollection m_PropertyDescriptorCollection;
        private Dictionary<string, MethodInvoker> m_PropertyRetrievers;


    }
}

