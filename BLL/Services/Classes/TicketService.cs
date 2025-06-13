using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Exceptions;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Classes
{
    public class TicketService(IUnitOfWork _unitOfWork,
                               IMapper _mapper) : ITicketService
    {
        //get all tickets
        public async Task<IEnumerable<TicketDto>> GetAllAsync()
        {
            var tickets= await _unitOfWork.GetRepository<Ticket,int>().GetAllActive().ToListAsync();
            var ticketDtos = _mapper.Map<IEnumerable<TicketDto>>(tickets);
            return ticketDtos;
        }

        //get ticket by id
        public async Task<TicketDetailsDto> GetTicketById(int id)
        {
            var ticket = await _unitOfWork.GetRepository<Ticket, int>().GetAllActive()
                                           .Include(t=> t.Task)
                                           .Include(t => t.Comments.Where(c => !c.IsDeleted))
                                           .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
            if (ticket == null)
            {
                throw new NotFoundException($"Ticket with id {id} not found");
            }
            var ticketDto = _mapper.Map<TicketDetailsDto>(ticket);
            return ticketDto;
        }

        //create ticket
        public async Task<int> CreateTicket(CreateTicketDto ticketDto)
        {
            var task = await _unitOfWork.GetRepository<TaskK, int>().GetByIdAsync(ticketDto.TaskId)
                           ?? throw new NotFoundException($"Task with id {ticketDto.TaskId} not found");
            var ticket = _mapper.Map<Ticket>(ticketDto);

            var IfExists = await _unitOfWork.GetRepository<Ticket, int>().GetAllActive()
                .AnyAsync(t => t.Title == ticket.Title && t.TaskId == ticket.TaskId);
            if (IfExists)
                throw new ConflictException($"Ticket with title {ticket.Title} already exists in task {ticket.TaskId}");

            await _unitOfWork.GetRepository<Ticket, int>().AddAsync(ticket);
            return await _unitOfWork.SaveChanges();
        }


        //update ticket
        public async Task<int> UpdateTicket(UpdateTicketDto updateTicketDto)
        {
            var ticket = await _unitOfWork.GetRepository<Ticket, int>().GetByIdAsync(updateTicketDto.Id);

            if (ticket == null)
                throw new NotFoundException($"Ticket with id {updateTicketDto.Id} not found");

            var IfExists = await _unitOfWork.GetRepository<Ticket, int>().GetAllActive()
                .AnyAsync(t => t.Title == updateTicketDto.Title && t.Id != updateTicketDto.Id);
            if (IfExists)
                throw new ConflictException($"Ticket with title {updateTicketDto.Title} already exists");
            _mapper.Map(updateTicketDto,ticket);
            _unitOfWork.GetRepository<Ticket, int>().Update(ticket);
            return await _unitOfWork.SaveChanges();
        }


        //delete ticket
        public async Task<bool> DeleteTicket(int id)
        {
            var ticket = await _unitOfWork.GetRepository<Ticket, int>().GetByIdAsync(id);
            if (ticket == null)
                throw new NotFoundException($"Ticket with id {id} not found");

            var comments = await _unitOfWork.GetRepository<Comment, int>()
                                            .GetAllActive()
                                            .Where(c => c.TicketId == id)
                                            .ToListAsync();

            foreach (var comment in comments)
            {
                comment.IsDeleted = true;
                _unitOfWork.GetRepository<Comment, int>().Update(comment);
            }

            ticket.IsDeleted = true;
            _unitOfWork.GetRepository<Ticket, int>().Update(ticket);
            return await _unitOfWork.SaveChanges() > 0 ? true : false;
        }

        public Count GetCount()
        {
            return _unitOfWork.GetRepository<Ticket, int>().GetCount();
        }

    }
}
