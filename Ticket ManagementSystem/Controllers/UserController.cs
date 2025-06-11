using System.Threading.Tasks;
using BLL.DataTransferObjects.UserDtos;
using BLL.Services.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ticket_ManagementSystem.Controllers
{
    public class UserController(UserManager<ApplicationUser> _userManager,
                                IServiceManger _serviceManger) : Controller
    {

        #region Index
        public async Task<IActionResult> Index(string searchVal)
        {
            var users = await _serviceManger.UserService.GetAllUsersAsync(searchVal);
            return View(users);
        }
        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var userDetails = await _serviceManger.UserService.GetUserDetailsAsync(id);

            return View(userDetails);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();
            var userRoles = await _userManager.GetRolesAsync(user);
            var userDto = new UpdateUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email
            };
            return View(userDto);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserDto updateUser)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == updateUser.Id);
                    if (user == null)
                        ModelState.AddModelError(string.Empty, "User not found.");

                    var result = await _serviceManger.UserService.UpdateUserAsync(updateUser);
                    if (result > 0) return RedirectToAction("Index");

                    ModelState.AddModelError(string.Empty, "Failed to update user. Please try again.");
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Optimistic concurrency failure"))
                    {
                        ModelState.AddModelError(string.Empty, "This user was modified by someone else. Please reload and try again.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"An error occurred while updating the user: {ex.Message}");
                    }
                }
            }
            return View(updateUser);
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
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    return NotFound();
                }

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to delete user. Please try again.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while deleting the user: {ex.Message}");
            }

            return RedirectToAction("Index");
        } 
        #endregion

    }
}
