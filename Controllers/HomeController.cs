using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectUrlShort.Data;
using ProjectUrlShort.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace ProjectUrlShort.Controllers
{
	public class HomeController : Controller
	{
		private readonly SignInManager<IdentityUser> SignInManager;
		private readonly ILogger<HomeController> _logger;
		private readonly UrlDBcontext _dBcontext;
		public HomeController(ILogger<HomeController> logger, UrlDBcontext dBcontext)
		{
			_dBcontext = dBcontext;
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}
		[Authorize]
		public IActionResult Links()
		{
			var currentUser = HttpContext.User;
			var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
			var tempUser = _dBcontext.Users.FirstOrDefault(u => u.Id == userId);
			var data = _dBcontext.Urls.Where(X=>X.UserUrl.Equals(tempUser)).ToList();
			return View(data);	
		}
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}