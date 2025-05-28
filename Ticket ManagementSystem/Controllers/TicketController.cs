using System.Threading.Tasks;
using BLL.DataTransferObjects.TicketDtos;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
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

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ticket = await _serviceManger.TicketService.GetTicketById(id);
            if (ticket == null) return NotFound();
            var viewModel = new UpdateTicketViewModel
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Type = ticket.Type,
                Status = ticket.Status,
                Description = ticket.Description
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updateTicketDto = new UpdateTicketDto()
                    {
                        Id = model.Id,
                        Title = model.Title,
                        Type = model.Type,
                        Status = model.Status,
                        Description = model.Description
                    };
                    int result = await _serviceManger.TicketService.UpdateTicket(updateTicketDto);
                    if (result > 0)
                        return RedirectToAction(nameof(Index));
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
            return View(model);
        }

        #endregion

        #region Delete
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0) return BadRequest();
            try
            {
                bool result = await _serviceManger.TicketService.DeleteTicket(id);
                if (result)
                    return RedirectToAction(nameof(Index));
                else
                    ModelState.AddModelError(string.Empty, "Ticket was not deleted");
            }
            catch (Exception ex)
            {
                //log Exception
                if (_environment.IsDevelopment())
                {
                    //1- Devolpment=> log error in console and return error message to user
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _logger.LogError(ex.Message);
                    return View("ErrorView", ex);
                }
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}
