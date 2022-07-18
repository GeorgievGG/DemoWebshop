using AutoMapper;
using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.DTOs.RequestModels;
using DemoWebshopApi.DTOs.ResponseModels;
using DemoWebshopApi.Services.Interfaces;
using DemoWebshopApi.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebshopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserResponseDto>> GetUser(Guid id)
        {
            var user = await _userService.GetUserById(id);

            return _mapper.Map<UserResponseDto>(user);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUser(UpdateUserRequestDto user)
        {
            if (UserId == null)
            {
                return BadRequest();
            }

            var updatedUser = _mapper.Map<User>(user);
            updatedUser.Id = Guid.Parse(UserId);

            await _userService.UpdateUser(_mapper.Map<User>(updatedUser));

            return NoContent();
        }

        [HttpPut("UpdatePassword")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordDto updatePasswordDto)
        {
            if (UserId == null)
            {
                return BadRequest();
            }

            await _userService.UpdateUserPasswrod(Guid.Parse(UserId), updatePasswordDto);

            return NoContent();
        }

        [HttpPut("{id}/SetUserAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetUserRole(Guid id)
        {
            await _userService.SetUserInRole(id, "Admin");

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> CreateUser(CreateUserRequestDto user)
        {
            var newUser = await _userService.CreateUser(_mapper.Map<User>(user), user.Password);

            return CreatedAtAction("GetUser", new { id = newUser.Id }, _mapper.Map<UserResponseDto>(newUser));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userService.DeleteUser(id);

            return NoContent();
        }
    }
}
