using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DataTransferObjects.UserDtos;
using BLL.Exceptions;
using BLL.Services.AttachmentServices.AttachmentServices;
using BLL.Services.Interfaces;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Classes
{
    public class UserService(UserManager<ApplicationUser> _userManager,
                             IAttachmentServices _attachmentServices) : IUserService
    {
        public async Task<List<UserDto>> GetAllUsersAsync(string? searchVal = null)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchVal))
            {
                query = query.Where(u => u.UserName.ToLower().Contains(searchVal.ToLower()) ||
                                         u.FullName.ToLower().Contains(searchVal.ToLower()));
            }

            var users = await query.ToListAsync();

            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Roles = roles.ToList()
                });
            }

            return userDtos;
        }


        public async Task<UserDetailsDto?> GetUserDetailsAsync(string userId)
        {
            var user = await _userManager.Users
                .Include(u => u.Projects.Where(p=> !p.IsDeleted))
                .Include(u => u.Comments.Where(c=> !c.IsDeleted))
                .FirstOrDefaultAsync(u => u.Id == userId)
                ?? throw new NotFoundException( $"User With {userId}ID Not Found.");
            var roles = await _userManager.GetRolesAsync(user);

            return new UserDetailsDto()
            {
                Id = user.Id,
                ImageName = user.ImageName,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList(),
                Comments = user.Comments?.Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                }).ToList(),
                Projects = user.Projects?.Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description
                }).ToList()
            };
        }

        public async Task<int> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == updateUserDto.Id)
                ?? throw new NotFoundException($"User with ID {updateUserDto.Id} not found.");

            if (updateUserDto.ImageName is not null && updateUserDto.Image is not null)
            {
                _attachmentServices.Delete(updateUserDto.ImageName);
            }

            if (updateUserDto.Image is not null)
            {
                var imageName = _attachmentServices.Upload(updateUserDto.Image,"Images");
                user.ImageName = imageName;
            }   

            user.ConcurrencyStamp = updateUserDto.ConcurrencyStamp;
            user.UserName = updateUserDto.UserName;
            user.FullName = updateUserDto.FullName;
            user.Email = updateUserDto.Email;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return 1;
            }
            else 
            { 
                if (result.Errors.Any(e => e.Code == "ConcurrencyFailure"))
                {
                    throw new Exception("Failed to update user: Optimistic concurrency failure, object has been modified.");
                }

                throw new Exception("Failed to update user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

    }
}
