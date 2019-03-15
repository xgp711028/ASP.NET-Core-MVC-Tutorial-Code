using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Heavy.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Heavy.Web.Controllers
{
	public class RoleController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public RoleController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task<IActionResult> Index()
		{
			var roles = await _roleManager.Roles.ToListAsync();
			return View(roles);
		}
	}
}