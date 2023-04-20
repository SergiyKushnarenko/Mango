using Mango.Services.Identity.DbContexts;
using Mango.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.Identity.Initializer
{
	public class DbInitializer : IDbInitializer
	{
		private readonly ApplicationDbContext _Db;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_Db = db;
			_userManager = userManager;
			_roleManager = roleManager;
		}
		public void Initialize()
		{
			if(_roleManager.FindByNameAsync(SD.Admin).Result == null )
			{
				_roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
				_roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();
			}
			else { return; }

			ApplicationUser adminUser = new ()
			{
				UserName = "admin1@gmail.com",
				Email = "admin1@gmail.com",
				EmailConfirmed = true,
				PhoneNumber = "111111111111",
				FirstName = "Ben",
				LastName = "Admin"
			};
		}
	}
}
