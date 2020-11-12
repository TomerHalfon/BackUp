using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesPojectShared.Entities
{
   public class Box
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int Count { get; set; }
        public DateTime? TimeLastPurchase { get; set; }

    }
}
