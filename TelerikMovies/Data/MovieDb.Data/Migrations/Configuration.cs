namespace TelerikMovies.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Configuration;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Linq;
    public sealed class Configuration : DbMigrationsConfiguration<MoviesContext>
    {
        private const string Role = "Admin";
        private readonly string administratorUserName;
        private readonly string administratorPassword;
        private readonly string administratorFirstName;
        private readonly string administratorLastName;
        private readonly bool administratorIsMale;

        public Configuration()
            
        {      
            this.AutomaticMigrationsEnabled = false;
            this.AutomaticMigrationDataLossAllowed = false;
            administratorUserName= ConfigurationManager.AppSettings["AdministratorUserName"];
            administratorPassword= ConfigurationManager.AppSettings["AdministratorPassword"];
            administratorFirstName = ConfigurationManager.AppSettings["AdministratorFirstName"];
            administratorLastName = ConfigurationManager.AppSettings["AdministratorLastName"];
            administratorIsMale = bool.Parse(ConfigurationManager.AppSettings["AdministratorIsMale"]);
        }

        protected override void Seed(MoviesContext context)
        {
            SeedUsers(context);
        }

        private void SeedUsers(MoviesContext context)
        {

            if (!context.Roles.Any())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var role = new IdentityRole { Name = Role };
                roleManager.Create(role);

                var userStore = new UserStore<Users>(context);
                var userManager = new UserManager<Users>(userStore);
                var user = new Users
                {
                    UserName = this.administratorUserName,
                    Email = this.administratorUserName,
                    EmailConfirmed = true,
                    CreatedOn = DateTime.Now,
                    FirstName = this.administratorFirstName,
                    LastName = this.administratorLastName,
                    isMale = this.administratorIsMale
                };

                userManager.Create(user, this.administratorPassword);
                userManager.AddToRole(user.Id, Role);
            }
        }
    }
}
