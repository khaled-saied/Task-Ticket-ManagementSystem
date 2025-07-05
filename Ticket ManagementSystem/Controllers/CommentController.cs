using System.Threading.Tasks;
using BLL.DataTransferObjects.CommentDtos;
using BLL.DataTransferObjects.TicketDtos;
using BLL.Services.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ticket_ManagementSystem.Controllers
{
    [Authorize]
    public class CommentController(IServiceManger _serviceManger,
                                   ILogger<CommentController> _logger,
                                   IWebHostEnvironment _environment,
                                   UserManager<ApplicationUser> _userManager) : Controller
    {
        #region Index
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            var Comments = await _serviceManger.CommentService.GetAllCommentsAsync();
            return View(Comments);
        }


        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var comment = await _serviceManger.CommentService.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Admin"))
                ViewBag.Layout = "~/Views/Shared/_DashboardLayout.cshtml";
            else
                ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";

            return View(comment);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create(int ticketId)
        {
            ViewBag.TicketId = ticketId;

            if (User.IsInRole("Admin"))
                ViewBag.Layout = "~/Views/Shared/_DashboardLayout.cshtml";
            else
                ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int ticketId, CreateCommentDto commentDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(User);
                    if (ticketId > 0 && userId is not null)
                    {
                        var Result = await _serviceManger.CommentService.CreateCommentAsync(commentDto, ticketId, userId);
                        if(Result > 0)
                        {
                            TempData["Success"] = "Comment created successfully.";
                            return RedirectToAction("Details", "Ticket", new { id = ticketId });
                        }
                        else
                        {
                            TempData["Error"] = "Failed to create comment.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (_environment.IsDevelopment())
                    {
                        ModelState.AddModelError(string.Empty, "An error occurred while creating the comment.");
                    }
                    else
                    {
                        _logger.LogError(string.Empty, ex.Message);
                    }
                }
            }
            return View(commentDto);
        }

        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Edit(int id, int ticketId)
        {
            ViewBag.TicketId = ticketId;
            var comment = await _serviceManger.CommentService.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            if (comment.UserId == userId || User.IsInRole("SuperAdmin")) // Check if the user is the owner of the comment
            {
                var commentDto = new UpdateCommentDto
                {
                    Id = comment.Id,
                    Content = comment.Content,
                };
                if (User.IsInRole("Admin"))
                    ViewBag.Layout = "~/Views/Shared/_DashboardLayout.cshtml";
                else
                    ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";

                return View(commentDto);
            }
            else
              return View("AccessDenied", comment.TicketDto.Id);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCommentDto commentDto,int ticketId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int result = await _serviceManger.CommentService.UpdateCommentAsync(commentDto);
                    if (result > 0)
                        return RedirectToAction("Details", "Ticket", new { id = ticketId });
                    else
                        ModelState.AddModelError(string.Empty, "Department was not updated");
                }
                catch (Exception ex)
                {
                    if (_environment.IsDevelopment())
                    {
                        //1- Devolpment=> log error in console and return error message to user
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                    else
                    {
                        //2-Deployment=> log error in file or database and return  error view,
                        _logger.LogError(ex.Message);
                        return View("ErrorView", ex);
                    }

                }
            }
            return View(commentDto);
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0 )
            {
                return NotFound();
            }
            var comment = await _serviceManger.CommentService.GetCommentByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            if (comment.UserId != userId)// Check if the user is the owner of the comment
                return View("AccessDenied");

            // Check if the user is an admin
            if (User.IsInRole("Admin"))
                ViewBag.Layout = "~/Views/Shared/_DashboardLayout.cshtml";
            else
                ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";

            return View(comment);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(CommentDetailsDto detailsDto)
        {
            try 
            {
                bool result = await _serviceManger.CommentService.DeleteCommentAsync(detailsDto.Id);
                if (result)
                {
                    return RedirectToAction("Details", "Ticket", new { id = detailsDto.TicketDto.Id });
                }
                else
                {
                    TempData["Error"] = "Failed to delete comment.";
                }
            }
            catch (Exception ex)
            {
                if (_environment.IsDevelopment())
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                else
                {
                    _logger.LogError(ex.Message);
                    return View("ErrorView", ex);
                }
            }
            return View();
        }

        #endregion

    }
}
