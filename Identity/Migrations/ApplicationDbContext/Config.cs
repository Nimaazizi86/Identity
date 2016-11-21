// this context connected to identity tables, so all the intial data regarding the predefined admin
// or other type of data that is realted to intitiy should be seeded here
namespace Identity.Migrations.ApplicationDbContext
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    internal sealed class Config : DbMigrationsConfiguration<Identity.Models.ApplicationDbContext>
    {
        public Config()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\ApplicationDbContext";
            ContextKey = "Identity.Models.ApplicationDbContext";
        }

        protected override void Seed(Identity.Models.ApplicationDbContext context)
        {
            // create a variable to store the role in it
            var roleStore = new RoleStore<IdentityRole>(context);
            // create a variable to store the managing data for the role in it
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            // create a new role and and save it in rolerSuperAdminesult and call it SuperAdmin
            var rolerSuperAdminesult = roleManager.Create(new IdentityRole("SuperAdmin"));

            // create new variable to store the information of the user
            var userStore = new UserStore<ApplicationUser>(context);

            // create new variable to store the managed information of the user
            var userManager = new UserManager<ApplicationUser>(userStore);
            
            // define th information of the user and its login password
            var result = userManager.Create(user: new ApplicationUser() { UserName = "Steve@Steve.com", Email = "Steve@Steve.com" }, password: "Password@123");

            // find the user by the name of "Steve@Steve.com"
            var user = userManager.FindByName("Steve@Steve.com");

            // Set the role of SuperAdmin to "Steve@Steve.com"
            userManager.AddToRole(user.Id, "SuperAdmin");

        }
    }
}
