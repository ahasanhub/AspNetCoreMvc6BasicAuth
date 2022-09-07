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
                
                if (model.Username == "admin" && model.Password == "123456")
                {
                    var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier,"Ahasan"),
                    new Claim(ClaimTypes.Name,"Ahasan"),
                    new Claim(ClaimTypes.Email,"ahasan@gmail.com"),
                    new Claim(ClaimTypes.Role,"Admin"),
                    };
                   var identity1 = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                                    
                    var principal=new ClaimsPrincipal(new[] {identity1});
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = model.RememberLogin });
                    return LocalRedirect(model.ReturnUrl);
                }
                else
                {
                    Unauthorized();
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            return LocalRedirect(model.ReturnUrl);
        }
    }
}
