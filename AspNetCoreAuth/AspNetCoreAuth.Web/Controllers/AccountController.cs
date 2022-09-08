
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AspNetCoreAuth.Web.Models;
using AspNetCoreAuth.Data.Repositories;

namespace AspNetCoreAuth.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "/")
        {
            return View(new LoginModel { ReturnUrl = returnUrl });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userRepository.GetByUsernameAndPassword(model.Username,model.Password);
                if (user == null)
                    return Unauthorized();
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("FavoriteColor", user.FavoriteColor)
            };

             var identity=new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
             var principal=new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal,new AuthenticationProperties { IsPersistent=model.RememberLogin});
                return LocalRedirect(model.ReturnUrl);

            }
            else
            {
                Unauthorized();
                ModelState.AddModelError("LoginError","Invalid login attempt.");
                return View(model);
            }
            #region old Logic
            //    ClaimsIdentity? identity = null;
            //    bool isAuthenticate = false;
            //    if (model.Username == "admin" && model.Password == "123456")
            //    {
            //        var claims = new List<Claim>() {
            //        new Claim(ClaimTypes.NameIdentifier,"Admin"),
            //        new Claim(ClaimTypes.Name,"Admin"),
            //        new Claim(ClaimTypes.Email,"admin@gmail.com"),
            //        new Claim(ClaimTypes.Role,"Admin"),
            //        new Claim(ClaimTypes.Role,"AdminTwo"),
            //        };
            //        identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //        isAuthenticate = true;
            //    }
            //    if (model.Username == "ahasan" && model.Password == "123456")
            //    {
            //        var claims = new List<Claim>() {
            //        new Claim(ClaimTypes.NameIdentifier,"Ahasan"),
            //        new Claim(ClaimTypes.Name,"Ahasan"),
            //        new Claim(ClaimTypes.Email,"ahasan@gmail.com"),
            //        new Claim(ClaimTypes.Role,"User"),
            //        new Claim(ClaimTypes.Role,"UserTwo"),
            //        };
            //        identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //        isAuthenticate = true;
            //    }
            //    if (isAuthenticate)
            //    {
            //        var principal = new ClaimsPrincipal(new[] { identity });
            //        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = model.RememberLogin });
            //    }

            //    return LocalRedirect(model.ReturnUrl);


            //}
            //else
            //{
            //    Unauthorized();
            //    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            //    return View(model);
            //}
            //return LocalRedirect(model.ReturnUrl);
            #endregion
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //return RedirectToAction("Index", "Home");
            return Redirect("/");
        }
    }
}
