using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DataTransferObjects.CommentDtos;
using BLL.Exceptions;
using BLL.Services.Interfaces;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Classes
{
    class CommentService(IUnitOfWork _unitOfWork,
                                IMapper _mapper) : ICommentService
    {
        public async Task<IEnumerable<CommentDto>> GetAllCommentsAsync()
        {
            var comments = await _unitOfWork.GetRepository<Comment, int>().GetAllActive().ToListAsync();
            var commentDto = _mapper.Map<IEnumerable<CommentDto>>(comments);
            return commentDto;
        }

        public async Task<CommentDetailsDto> GetCommentByIdAsync(int id)
        {
            var comment = await _unitOfWork.GetRepository<Comment, int>().GetByIdAsync(id);
            if (comment == null)
            {
                throw new NotFoundException($"Comment with id {id} not found");
            }
            var commentDto = _mapper.Map<CommentDetailsDto>(comment);
            return commentDto;
        }

        public async Task<int> CreateCommentAsync(CreateCommentDto commentDto)
        {
            //var existingComment = await _unitOfWork.GetRepository<Comment, int>().GetAllActive().FirstOrDefaultAsync(c => c.Content == commentDto.Content && c.TicketId == commentDto.TicketId);
            var ticket = await _unitOfWork.GetRepository<Ticket, int>().GetByIdAsync(commentDto.TicketId)
                        ??throw new NotFoundException($"Ticket with id {commentDto.TicketId} not found");

            var comment = _mapper.Map<Comment>(commentDto);
            await _unitOfWork.GetRepository<Comment, int>().AddAsync(comment);
            return await _unitOfWork.SaveChanges();
        }


        public async Task<int> UpdateCommentAsync(UpdateCommentDto commentDto)
        {
            var comment = _mapper.Map<Comment>(commentDto);
            var existingComment = await _unitOfWork.GetRepository<Comment, int>().GetByIdAsync(commentDto.Id);
            if (existingComment == null)
                throw new NotFoundException($"Comment with id {commentDto.Id} not found");

            _mapper.Map(commentDto, existingComment); 
            _unitOfWork.GetRepository<Comment, int>().Update(existingComment);
            return await _unitOfWork.SaveChanges();
        }

        public async Task<bool> DeleteCommentAsync(int id)
        {
            var comment = await _unitOfWork.GetRepository<Comment, int>().GetAllActive().FirstOrDefaultAsync(c=> c.Id == id);
            if (comment == null)
            {
                throw new NotFoundException($"Comment with id {id} not found");
            }
            comment.IsDeleted = true;
            _unitOfWork.GetRepository<Comment, int>().Update(comment);
            return await _unitOfWork.SaveChanges() >0 ? true : false;
        }

    }
}
