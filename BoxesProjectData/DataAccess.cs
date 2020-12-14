using BoxesProjectData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoxesPojectShared.Entities;

namespace BoxesProjectData
{
    /// <summary>
    /// A Data Access Class to be used insted of full EF models
    /// </summary>
    public class DataAccess : IDisposable
    {
        BoxesDBContext Db { get; }

        public DataAccess()
        {
            Db = new BoxesDBContext();
        }
        
        public IEnumerable<Box> GetAllData() => Db.Boxes.AsEnumerable();
        public void Delete(Box box)
        {
            var BoxToDelete = Db.Boxes.Where(b => b.X == box.X && b.Y == box.Y).SingleOrDefault();
            if (BoxToDelete is null) return;
            Db.Boxes.Remove(BoxToDelete);
            //save changes
            Db.SaveChanges();
        }
        public void AddOrUpdatae(Box box)
        {
            var res = Db.Boxes.Where(b => b.X == box.X).Where(b => b.Y == box.Y).SingleOrDefault();
            if (res is default(Box)) AddData(box);
            else Update(res, box);
            Db.SaveChanges();
        }
        /// <summary>
        /// Update an existing box
        /// </summary>
        /// <param name="originalBox"></param>
        /// <param name="updatedBox"></param>
        private void Update(Box originalBox, Box updatedBox)
        {
            originalBox.Count = updatedBox.Count;
            originalBox.TimeLastPurchase = updatedBox.TimeLastPurchase;
        }
        /// <summary>
        /// Add Box To Db
        /// </summary>
        /// <param name="box"></param>
        private void AddData(Box box) => Db.Boxes.Add(box);

        /// <summary>
        /// Dispose the Db
        /// </summary>
        public void Dispose() => Db.Dispose();
    }
}
