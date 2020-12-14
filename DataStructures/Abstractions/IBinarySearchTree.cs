using System;

namespace DataStructures.Abstractions
{
    public interface IBinarySearchTree<T> where T : IComparable
    {
        bool IsEmpty { get; }

        void Delete(T data, out T deletedData);
        bool FindAndUpdate(T searchTerm, out T data);
        T FindClosestOrExact(T searchTerm);
        bool FindExact(T searchTerm, out T data);
        T FindExactOrParent(T searchTerm);
        void Insert(T data);
        void TraverseInOrder(Action<T> action);
    }
}