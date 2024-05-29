using CoreIdentityComplete_0.Models;
using CoreIdentityComplete_0.Models.Entities;
using CoreIdentityComplete_0.Models.FluentValidation;
using CoreIdentityComplete_0.Models.ViewModels.AppUsers;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace CoreIdentityComplete_0.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly RoleManager<AppRole> _roleManager;
        readonly IValidator<UserRegisterRequestModel> _userRegisterRequestValidator;
        readonly IValidator<UserSignInRequestModel> _userSignInRequestValidator;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IValidator<UserRegisterRequestModel> userRegisterRequestValidator, IValidator<UserSignInRequestModel> userSignInRequestValidator)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userRegisterRequestValidator = userRegisterRequestValidator;
            _userSignInRequestValidator = userSignInRequestValidator;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterRequestModel model)
        {

            ValidationResult validationResult = _userRegisterRequestValidator.Validate(model);
            if (validationResult.IsValid)
            {
                AppUser appUser = new()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                };

                IdentityResult result = await _userManager.CreateAsync(appUser, model.Password);

                if (result.Succeeded)
                {
                    #region AdminEklemekIcinKodlar
                    //AppRole role = await _roleManager.FindByNameAsync("Admin");
                    //if (role == null) await _roleManager.CreateAsync(new() { Name = "Admin" });
                    //await _userManager.AddToRoleAsync(appUser, "Admin");
                    #endregion

                    AppRole role = await _roleManager.FindByNameAsync("Member");
                    if (role == null) await _roleManager.CreateAsync(new() { Name = "Member" });
                    await _userManager.AddToRoleAsync(appUser, "Member");
                    return RedirectToAction("Register");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            else
            {
                foreach (ValidationFailure validationError in validationResult.Errors)
                {
                    ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                }
            }

            return View(model);
        }


        public IActionResult SignIn(string returnUrl)
        {
            UserSignInRequestModel usModel = new() { ReturnUrl = returnUrl };
            return View(usModel);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInRequestModel model)
        {
            ValidationResult validationResult = _userSignInRequestValidator.Validate(model);

            if (validationResult.IsValid)
            {
                AppUser appUser = await _userManager.FindByNameAsync(model.UserName);
                if (appUser == null)
                {
                    ModelState.AddModelError("", "Kullanıcı bulunamadı");
                    return View(model);
                }
                SignInResult result = await _signInManager.PasswordSignInAsync(appUser, model.Password, model.RememberMe, true);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);

                    }

                    IList<string> roles = await _userManager.GetRolesAsync(appUser);

                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("AdminPanel", "Auth");
                    }
                    else if (roles.Contains("Member")) return RedirectToAction("MemberPanel");
                    return RedirectToAction("Panel");
                }

                else if (result.IsLockedOut)
                {
                    DateTimeOffset? lockOutEndDate = await _userManager.GetLockoutEndDateAsync(appUser);
                    ModelState.AddModelError("", $"Hesabınız {(lockOutEndDate.Value.UtcDateTime - DateTime.UtcNow).Minutes} dakika süreyle askıya alınmıstır");
                }

                else
                {


                    int maxFailedAttempts = _userManager.Options.Lockout.MaxFailedAccessAttempts;
                    string message = $"Eger {maxFailedAttempts - await _userManager.GetAccessFailedCountAsync(appUser)} kez daha yanlıs giriş yaparsanız hesabınız gecici olarak askıya alanacaktır";



                    ModelState.AddModelError("", message);
                }

            }
            else
            {
                foreach (ValidationFailure validationError in validationResult.Errors)
                {

                    ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);

                }
            }
            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Member")]
        public IActionResult MemberPanel()
        {
            return View();
        }

        public IActionResult Panel()
        {
            return View();
        }
    }
}
