using System.Security.Claims;
using System.Threading.Tasks;
using Bomega.WebApp.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bomega.WebApp.Controllers
{

    public class AccountController : Controller
    {
        public AccountController() { }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLogin( [FromQuery] string provider )
        {
            if( string.IsNullOrWhiteSpace( provider ) )
            {
                return BadRequest();
            }

            string redirectUri = Url.Action( nameof( ExternalLoginCallback ), "Account" );
            return Challenge( new AuthenticationProperties { RedirectUri = redirectUri }, provider );
        }

        [HttpGet]
        [Authorize( AuthenticationSchemes = CookieAuthentication.AuthenticationScheme )]
        //[Authorize(AuthenticationSchemes = CookieAuthentication.AuthenticationScheme)]
        public IActionResult ExternalLoginCallback()
        {
            return RedirectToAction( nameof( Authenticated ) );
        }

        [HttpGet]
        [Authorize( AuthenticationSchemes = CookieAuthentication.AuthenticationScheme )]
        public async Task<IActionResult> Authenticated()
        {
            string userId = User.FindFirst( ClaimTypes.NameIdentifier ).Value;
            string email = User.FindFirst( ClaimTypes.Email ).Value;
            //Token token = _tokenService.GenerateToken(userId, email);
            //IEnumerable<string> providers = await _userGateway.GetAuthenticationProviders(userId);
            //ViewData["BreachPadding"] = GetBreachPadding(); // Mitigate BREACH attack. See http://www.breachattack.com/
            //ViewData["Token"] = token;
            //ViewData["Email"] = email;
            //ViewData["NoLayout"] = true;
            //ViewData["Providers"] = providers;
            return View();
        }
    }
}