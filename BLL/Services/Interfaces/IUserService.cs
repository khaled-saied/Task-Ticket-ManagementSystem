using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DataTransferObjects.UserDtos;

namespace BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDetailsDto?> GetUserDetailsAsync(string userId);

        Task<List<UserDto>> GetAllUsersAsync(string? searchVal = null);

        Task<int> UpdateUserAsync(UpdateUserDto updateUserDto);
    }
}
