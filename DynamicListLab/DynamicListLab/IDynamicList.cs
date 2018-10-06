using System.Collections.Generic;

namespace DynamicListLab
{
    interface IDynamicList<T> : IEnumerable<T>
    {
        void Add(T element);
        void Clear();
        bool Remove(T element);
        void RemoveAt(int index);
    }
}
