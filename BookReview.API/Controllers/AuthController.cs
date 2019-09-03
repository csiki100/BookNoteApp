using System.Threading.Tasks;
using AutoMapper;
using BookReview.API.Data;
using BookReview.API.Dtos;
using BookReview.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookReview.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists");

             var userToCreate = _mapper.Map<User>(userForRegisterDto);

             var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

             var userToReturn = _mapper.Map<UserToReturnDto>(createdUser);

            return CreatedAtRoute("GetUser", new
            {
                controller = "Users",
                id = createdUser.Id
            }, userToReturn);
        }

        [HttpPost("login")]
        public async Task<IActionResult> login()
        {
            return Ok();
        }
    }
}