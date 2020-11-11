using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        //First is the newest

        static void Main(string[] args)
        {
            var l = new Linked_List<DateTime>();
            l.AddFirst(new DateTime(2020, 3, 1));
            l.AddFirst(new DateTime(2020, 3, 2));
            l.AddFirst(new DateTime(2020, 3, 3));
            l.AddFirst(new DateTime(2020, 3, 4));
            l.AddFirst(new DateTime(2020, 3, 5));
            l.DeleteNodesSmallerThan(new DateTime(2020, 3, 3));
            //for (int i = 0; i < 10; i += 2)
            //{
            //    l.Add(i);
            //}
            //for (int i = 1; i < 10; i += 2) 
            //{
            //    l.InsertByValue(i);
            //}
            ////l.DeleteNodesFromTheEnd(num => num < 3);
            //var nodes = l.DeleteNodesSmallerThan(3);
        }
    }
}
