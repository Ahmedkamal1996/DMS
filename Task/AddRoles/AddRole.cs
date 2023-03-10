using DMSTask.BLL.Helpers;
using DMSTask.DAL.Database;
using Microsoft.AspNetCore.Identity;

namespace DMSTask.AddRoles
{
 
    public static class AddRole
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();

            string[] roles = new string[] { Roles.Admin, Roles.Customer };

            foreach (string role in roles)
            {
                if (!context.Roles.Any(r => r.Name == role))
                {
                    context.Roles.Add(new IdentityRole() { Name = role, NormalizedName = role.ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString("D") });
                }
            }

            //// Admin
            //var admin = new IdentityUser
            //{
            //    Email = "admin@admin.com",
            //    NormalizedEmail = "ADMIN@ADMIN.COM",
            //    UserName = "Admin",
            //    NormalizedUserName = "ADMIN",
            //    PhoneNumber = "+111111111111",
            //    EmailConfirmed = true,
            //    PhoneNumberConfirmed = true,
            //    SecurityStamp = Guid.NewGuid().ToString("D")
            //};


            //if (!context.Users.Any(u => u.UserName == admin.UserName))
            //{
            //    var password = new PasswordHasher<IdentityUser>();
            //    var hashed = password.HashPassword(admin, "Qwe@123");
            //    admin.PasswordHash = hashed;

            //    var userStore = new UserStore<IdentityUser>(context);
            //    var result = userStore.CreateAsync(admin);
            //}

            //AssignRoles(serviceProvider, admin.Email, new string[] { DmsRoles.ADMIN});

            context.SaveChanges();
        }

        //public static async Task<IdentityResult> AssignRoles(IServiceProvider services, string email, string[] roles)
        //{
        //    UserManager<IdentityUser> _userManager = services.GetService<UserManager<IdentityUser>>();
        //    IdentityUser user = await _userManager.FindByEmailAsync(email);
        //    var result = await _userManager.AddToRolesAsync(user, roles);

        //    return result;
        //}
    }
}

