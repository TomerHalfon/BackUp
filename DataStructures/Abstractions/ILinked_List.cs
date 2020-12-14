using System;
using System.Collections.Generic;

namespace DataStructures.Abstractions
{
    public interface ILinked_List<T>:IEnumerable<T> where T : IComparable
    {
        Linked_List<T>.Node First { get; }
        Linked_List<T>.Node Last { get; }

        Linked_List<T>.Node Add(T data);
        void DeleteNode(Linked_List<T>.Node node);
        IEnumerable<T> DeleteNodesSmallerThan(T condition);
        Linked_List<T>.Node InsertByValue(T data);
        bool ShiftNodeToEnd(Linked_List<T>.Node node);
    }
}