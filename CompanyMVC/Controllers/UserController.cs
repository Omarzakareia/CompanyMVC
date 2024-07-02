using AutoMapper; // For object mapping
using DAL.Models; // For data access layer models
using Microsoft.AspNetCore.Authorization; // For authorization attributes
using Microsoft.AspNetCore.Identity; // For identity management
using Microsoft.AspNetCore.Mvc; // For MVC controllers and actions
using Microsoft.EntityFrameworkCore; // For Entity Framework Core
using CompanyMVC.ViewModels; // For view models specific to the MVC project


namespace CompanyMVC.Controllers
{
    [Authorize(Roles = "Admin,HR")] // Requires users to be authenticated to access actions in this controller
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager; // Manages user-related operations
        private readonly IMapper _mapper; // Maps objects to different types

        public UserController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;   // Dependency injection for UserManager
            _mapper = mapper;             // Dependency injection for AutoMapper
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                var Users = await _userManager.Users.Select(
                    U => new UserViewModel()
                    {
                        Id = U.Id,
                        FName = U.FName,
                        LName = U.LName,
                        Email = U.Email,
                        PhoneNumber = U.PhoneNumber,
                        Roles = _userManager.GetRolesAsync(U).Result
                    }).ToListAsync();
                return View(Users);
            }
            else
            {
                var User = await _userManager.FindByEmailAsync(SearchValue);
                var MappedUser = new UserViewModel()
                {
                    Id = User.Id,
                    FName = User.FName,
                    LName = User.LName,
                    Email = User.Email,
                    PhoneNumber = User.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(User).Result
                };
                return View(new List<UserViewModel> { MappedUser });
            }
        }

        public async Task<IActionResult> Details(string Id, string ViewName = "Details")
        {
            if (Id is null)
                return BadRequest();
            var User = await _userManager.FindByIdAsync(Id);
            if (User == null)
                return NotFound();
            var MappedUser = _mapper.Map<ApplicationUser, UserViewModel>(User);
            return View(ViewName,MappedUser);
        }

        public async Task<IActionResult> Edit(string Id)
        {
            return await Details(Id , "Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model , [FromRoute] string Id)
        {
            if(Id != model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var User = await _userManager.FindByIdAsync (Id);
                    User.PhoneNumber = model.PhoneNumber;
                    User.FName = model.FName;
                    User.LName = model.LName;
                    await _userManager.UpdateAsync(User);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty , ex.Message);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string Id)
        {
            return await Details(Id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string Id)
        {
            try
            {
                var User = await _userManager.FindByIdAsync(Id);
                await _userManager.DeleteAsync(User);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
