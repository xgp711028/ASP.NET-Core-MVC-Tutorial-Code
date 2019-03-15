using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Heavy.Web.Models;
using Heavy.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Heavy.Web.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public UserController(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var users = await _userManager.Users.ToListAsync();
			return View(users);
		}

		public IActionResult Add()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Add(UserCreateViewModel userCreateViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(userCreateViewModel);
			}
			var user = new ApplicationUser
			{
				UserName = userCreateViewModel.UserName,
				Email = userCreateViewModel.Email
			};
			var result = await _userManager.CreateAsync(user, userCreateViewModel.Password);
			if (result.Succeeded)
			{
				return RedirectToAction(nameof(Index), await _userManager.Users.ToListAsync());
			}
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
			return View(userCreateViewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user != null)
			{
				var result = await _userManager.DeleteAsync(user);
				if (result.Succeeded)
				{
					return RedirectToAction(nameof(Index));
				}
				ModelState.AddModelError(string.Empty, "删除用户时发生错误");
			}
			else
			{
				ModelState.AddModelError(string.Empty, "用户找不到");
			}
			return View(nameof(Index), await _userManager.Users.ToListAsync());
		}
	}
}