namespace IdSharp.Utils
{
    using System;

    internal static class Guard
    {
        public static void ArgumentNotNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName, string.Format("Parameter '{0}' cannot be null.", parameterName));
            }
        }

        public static void ArgumentNotNullOrEmptyString(string value, string parameterName)
        {
            Guard.ArgumentNotNull(value, parameterName);
            if (value.Length == 0)
            {
                throw new ArgumentException(string.Format("String parameter '{0}' cannot be empty.", parameterName), parameterName);
            }
        }

    }
}

