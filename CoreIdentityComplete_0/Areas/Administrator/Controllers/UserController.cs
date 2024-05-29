using CoreIdentityComplete_0.Areas.Administrator.Models.AppRoles.PageVMs;
using CoreIdentityComplete_0.Areas.Administrator.Models.AppRoles.ResponseModels;
using CoreIdentityComplete_0.Areas.Administrator.Models.AppUsers;
using CoreIdentityComplete_0.Models.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreIdentityComplete_0.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        readonly UserManager<AppUser> _userManager;
        readonly RoleManager<AppRole> _roleManager;
        readonly IValidator<CreateUserRequestModel> _createUserValidator;
        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IValidator<CreateUserRequestModel> createUserValidator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _createUserValidator = createUserValidator;
        }


        public  IActionResult Index()
        {
            List<AppUser> allUsers =  _userManager.Users.ToList();
            return View(allUsers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserRequestModel model)
        {
            ValidationResult validationResult = _createUserValidator.Validate(model);
            if (validationResult.IsValid)
            {
                AppUser appUser = new()
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                IdentityResult result = await _userManager.CreateAsync(appUser, $"{model.UserName}cgr123");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(appUser, "Member");
                    return RedirectToAction("Index");
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

        public async Task<IActionResult> AssignRole(int id)
        {
            AppUser appUser = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == id);

            IList<string> userRoles = await _userManager.GetRolesAsync(appUser); //Elimize geçen kullanıcının rollerini verir
            List<AppRole> allRoles = await _roleManager.Roles.ToListAsync(); // bütün yoller
            List<AppRoleResponseModel> responseRoles = new();

            foreach (AppRole item in allRoles)
            {
                responseRoles.Add(new()
                {
                   
                    RoleName = item.Name,
                    IsChecked = userRoles.Contains(item.Name)
                });
            }

            AssignRolePageVM aRPvm = new()
            {
                UserID = id,
                Roles = responseRoles
            };

            return View(aRPvm);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(AssignRolePageVM model)
        {
            AppUser appUser = await _userManager.Users.SingleOrDefaultAsync(x=>x.Id== model.UserID);
            IList<string> userRoles = await _userManager.GetRolesAsync(appUser);

            foreach (AppRoleResponseModel role in model.Roles)
            {
                if (role.IsChecked && !userRoles.Contains(role.RoleName)) await _userManager.AddToRoleAsync(appUser, role.RoleName);
                else if (!role.IsChecked && userRoles.Contains(role.RoleName)) await _userManager.RemoveFromRoleAsync(appUser, role.RoleName);
            }

            return RedirectToAction("Index");
        }
    }
}
