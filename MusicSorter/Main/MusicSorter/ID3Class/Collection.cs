using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.ComponentModel;

namespace ID3.ID3v2Frames
{
    public class FilterCollection
    {
        private ArrayList _Frames;

        internal FilterCollection()
        {
            _Frames = new ArrayList();
        }

        /// <summary>
        /// 添加FrameID到FrameList
        /// </summary>
        /// <param name="FrameID">要添加的FrameID</param>
        public void Add(string FrameID)
        {
            if (!_Frames.Contains(FrameID))
                _Frames.Add(FrameID);
        }

        /// <summary>
        /// 删除确定的FrameID
        /// </summary>
        /// <param name="FrameID">要删除的FrameID</param>
        public void Remove(string FrameID)
        {
            _Frames.Remove(FrameID);
        }

        /// <summary>
        /// 获取Frames
        /// </summary>
        public string[] Frames
        {
            get
            { return (string[])_Frames.ToArray(typeof(string)); }
        }

        /// <summary>
        /// 删除全部的Frame
        /// </summary>
        public void Clear()
        {
            _Frames.Clear();
        }

        /// <summary>
        /// 判断Frame是否存在
        /// </summary>
        /// <param name="FrameID">FrameID to search</param>
        /// <returns>是否存在</returns>
        public bool IsExists(string FrameID)
        {
            if (_Frames.Contains(FrameID))
                return true;
            else
                return false;
        }
    }

    public class FramesCollection<T>
    {
        private ArrayList _Items; // 存储所有的数据- -

        /// <summary>
        /// Frames Collection
        /// </summary>
        internal FramesCollection()
        {
            _Items = new ArrayList();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            // 如果存在，则先删除
            _Items.Remove(item);

            _Items.Add(item);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="item">Item to remove</param>
        public void Remove(T item)
        {
            _Items.Remove(item);
        }

        /// <summary>
        /// 根据确定的位置删除
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            _Items.RemoveAt(index);
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            _Items.Clear();
        }

        /// <summary>
        /// Array of items
        /// </summary>
        public T[] Items
        {
            get
            { return (T[])_Items.ToArray(typeof(T)); }
        }

        /// <summary>
        /// 获取总长度
        /// </summary>
        public int Length
        {
            get
            {
                int Len = 0;
                foreach (ILengthable IL in _Items)
                    Len += IL.Length;
                return Len;
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        public void Sort()
        {
            _Items.Sort();
        }

        /// <summary>
        /// 当前FramesCollection的数目
        /// </summary>
        public int Count
        {
            get
            { return _Items.Count; }
        }

        public IEnumerator GetEnumerator()
        {
            return _Items.GetEnumerator();
        }
    }
}
