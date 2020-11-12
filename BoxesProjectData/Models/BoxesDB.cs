namespace BoxesProjectData.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class BoxesDB : DbContext
    {
        // Your context has been configured to use a 'BoxesDB' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'BoxesProjectData.Models.BoxesDB' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'BoxesDB' 
        // connection string in the application configuration file.
        public BoxesDB()
            : base("name=BoxesDB")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<BoxesPojectShared.Entities.Box> Boxes { get; set; }
    }
}