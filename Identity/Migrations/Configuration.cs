// in this project i had two context: ApplictiondbContext and person context, it was really important to know 
// that i have to enable the migration for both and when i want o update the database, which one should be updated.
// also its important in which context the initial data should be seeded, the data for users should be in the other 
// context which is connected to identity tables and database
namespace Identity.Migrations
{

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Identity.Models.PersonContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Identity.Models.PersonContext context)
        {



        }



    }

}
