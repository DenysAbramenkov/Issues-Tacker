namespace Issues_Tracker.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Issues_Tracker.Models.ApplicationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            if (!context.Roles.Any(r => r.Name == "Admin"))
            { 
                var role = new IdentityRole { Name = "Admin" };
                roleManager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                var role = new IdentityRole { Name = "Developer" };
                roleManager.Create(role);
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            if (!context.Users.Any(u => u.Email == "den_abr@ukr.net"))
            {  
                var user = new ApplicationUser { UserName = "den_abr@ukr.net" };
                userManager.Create(user, "123456");
                userManager.AddToRole(user.Id, "Admin");
            }
            if (!context.Users.Any(u => u.Email == "denabr969@gmail.com"))
            {
                var user = new ApplicationUser { UserName = "denabr969@gmail.com" };
                userManager.Create(user, "123456");
                userManager.AddToRole(user.Id, "Developer");
            }
        }
    }
}
