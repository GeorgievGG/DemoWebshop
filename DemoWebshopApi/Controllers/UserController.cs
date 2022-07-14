using AutoMapper;
using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.DTOs.RequestModels;
using DemoWebshopApi.DTOs.ResponseModels;
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
            if (user == null)
            {
                return NotFound();
            }

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

            var isSuccessful = await _userService.UpdateUser(_mapper.Map<User>(updatedUser));
            if (!isSuccessful)
            {
                return NotFound();
            }

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

            var isSuccessful = await _userService.UpdateUserPasswrod(Guid.Parse(UserId), updatePasswordDto);
            if (!isSuccessful)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id}/SetUserAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetUserRole(Guid id)
        {
            var isSuccessful = await _userService.SetUserInRole(id, "Admin");
            if (!isSuccessful)
            {
                return NotFound();
            }

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
            var isSuccessful = await _userService.DeleteUser(id);
            if (!isSuccessful)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
