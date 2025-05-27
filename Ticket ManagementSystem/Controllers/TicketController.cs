using System.Threading.Tasks;
using BLL.DataTransferObjects.TicketDtos;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ticket_ManagementSystem.ViewModels.ViewModelOfTicket;

namespace Ticket_ManagementSystem.Controllers
{
    public class TicketController(IServiceManger _serviceManger,
                                  ILogger<TicketController> _logger,
                                  IWebHostEnvironment _environment) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var tickets = await _serviceManger.TicketService.GetAllAsync();
            return View(tickets);
        }

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create(int? TaskId,string returnUrl)
        {

            var viewModel = new CraeteTicketViewModel
            {
                TaskId = TaskId ?? 0,
                Tasks = await GetTaskSelectListAsync(),
                RetutnUrl = returnUrl
            };

            return View(viewModel);
        }

        private async Task<List<SelectListItem>> GetTaskSelectListAsync()
        {
            var tasks = await _serviceManger.TaskService.GetAllTasks();
            return tasks.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Title
            }).ToList(); 
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTicketDto createTicketDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int Result = await _serviceManger.TicketService.CreateTicket(createTicketDto);

                    string Message;
                    if(Result > 0)
                        Message = $"Department {createTicketDto.Title} Is Created Sucessfully";
                    else
                        Message = $"Department {createTicketDto.Title} Is Not Created Sucessfully";
                    TempData["Message"] = Message;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    if (_environment.IsDevelopment())
                        ModelState.AddModelError(string.Empty, ex.Message);
                    else
                        _logger.LogError(ex, "Error creating ticket");  
                    //_logger.LogError(ex, "Error creating ticket");
                    //ModelState.AddModelError("", "An error occurred while creating the ticket. Please try again.");
                }
            }
            return View(createTicketDto);
        }

        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var ticket = await _serviceManger.TicketService.GetTicketById(id.Value);
            if (ticket == null) return NotFound();
            return View(ticket);
        }
        #endregion

    }
}
