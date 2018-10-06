using System;
using System.Collections;
using System.Collections.Generic;

namespace DynamicListLab
{
    public class DynamicList<T> : IDynamicList<T>//, IEnumerable<T>
    {
        private const int MAX_CAPACITY = 0x7fefffff;
        private T[] _list;
        private T[] _emptyArray = new T[0];

        public int Count { get; private set; }

        public DynamicList()
        {
            _list = _emptyArray;
        }

        public DynamicList(int initialCount)
        {
            if (initialCount < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            else
            {
                _list = (initialCount > 0)
                    ? new T[initialCount]
                    : _emptyArray;
            }
        }

        public DynamicList(T[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                _list = new T[items.Length];

                for (int i = 0; i < items.Length; i++)
                {
                    _list[i] = items[i];
                }

                Count = _list.Length;
            }
        }

        public T this[int index]
        {
            get
            {
                if (index > Count)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    return _list[index];
                }
            }
            set
            {
                if (index > Count)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    _list[index] = value;
                    Count++;
                }
            }
        }

        public void Add(T item)
        {
           if (Count == MAX_CAPACITY - 1)
           {
                throw new OverflowException();
           }
           else if (Count == _list.Length)
           {
                ResizeArray();
           }
            _list[Count++] = item;

        }

        public void Clear()
        {
            if (Count > 0)
            {
                Array.Clear(_list, 0, Count);
            }
            Count = 0;
        }

        public bool Remove(T element)
        {
            int index = Array.IndexOf(_list, element, 0, Count);
            
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            
            return false;
        }

        public void RemoveAt(int index)
        {
            if (index >= Count)
            {
                throw new ArgumentOutOfRangeException();
            }
            else
            {
                Count--;
                Array.Copy(_list, index + 1, _list, index, Count - index);
            }
        }

        private void ResizeArray()
        {
            int newCount = Math.Min(MAX_CAPACITY, _list.Length * 2);

            if (newCount != _list.Length)
            {
                Array.Resize(ref _list, newCount);
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return _list[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return _list[i];
            }
        }
    }
}
