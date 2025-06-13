using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DataTransferObjects.CommentDtos;
using BLL.Exceptions;
using BLL.Services.Interfaces;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Classes
{
    public class CommentService(IUnitOfWork _unitOfWork,
                                IMapper _mapper,
                                UserManager<ApplicationUser> _userManager) : ICommentService
    {
        public async Task<IEnumerable<CommentDto>> GetAllCommentsAsync()
        {
            var comments = await _unitOfWork.GetRepository<Comment, int>().GetAllActive().ToListAsync();
            var commentDto = _mapper.Map<IEnumerable<CommentDto>>(comments);
            return commentDto;
        }

        public async Task<CommentDetailsDto> GetCommentByIdAsync(int id)
        {
            var comment = await _unitOfWork.GetRepository<Comment, int>()
                                           .GetAllActive()
                                           .Include(c => c.Ticket)
                                           .Include(c => c.User)
                                           .FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null)
            {
                throw new NotFoundException($"Comment with id {id} not found");
            }
            var commentDto = _mapper.Map<CommentDetailsDto>(comment);
            return commentDto;
        }

        public async Task<int> CreateCommentAsync(CreateCommentDto commentDto, int TicketId, string UserId)
        {
            var ticket = await _unitOfWork.GetRepository<Ticket, int>().GetByIdAsync(TicketId)
                        ?? throw new NotFoundException($"Ticket with id {TicketId} not found");
            var user = await _userManager.FindByIdAsync(UserId)
                        ?? throw new NotFoundException($"User with id {UserId} not found");


            var comment = _mapper.Map<Comment>(commentDto);
            comment.TicketId = TicketId;
            comment.UserId = UserId;
            await _unitOfWork.GetRepository<Comment, int>().AddAsync(comment);
            return await _unitOfWork.SaveChanges();
        }


        public async Task<int> UpdateCommentAsync(UpdateCommentDto commentDto)
        {
            //Validate if User Is who is trying to update the comment is the owner of the comment
            var commentRepo = _unitOfWork.GetRepository<Comment, int>();

            var existingComment = await commentRepo.GetByIdAsync(commentDto.Id)
                                    ?? throw new NotFoundException($"Comment with id {commentDto.Id} not found");

            _mapper.Map(commentDto, existingComment);

            commentRepo.Update(existingComment);
            return await _unitOfWork.SaveChanges();
        }

        public async Task<bool> DeleteCommentAsync(int id)
        {
            var comment = await _unitOfWork.GetRepository<Comment, int>().GetAllActive().FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null)
            {
                throw new NotFoundException($"Comment with id {id} not found");
            }
            comment.IsDeleted = true;
            _unitOfWork.GetRepository<Comment, int>().Update(comment);
            return await _unitOfWork.SaveChanges() > 0 ? true : false;
        }

        public Count GetCount()
        {
            return _unitOfWork.GetRepository<Comment, int>().GetCount();
        }

        // Show deleted comments
        public IQueryable<CommentDto> GetAllDeletedComments()
        {
            return _unitOfWork.GetRepository<Comment, int>().GetAllDeleted().Select(C=> _mapper.Map<CommentDto>(C));
        }
    }
}
