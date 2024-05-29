using CoreIdentityComplete_0.Areas.Administrator.Models.AppRoles.RequestModels;
using CoreIdentityComplete_0.Models.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreIdentityComplete_0.Areas.Administrator.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administrator")]
    public class RoleController : Controller
    {
        readonly RoleManager<AppRole> _roleManager;
        readonly IValidator<CreateRoleRequestModel> _createRoleValidator;

        public RoleController(RoleManager<AppRole> roleManager, IValidator<CreateRoleRequestModel> createRoleValidator)
        {
            _roleManager = roleManager;
            _createRoleValidator = createRoleValidator;
        }

        public IActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleRequestModel model)
        {
            ValidationResult validationResult = _createRoleValidator.Validate(model);
            if (validationResult.IsValid)
            {
                IdentityResult result = await _roleManager.CreateAsync(new AppRole { Name = model.RoleName });
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");

                }
                foreach (IdentityError error in result.Errors)
                {

                    ModelState.AddModelError("", error.Description);

                }
            }
            else
            {
                foreach (ValidationFailure validationError in validationResult.Errors) ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                
            }
            return View(model);
        }
    }
}
