using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjectUrlShort.Data;
using ProjectUrlShort.Models;
using System.Security.Claims;

namespace ProjectUrlShort.Controllers
{
	[Route("api")]
	[ApiController]
	public class Apishortcontroller : ControllerBase
	{
		private readonly UrlDBcontext _dBcontext;
		public Apishortcontroller(UrlDBcontext urlDBcontext)
		{
			_dBcontext = urlDBcontext;
		}
		[HttpPost("shorten")]
		public ActionResult Shorten([FromBody] string fullurl)
		{

			if (!string.IsNullOrEmpty(fullurl))
			{
				Uri uriResult;
				bool result = Uri.TryCreate(fullurl, UriKind.Absolute, out uriResult)
					&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

				if (!result)
				{
					return BadRequest();
				}

				var currentUser = HttpContext.User;
				var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
				var tempUser = _dBcontext.Users.FirstOrDefault(u => u.Id == userId);

				string temp = "";
				string shorturl = "https://localhost:7121/w/";

				if (tempUser != null && _dBcontext.Urls.Any(u => u.FullUrl == fullurl && u.UserUrl.Equals(tempUser)))
				{
					shorturl += _dBcontext.Urls.First(u => u.FullUrl == fullurl && u.UserUrl.Equals(tempUser)).ShortUrl;
					return Ok(shorturl);
				}
				else if (tempUser == null && _dBcontext.Urls.Any(u => u.FullUrl == fullurl && u.UserUrl==null ))
				{
					shorturl += _dBcontext.Urls.First(u => u.FullUrl == fullurl && u.UserUrl == null).ShortUrl;
					return Ok(shorturl);
				}
				else
				{
					URL model = new URL();
					Random random = new Random();
					do
					{
						string arr = "abcdefghigklmnopqrstuvwxyzABCDEFGHIGKLMNOPQRSTUVWXYZ0123456789";
						var ans = Enumerable.Range(6, 10)
						.Select(x => arr[random.Next(0, arr.Length)]);
						temp = new string(ans.ToArray());
					} while (_dBcontext.Urls.Any(u => u.ShortUrl == temp));

					model.ShortUrl = temp;
					model.FullUrl = fullurl;
					model.UserUrl = tempUser;
					_dBcontext.Urls.Add(model);
					_dBcontext.SaveChanges();
					return Ok(shorturl + temp);
				}
			}
			else { return BadRequest(); }
		}
		[HttpGet("/w/{shorturl}")]
		public ActionResult RediracteShort(string shorturl)
		{

			var ob = _dBcontext.Urls.FirstOrDefault(u => u.ShortUrl == shorturl);
			if (ob != null)
			{
				ob.NumOfRequest++;
				_dBcontext.SaveChanges();
				return Redirect(ob.FullUrl);
			}
			else { return BadRequest(); }
		}
	}
}
