using BackEndCyrk.DTOs;
using BackEndCyrk.Models;
using BackEndCyrk.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEndCyrk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAll()
        {
            var users = await _userRepository.GetAllAsync();
            var userDtos = users.Select(u => new UserReadDto
            {
                id = u.id,
                name = u.name,
                surname = u.surname,
                email = u.email,
                address = u.address,
                phone = u.phone
            });
            return Ok(userDtos);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDto>> GetById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return NotFound();

            var userDto = new UserReadDto
            {
                id = user.id,
                name = user.name,
                surname = user.surname,
                email = user.email,
                address = user.address,
                phone = user.phone
            };
            return Ok(userDto);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<UserReadDto>> Create(UserRegisterDto createDto)
        {
            var user = new User
            {
                name = createDto.name,
                surname = createDto.surname,
                email = createDto.email,
                password_hash = BCrypt.Net.BCrypt.HashPassword(createDto.password),
                address = createDto.address,
                phone = createDto.phone
            };

            var createdUser = await _userRepository.AddAsync(user);
            var userDto = new UserReadDto
            {
                id = createdUser.id,
                name = createdUser.name,
                surname = createdUser.surname,
                email = createdUser.email,
                address = createdUser.address,
                phone = createdUser.phone
            };

            return CreatedAtAction(nameof(GetById), new { id = userDto.id }, userDto);
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserReadDto>> Update(int id, UserUpdateDto updateDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null) return NotFound();

            if (updateDto.name != null) existingUser.name = updateDto.name;
            if (updateDto.surname != null) existingUser.surname = updateDto.surname;
            if (updateDto.email != null) existingUser.email = updateDto.email;
            if (updateDto.address != null) existingUser.address = updateDto.address;
            if (updateDto.phone != null) existingUser.phone = updateDto.phone;

            await _userRepository.UpdateAsync(existingUser);

            var userDto = new UserReadDto
            {
                id = existingUser.id,
                name = existingUser.name,
                surname = existingUser.surname,
                email = existingUser.email,
                address = existingUser.address,
                phone = existingUser.phone
            };

            return Ok(userDto);
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _userRepository.DeleteByIdAsync(id);
            return NoContent();
        }
    }
}