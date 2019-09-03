using System.Threading.Tasks;
using AutoMapper;
using BookReview.API.Data;
using BookReview.API.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BookReview.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController :ControllerBase
    {
        private readonly IBookRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IBookRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);
            var userToReturn = _mapper.Map<UserToReturnDto>(user);

            return Ok(userToReturn);
        }
    }
}