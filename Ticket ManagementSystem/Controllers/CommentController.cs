using System.Threading.Tasks;
using BLL.DataTransferObjects.CommentDtos;
using BLL.Services.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ticket_ManagementSystem.Controllers
{
    public class CommentController(IServiceManger _serviceManger,
                                   ILogger<CommentController> _logger,
                                   IWebHostEnvironment _environment,
                                   UserManager<ApplicationUser> _userManager) : Controller
    {
        #region Index
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
            return View(comment);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create(int ticketId)
        {
            ViewBag.TicketId = ticketId;
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

    }
}
