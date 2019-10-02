using System.Threading.Tasks;
using AutoMapper;
using BookReview.API.Data;
using BookReview.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookReview.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// Repository that contains the implementations of authentication methods
        /// </summary>
        private readonly IBookRepository _repo;
        /// <summary>
        /// AutoMapper that is used for object-object mapping
        /// </summary>
        private readonly IMapper _mapper;



        /// <summary>
        /// Class Constructor
        /// </summary>
        public UsersController(IBookRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }


        [HttpGet("{userId}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var user=await _repo.GetUser(userId);

            var userToReturn = _mapper.Map<UserToReturnDto>(user);

            return Ok(userToReturn);
        }
    }
}