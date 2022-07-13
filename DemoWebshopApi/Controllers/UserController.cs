using AutoMapper;
using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.DTOs.RequestModels;
using DemoWebshopApi.DTOs.ResponseModels;
using DemoWebshopApi.Services.Services;
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
        public async Task<ActionResult<UserResponseDto>> GetUser(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return _mapper.Map<UserResponseDto>(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserRequestDto user)
        {
            var updatedUser = _mapper.Map<User>(user);
            updatedUser.Id = id;

            var isSuccessful = await _userService.UpdateUserAsync(_mapper.Map<User>(updatedUser));
            if (!isSuccessful)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id}/UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(Guid id, UpdatePasswordDto updatePasswordDto)
        {
            var isSuccessful = await _userService.UpdateUserPasswrodAsync(id, updatePasswordDto);
            if (!isSuccessful)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> CreateUser(CreateUserRequestDto user)
        {
            var newUser = await _userService.CreateUserAsync(_mapper.Map<User>(user), user.Password);

            return CreatedAtAction("GetUser", new { id = newUser.Id }, _mapper.Map<UserResponseDto>(newUser));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var isSuccessful = await _userService.DeleteUserAsync(id);
            if (!isSuccessful)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
