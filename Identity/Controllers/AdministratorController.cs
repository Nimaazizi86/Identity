using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Identity.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace Identity.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AdministratorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Administrator
        public ActionResult Index()
        {

            //we are simply getting the Roles collection from ApplicationDbContext and returning to the View
            var roles = db.Roles.Include(o => o.Users).ToList();

            //var users = db.Users.Include(o => o.Roles).ToList();


            //var Courses = db.Courses.Include(o => o.Teacher);
            //var allUsers = db.Users.ToList();
            return View(roles);
        }

        //The first Create method simply returns the Create view and second Create
        //method accepts FormCollection object as parameter and uses context object 
        //(instance of ApplicationDbContext) to add a Role to the Roles collection. 
        //Note that despite our roles table in the database name is AspNetRoles, 
        //the model class name is IdentityRole (part of ASP.NET Identity provider). 
        //Calling SaveChanges method saves the new role into the database.

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                db.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                {
                    Name = collection["RoleName"]
                });
                db.SaveChanges();
                ViewBag.ResultMessage = "Role created successfully !";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //Clicking Delete link from the list of Roles deletes a particulrole from the database, and here is the action method of the RolesController
        //Where we are getting the selected Role from the database and calling Remove method of the Roles collection. Calling SaveChanges method of 
        //the ApplicationDbContext object deletes the selected role from the database.
        public ActionResult Delete(string RoleName)
        {
            var thisRole = db.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            db.Roles.Remove(thisRole);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        //the first method accept roleName as parameter and based on that we retrieve the role from the database and returns to the View.
        //The second method accept IdentityRole as parameter and update the record to the database.
        // GET: /Roles/Edit/5
        public ActionResult Edit(string roleName)
        {
            var thisRole = db.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            return View(thisRole);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Microsoft.AspNet.Identity.EntityFramework.IdentityRole role)
        {
            try
            {
                db.Entry(role).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //To manage role for a particular user in ASP.NET Identity, we have below view that has more than one form to 
        //Add a role to the user,Get roles for a user and, Delete/Detach a user from a particular role
        public ActionResult ManageUserRoles()
        {
            // prepopulat roles for the view dropdown
            var Roleslist = db.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roleslist = Roleslist;
            var Userslist = db.Users.OrderBy(r => r.UserName).ToList().Select(rr => new SelectListItem { Value = rr.UserName.ToString(), Text = rr.UserName }).ToList();
            ViewBag.Userslist = Userslist;
            return View();
        }


        //Where the first Form has simply a UserName textbox and a RoleName dropdown list that contains current Roles
        //from the database. Submitting the form sends form data to RoleAddToUser action method of the RolesController.
        //In the below method, we are getting UserName and RoleName as parameter.
        //UserName is being used to get the ApplicationUser from context.Users collection.
        //Next few lines of codes are just to list the roles in the DropDown list again as we are returning to the same (ManageUserRoles) view again.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            ApplicationUser user = db.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            userManager.AddToRole(user.Id, RoleName);

            ViewBag.ResultMessage = "Role created successfully !";

            // prepopulat roles for the view dropdown
            var RolesList = db.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.RolesList = RolesList;
            var Userslist = db.Users.OrderBy(r => r.UserName).ToList().Select(rr => new SelectListItem { Value = rr.UserName.ToString(), Text = rr.UserName }).ToList();
            ViewBag.Userslist = Userslist;

            return View("ManageUserRoles");
        }

        //In the above code snippet, we are getting the ApplicationUser object using the UserName. 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = db.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                var userStore = new UserStore<ApplicationUser>(db);
                var userManager = new UserManager<ApplicationUser>(userStore);

                ViewBag.RolesForThisUser = userManager.GetRoles(user.Id);

                // prepopulat roles for the view dropdown
                var RolesList = db.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.RolesList = RolesList;
                var Userslist = db.Users.OrderBy(r => r.UserName).ToList().Select(rr => new SelectListItem { Value = rr.UserName.ToString(), Text = rr.UserName }).ToList();
                ViewBag.Userslist = Userslist;

            }

            return View("ManageUserRoles");
        }


        //In the above code snippet, we are getting ApplicationUser and then checking whether this user
        //belongs to the selected Role or not (using AccountController object), if it is then calling the 
        //RemoveFromRole method by passing UserId and RoleName parameter that removes the user from the role.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            ApplicationUser user = db.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (userManager.IsInRole(user.Id, RoleName))
            {
                userManager.RemoveFromRole(user.Id, RoleName);
                ViewBag.ResultMessage = "Role removed from this user successfully !";
            }
            else
            {
                ViewBag.ResultMessage = "This user doesn't belong to selected role.";
            }
            // prepopulat roles for the view dropdown
            var RolesList = db.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.RolesList = RolesList;
            var Userslist = db.Users.OrderBy(r => r.UserName).ToList().Select(rr => new SelectListItem { Value = rr.UserName.ToString(), Text = rr.UserName }).ToList();
            ViewBag.Userslist = Userslist;

            return View("ManageUserRoles");
        }
    }




}