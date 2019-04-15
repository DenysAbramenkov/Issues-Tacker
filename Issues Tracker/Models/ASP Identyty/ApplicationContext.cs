using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Issues_Tracker.Models
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext() : base("IdentityDb") {
            Database.SetInitializer<ApplicationContext>(new CreateDatabaseIfNotExists<ApplicationContext>());
        }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }
    }
}