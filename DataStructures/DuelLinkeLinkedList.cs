using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class DuelLinkeLinkedList<T>: IEnumerable<T> where T : IComparable
    {
        class Node
        {
            /// <summary>
            /// The Next node in the chain
            /// </summary>
            public Node Next { get; set; }
            /// <summary>
            /// The previus node in the chain
            /// </summary>
            public Node Previus { get; set; }
            /// <summary>
            /// The Node's data
            /// </summary>
            public T Data { get; set; }
            public Node(T data)
            {
                Data = data;
            }
            public static Node operator ++(Node n) => n.Next;
            public static Node operator --(Node n) => n.Previus;
        }

        #region Properties

        /// <summary>
        /// The first node in the list
        /// </summary>
        Node First { get; set; }
        /// <summary>
        /// The last node in the list
        /// </summary>
        Node Last { get; set; }
        /// <summary>
        /// How many items in the list
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Retutns if the list is empty
        /// </summary>
        public bool IsEmptyList => First is null;
        /// <summary>
        /// Add data to the link list in a sorted way
        /// </summary>
        /// <param name="data"></param> 
	#endregion
        public void AddInOrder(T data)
        {
            Count++;
            if (IsEmptyList)
            {
                //add to a empty list
                AddToEmptyList(new Node(data));
                return;
            }
            //Find the position to append the new data
            var node = FindNodePositionFor(data);
            //create the new node
            var newNode = new Node(data);
            //if no node to insert before, meaning it should be the last node add the new node 
            if (node is null)
                AddLast(newNode);
            else AddBefore(node, newNode);

        }
        #region AddingItems private
        /// <summary>
        /// Add a new node before an existing node
        /// </summary>
        /// <param name="node">original node</param>
        /// <param name="newNode">node to append</param>
        void AddBefore(Node node, Node newNode)
        {
            //set the new node next pointer to the node
            newNode.Next = node;
            //set the new node previus pointer to the nodes previus pointer
            newNode.Previus = node.Previus;
            if (node.Previus != null)
            {
                //set the next on the node previus to the new node
                node.Previus.Next = newNode;
            }
            else First = newNode;
            //set the node.previus to the new node
            node.Previus = newNode;
        }
        /// <summary>
        /// Add a new node after an existing node
        /// </summary>
        /// <param name="node">original node</param>
        /// <param name="newNode">node to append</param>
        void AddAfter(Node node, Node newNode)
        {
            //set the next for the new node
            newNode.Next = node.Next.Previus;
            //set the previus fo the new node
            newNode.Previus = node;
            //set the previus pointer for the next node after the original node
            node.Next.Previus = newNode;
            //set the next pointer for the original node to the new node
            node.Next = newNode;
        }
        /// <summary>
        /// Adds a new <c>Node</c> to the end of the list
        /// </summary>
        /// <param name="newNode"></param>
        void AddLast(Node newNode)
        {
            //set the new node previus to point to the current last node
            newNode.Previus = Last;
            //set the current last next pointer to the new node
            Last.Next = newNode;
            //set the last pointer to the new node
            Last = newNode;

        }
        /// <summary>
        /// Adds a newnode to the first position in the list
        /// </summary>
        /// <param name="newNode"></param>
        void AddFirst(Node newNode)
        {
            //set the newNode's next to point to the current first
            newNode.Next = First;
            //set the current first node's previus to the new node
            First.Previus = newNode;
            //Save the new node as the first node
            First = newNode;
        }
        /// <summary>
        /// Adding a new node to an empty list
        /// </summary>
        /// <param name="newNode"></param>
        void AddToEmptyList(Node newNode) => First = Last = newNode;
        #endregion
        #region Search
        /// <summary>
        /// Find where to append the data recived O(N)
        /// for smaller than smallest data and for larger than largest O(1)
        /// </summary>
        /// <param name="data"></param>
        /// <returns>The node to append to</returns>
        Node FindNodePositionFor(T data)
        {
            //if the data is proceeds the first node's data it means it should be the first node.
            if (data.CompareTo(First.Data) < 0)
                return First;
            //if the data follows the Last node's data it means it should be added after the last node
            if (data.CompareTo(Last.Data) > 0)
                return null;
            //else locate the node
            Node tmp;
            for (tmp = First; tmp != null && tmp.Data.CompareTo(data) < 0; tmp++) ;
            return tmp;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (Node tmp = First; tmp != null; tmp++) yield return tmp.Data;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion
    }
}
