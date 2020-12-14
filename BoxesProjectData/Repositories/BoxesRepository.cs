using BoxesPojectShared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxesProjectData.Repositories
{
    public class BoxesRepository : BoxesPojectShared.Interfaces.IRepository<Box>
    {
        List<Box> _initialData = new List<Box>
        {
            new Box
            {
                X = 1,
                Y = 1,
                Count = 30,
                TimeLastPurchase = DateTime.Now.Subtract(TimeSpan.FromDays(1))
            },
            new Box
            {
                X = 1,
                Y = 2,
                Count = 20,
                TimeLastPurchase = DateTime.Now.Subtract(TimeSpan.FromDays(2))
            },
            new Box
            {
                X = 1,
                Y = 3,
                Count = 40,
                TimeLastPurchase = DateTime.Now.Subtract(TimeSpan.FromDays(3))
            },
            new Box
            {
                X = 2,
                Y = 3,
                Count = 20,
                TimeLastPurchase = DateTime.Now
            },
            new Box
            {
                X = 2,
                Y = 1,
                Count = 20,
                TimeLastPurchase = DateTime.Now.Subtract(TimeSpan.FromDays(5))
            },
            new Box
            {
                X = 2,
                Y = 2,
                Count = 20,
                TimeLastPurchase = DateTime.Now
            },
            new Box
            {
                X = 3,
                Y = 3,
                Count = 20,
                TimeLastPurchase = DateTime.Now.Subtract(TimeSpan.FromDays(1))
            },
            new Box
            {
                X = 4,
                Y = 1,
                Count = 20,
                TimeLastPurchase = DateTime.Now
            },
            new Box
            {
                X = 4,
                Y = 2,
                Count = 20,
                TimeLastPurchase = DateTime.Now.Subtract(TimeSpan.FromDays(2))
            },
            new Box
            {
                X = 4,
                Y = 3,
                Count = 20,
                TimeLastPurchase = DateTime.Now
            },
            new Box
            {
                X = 20,
                Y = 2,
                Count = 20,
                TimeLastPurchase = DateTime.Now.Subtract(TimeSpan.FromDays(10))
            },
            new Box
            {
                X = 20,
                Y = 22,
                Count = 20,
                TimeLastPurchase = DateTime.Now
            },
            new Box
            {
                X = 100,
                Y = 100,
                Count = 20,
                TimeLastPurchase = DateTime.Now.Subtract(TimeSpan.FromDays(6))
            }
        };
        public BoxesRepository()
        {
            //if a new instance init the repo
            InitDB();
        }
        public void InitDB()
        {
            //make sure the db is created and if it needs to create it it will load initial data on the db
            using (var context = new Models.BoxesDBContext())
            {
                //Creat the entire db if not exist
                bool isNew = context.Database.CreateIfNotExists();
                if (isNew == false) return;
                //if db was created
                //inti for safty
                context.Database.Initialize(true);
            }
                //Load initial data
                _initialData.ForEach(b => Create(b));

        }
        public Box Create(Box entityToCreate)
        {
            using (var context = new Models.BoxesDBContext())
            {
                context.Boxes.Add(entityToCreate);
                context.SaveChanges();
            }
            return entityToCreate;
        }

        public void Delete(int id)
        {
            using (var context = new Models.BoxesDBContext())
            {
                var boxToDelete = context.Boxes.FirstOrDefault(b => b.Id.Equals(id));
                context.Boxes.Remove(boxToDelete);
                context.SaveChanges();
            }
        }

        public IEnumerable<Box> Get()
        {
            using (var context = new Models.BoxesDBContext())
            {
                return context.Boxes.OrderBy(b => b.TimeLastPurchase).ToList();
            }
        }

        public Box GetById(int id)
        {
            using (var context = new Models.BoxesDBContext())
            {
                return context.Boxes.FirstOrDefault(b => b.Id.Equals(id));
            }
        }

        public Box Update(Box entityToUpdate)
        {
            using (var context = new Models.BoxesDBContext())
            {
                var box = context.Boxes.FirstOrDefault(b => b.Id.Equals(entityToUpdate.Id));
                if (box is null) throw new ArgumentException("No such box in db");
                box.X = entityToUpdate.X;
                box.Y = entityToUpdate.Y;
                box.Count = entityToUpdate.Count;
                box.TimeLastPurchase = entityToUpdate.TimeLastPurchase;
                context.SaveChanges();
                return box;
            }
        }
    }
}
