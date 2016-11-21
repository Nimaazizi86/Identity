using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Identity.Models
{
    public class PersonContext: DbContext
    {
        public PersonContext() : base("PersonContext") { }

        public static PersonContext Create()
        {
            return new PersonContext();
        }

        public DbSet<Person> People { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<City> Cities { get; set; }

    }
}