using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookReview.API.Data;
using BookReview.API.Dtos;
using BookReview.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BookReview.API.Controllers
{
    /// <summary>
    /// Controller that contains the end-points of user authentication
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Repository that contains the implementations of authentication methods
        /// </summary>
        private readonly IAuthRepository _repo;
        /// <summary>
        /// AutoMapper that is used for object-object mapping
        /// </summary>
        private readonly IMapper _mapper;
        /// <summary>
        /// App configuration
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        /// Class Constructor
        /// </summary>
        public AuthController(IAuthRepository repo, IMapper mapper, IConfiguration config)
        {
            _config = config;
            _mapper = mapper;
            _repo = repo;
        }


        /// <summary>
        /// Endpoint that registers a new user accoring to the data in the userForRegisterDto
        ///</summary> 
        /// <param name="userForRegisterDto">
        ///Contains the new user's data (username, password)
        ///</param>
        ///<returns>
        ///Returns a "201" Http Response if everything went well,
        /// or a "400" if the username was already taken
        ///</returns>  
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            //Converting username to lowercase
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            //Checking if the username exists
            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists");

            var userToCreate = _mapper.Map<User>(userForRegisterDto);

            //Registering the user
            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            var userToReturn = _mapper.Map<UserToReturnDto>(createdUser);

            //Response
            return CreatedAtRoute("GetUser", new
            {
                controller = "Users",
                userId = createdUser.Id
            }, userToReturn);
        }


        /// <summary>
        /// Endpoint, that logs in the user according to the data in userForLoginDto
        ///</summary> 
        /// <param name="userForLoginDto">
        ///Contains the user's data (username, password)
        ///</param>
        ///<returns>
        ///Returns a "200" Http Response if everything went well,
        /// or a "401" if the username or password was incorrect
        ///</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            //Checking if User exists
            var userFromRepo = await _repo.Login(userForLoginDto.Username, userForLoginDto.Password);

            if (userFromRepo == null)
            {
                return Unauthorized("Username or password is not correct.");
            }

            //Creating the token for the User
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            //Response
            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}