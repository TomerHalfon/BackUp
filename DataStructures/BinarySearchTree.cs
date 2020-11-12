using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStructures
{
    /// <summary>
    /// Binary Search Tree
    /// </summary>
    /// <typeparam name="T">Data type to store</typeparam>
    public class BinarySearchTree<T> where T : IComparable
    {
        /// <summary>
        /// The Node of the tree
        /// </summary>
        class Node
        {
            /// <summary>
            /// The Left Child of this <c>Node</c>
            /// </summary>
            public Node Left { get; set; }
            /// <summary>
            /// The Right Child of this <c>Node</c>
            /// </summary>
            public Node Right { get; set; }
            /// <summary>
            /// The parent of this <c>Node</c>
            /// null if root
            /// </summary>
            public Node Parent { get; set; }
            /// <summary>
            /// The <c>T</c> data
            /// </summary>
            public T Data { get; set; }
            public Node(T value)
            {
                Data = value;
            }
        }

        /// <summary>
        /// The Root <c>Node</c> of this tree
        /// </summary>
        Node Root { get; set; }
        /// <summary>
        /// Returns if the tree is empty
        /// </summary>
        public bool IsEmpty => Root is null;
        /// <summary>
        /// Insert data to the tree
        /// </summary>
        /// <param name="data"></param>
        public void Insert(T data)
        {
            //Recursive insertion
            Node InnerInsert(Node node, Node parent = null)
            {
                //if no node create it 
                if (node is null)
                {
                    node = new Node(data) { Parent = parent };
                    return node;
                }
                //if smaller than the node continue left
                else if (node.Data.CompareTo(data) > 0)
                {
                    node.Left = InnerInsert(node.Left, node);
                }
                //if grater than the node continue right
                else if (node.Data.CompareTo(data) < 0)
                {
                    node.Right = InnerInsert(node.Right, node);
                }
                return node;
            }
            Root = InnerInsert(Root);
        }
        /// <summary>
        /// Delete data from tree
        /// </summary>
        /// <param name="data"></param>
        public void Delete(T data)
        {
            Node deleteRec(Node node)
            {
                /* Base Case: If the tree is empty */
                if (node == null) return node;

                /* Otherwise, recur down the tree */
                if (node.Data.CompareTo(data) > 0)
                    node.Left = deleteRec(node.Left);
                else if (node.Data.CompareTo(data) < 0)
                    node.Right = deleteRec(node.Right);

                // if key is same as root's key, then This is the node  
                // to be deleted  
                else
                {
                    // node with only one child or no child  
                    if (node.Left == null)
                        return node.Right;
                    else if (node.Right == null)
                        return node.Left;

                    // node with two children: Get the 
                    // inorder successor (smallest  
                    // in the right subtree)  
                    node.Data = minValue(node.Right);

                    // Delete the inorder succe ssor  
                    node.Right = deleteRec(node.Right);
                }
                return node;
            }
            T minValue(Node node)
            {
                T minv = node.Data;
                while (node.Left != null)
                {
                    minv = node.Left.Data;
                    node = node.Left;
                }
                return minv;
            }
            Root = deleteRec(Root);
        }
        /// <summary>
        /// Search a search term in the tree, data is set to default value if no such node and return false
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool FindExact(T searchTerm, out T data)
        {
            //Find a node with the searchTerm, null if no such node exist
            Node InnerFind(Node node)
            {
                if (node is null || node.Data.CompareTo(searchTerm) == 0)
                {
                    return node;
                }
                if (node.Data.CompareTo(searchTerm) > 0)
                    return InnerFind(node.Left);
                return InnerFind(node.Right);
            }
            var resNode = InnerFind(Root);
            if (resNode is null)
            {
                data = default(T);
                return false;
            }
            data = resNode.Data;
            return true;
        }
        /// <summary>
        /// Will search for the searchTerm in the tree, if it wont find it it will create it
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="data"></param>
        public void FindAndUpdate(T searchTerm, out T data)
        {
            //Recursive insertion
            Node InnerFindAndUpdate(Node node, out T res, Node parent = null)
            {
                //if no node create it 
                if (node is null)
                {
                    node = new Node(searchTerm) { Parent = parent };
                    res = node.Data;
                    return node;
                }
                //if smaller than the node continue left
                else if (node.Data.CompareTo(searchTerm) > 0)
                {
                    node.Left = InnerFindAndUpdate(node.Left, out res, node);
                }
                //if grater than the node continue right
                else if (node.Data.CompareTo(searchTerm) < 0)
                {
                    node.Right = InnerFindAndUpdate(node.Right, out res, node);
                }
                //if equls
                else
                {
                    //we dont want to make any changes to the tree in this case
                    //because here the node's data equals to the search term
                    res = node.Data;
                    return node;
                }
                return node;
            }
            Root = InnerFindAndUpdate(Root, out data);
        }
        /// <summary>
        /// Find Data with Exect metch, if no such data returns the parent Node's data
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="actualData"></param>
        /// <returns></returns>
        public T FindExactOrParent(T searchTerm)
        {
            Node InnerFind(Node node, Node parent = null)
            {
                // if no data return the parent of this node
                if (node is null)
                {
                    return parent;
                };
                if (node.Data.CompareTo(searchTerm) > 0)
                {
                    return InnerFind(node.Left, node);
                }
                else if (node.Data.CompareTo(searchTerm) < 0)
                {
                    return InnerFind(node.Right, node);
                }
                else return node;
            }
            return InnerFind(Root).Data;

        }

        /// <summary>
        /// Traverse in an inorder fashion and invoke an action
        /// </summary>
        /// <param name="action"></param>
        public void TraverseInOrder(Action<T> action)
        {
            void TraverseInOrder(Node parent)
            {
                if (parent is null)
                    return;
                if (parent.Left != null) TraverseInOrder(parent.Left);
                action(parent.Data);
                if (parent.Right != null) TraverseInOrder(parent.Right);
            }
            TraverseInOrder(Root);
        }

        /// <summary>
        /// Finds the closeset data to the search term (only  larger than exact)
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public T FindClosestOrExact(T searchTerm)
        {
            Node tmpNode = Root;
            T res = default(T);
            while (tmpNode != null)
            {
                //if found exact
                if(tmpNode.Data.CompareTo(searchTerm) == 0) return tmpNode.Data;
                
                //if smaller than data
                else if(tmpNode.Data.CompareTo(searchTerm) > 0)
                {
                    res = tmpNode.Data;
                    tmpNode = tmpNode.Left;
                }
                //if larger
                else tmpNode = tmpNode.Right;
            }
            //if null return the res
            return res;

        }
    }
}
