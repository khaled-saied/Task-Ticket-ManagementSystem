using System.Threading.Tasks;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ticket_ManagementSystem.ViewModels.RoleViewModel;

namespace Ticket_ManagementSystem.Controllers
{
    public class RoleController(RoleManager<IdentityRole> _roleManager,
                                UserManager<ApplicationUser> _userManager) : Controller
    {
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            var roleViewModels = roles.Select(role => new ReturnRoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            }).ToList();

            return View(roleViewModels);
        }

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateRoleViewModel createRoleView)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole
                {
                    Name = createRoleView.Name
                };
                var result = _roleManager.CreateAsync(role).Result;
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Role created successfully.";
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    TempData["ErrorMessage"] = error.Description;
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(createRoleView);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            var editRoleViewModel = new ReturnRoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(editRoleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ReturnRoleViewModel editRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(editRoleViewModel.Id);
                if (role == null) return NotFound();

                role.Name = editRoleViewModel.Name;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Role updated successfully.";
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    TempData["ErrorMessage"] = error.Description;
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(editRoleViewModel);
        }

        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var role = await _roleManager.Roles.FirstOrDefaultAsync(u => u.Id == id);
                if (role == null) return NotFound();

                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Role deleted successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to delete role. Please try again.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while deleting the role: {ex.Message}");
            }

            return RedirectToAction("Index");
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUserRole(string roleId)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null) return NotFound();

            ViewData["RoleName"] = roleId;

            var UsersInRole = new List<UsersInRoleViewModel>();

            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userinRole = new UsersInRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                var isInRole = await _userManager.IsInRoleAsync(user, role.Name)
                             ? userinRole.IsSelected=true : userinRole.IsSelected = false;

                UsersInRole.Add(userinRole);
            }


            return View(UsersInRole);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUserRole(string roleId ,List<UsersInRoleViewModel> users)
        {
            var role =await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
            if (role == null) return NotFound();

            if (ModelState.IsValid)
            {
                foreach(var user in users)
                {
                    var userApp = await _userManager.FindByIdAsync(user.UserId);

                    if(userApp is not null)
                    {
                        if (user.IsSelected && ! await _userManager.IsInRoleAsync(userApp,role.Name) )
                        {
                            await _userManager.AddToRoleAsync(userApp, role.Name);
                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(userApp, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(userApp,role.Name);
                        }
                    }

                }

                return RedirectToAction("Edit", new {id= roleId});
            }

            return View(users);
        }

    }
}
