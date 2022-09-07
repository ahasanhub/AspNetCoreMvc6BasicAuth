using AspNetCore6BasicAuth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AspNetCore6BasicAuth.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public IActionResult Login(string returnUrl="/")
        {
            return View(new LoginModel { ReturnUrl=returnUrl});
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model) {
            if (ModelState.IsValid)
            {

                ClaimsIdentity? identity = null;
                bool isAuthenticate = false;
                if (model.Username == "admin" && model.Password == "123456")
                {
                    var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier,"Admin"),
                    new Claim(ClaimTypes.Name,"Admin"),
                    new Claim(ClaimTypes.Email,"admin@gmail.com"),
                    new Claim(ClaimTypes.Role,"Admin"),
                    new Claim(ClaimTypes.Role,"AdminTwo"),
                    };
                    identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    isAuthenticate = true;
                }
               if(model.Username == "ahasan" && model.Password == "123456")
                {
                    var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier,"Ahasan"),
                    new Claim(ClaimTypes.Name,"Ahasan"),
                    new Claim(ClaimTypes.Email,"ahasan@gmail.com"),
                    new Claim(ClaimTypes.Role,"User"),
                    new Claim(ClaimTypes.Role,"UserTwo"),
                    };
                    identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    isAuthenticate = true;
                }
                if (isAuthenticate)
                {
                    var principal = new ClaimsPrincipal(new[] { identity });
                   await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = model.RememberLogin });
                }
                
               return LocalRedirect(model.ReturnUrl);
                
                
            }
            else
            {
                Unauthorized();
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
            //return LocalRedirect(model.ReturnUrl);
        }

        public async Task<IActionResult> Logout()
        {
           await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Home");
        }
    }
}
