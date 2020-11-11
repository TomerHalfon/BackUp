using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Linked_List<T> where T : IComparable
    {
        public class Node
        {
            internal Node(T data)
            {
                Data = data;
            }

            public Node Next { get; internal set; }
            public Node Previus { get; internal set; }
            public T Data { get; internal set; }
        }
        /// <summary>
        /// The First Node in the
        /// </summary>
        public Node First { get; internal set; }
        public Node Last { get; internal set; }


        /// <summary>
        /// Add data to the List, will add to the begining
        /// </summary>
        /// <param name="data"></param>
        /// <returns>the node</returns>
        public Node AddFirst(T data)
        {
            if (First == null)
                return AddToEmptyList(data);
            return AddBefore(First, data);
        }

        /// <summary>
        /// Insert data to the correct position in the list
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Node InsertByValue(T data)
        {
            if (First is null)
                return AddToEmptyList(data);
            var nodeAfer = FindNodeAfter(data);
            if (nodeAfer != null)
               return AddBefore(nodeAfer, data);
            var newNode = new Node(data);
            AddAfter(Last, newNode);
            return newNode;
        }

        /// <summary>
        /// Shift a node to the end of the list
        /// </summary>
        /// <param name="node"></param>
        public void ShiftNodeToEnd(Node node)
        {
            RemoveSpecific(node);
            AddAfter(Last, node);
        }

        /// <summary>
        /// Delete a node from the list
        /// </summary>
        /// <param name="node"></param>
        public void DeleteNode(Node node)
        {
            RemoveSpecific(node);
        }

        /// <summary>
        /// Delete all nodes smaller than contion
        /// </summary>
        /// <param name="condition"></param>
        /// <returns>All deleted nodes</returns>
        public IEnumerable<Node> DeleteNodesSmallerThan(T condition)
        {
            var tmp = Last;
            var res = new List<Node>();
            while (tmp != null && tmp.Data.CompareTo(condition) < 0)
            {
                DeleteNode(tmp);
                res.Add(tmp);
                tmp = tmp.Previus;
            }
            return res;
        }
        
        /// <summary>
        /// Find a node that follows the search term in order
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        private Node FindNodeAfter(T searchTerm)
        {
            var tmp = First;
            while (tmp != null && tmp.Data.CompareTo(searchTerm) > 0)
            {
                tmp = tmp.Next;
            }
            return tmp;
        }

        /// <summary>
        /// Removes a node from the list
        /// </summary>
        /// <param name="node"></param>
        private void RemoveSpecific(Node node)
        {
            var left = node.Previus;
            var right = node.Next;

            if (left != null)
            {
                left.Next = right;
            }
            else First = right;
            if (right != null)
            {
                right.Previus = left;
            }
            else Last = left;
        }

        /// <summary>
        /// Add to an empty list
        /// </summary>
        /// <param name="node"></param>
        private void AddToEmptyList(Node node)
        {
            First = node;
            Last = node;
        }

        /// <summary>
        /// Add element to an empty list (will be the only item currently)
        /// </summary>
        /// <param name="data"></param>
        private Node AddToEmptyList(T data)
        {
            var newNode = new Node(data);
            AddToEmptyList(newNode);
            return newNode;
        }

        /// <summary>
        /// Add data before a node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="data"></param>
        private Node AddBefore(Node node, T data)
        {
            var newNode = new Node(data) { Next = node, Previus = node.Previus };

            if (node.Previus != null)
            {
                node.Previus.Next = newNode;
            }
            else
            {
                First = newNode;
            }
            node.Previus = newNode;
            return newNode;
        }

        /// <summary>
        /// Appends a new node to an existing node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="newNode"></param>
        private void AddAfter(Node node, Node newNode)
        {
            newNode.Next = node.Next;
            newNode.Previus = node;
            if (node.Next != null)
            {
                node.Next.Previus = newNode;
            }
            else
            {
                Last = newNode;
            }
            node.Next = newNode;
        }
    }
}