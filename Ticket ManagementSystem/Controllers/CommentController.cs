using System.Threading.Tasks;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ticket_ManagementSystem.Controllers
{
    public class CommentController(IServiceManger _serviceManger,
                                   ILogger<CommentController> _logger,
                                   IWebHostEnvironment _environment) : Controller
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

    }
}
