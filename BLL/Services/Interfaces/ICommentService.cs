using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetAllCommentsAsync();
        Task<CommentDetailsDto> GetCommentByIdAsync(int id);
        Task<int> CreateCommentAsync(CreateCommentDto commentDto);
        Task<int> UpdateCommentAsync(UpdateCommentDto commentDto);
        Task<bool> DeleteCommentAsync(int id);
    }
}
