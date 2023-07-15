using FindMe.DTO;
using FindMe.Services;
using Microsoft.AspNetCore.Mvc;

namespace FindMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //private readonly IMapper mapper;
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<List<UserInfoDTO>>> Get()
        {
            var users = await _userService.GetAllUsers();
            return new ObjectResult(users.Data);
        }

        //// GET api/users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _userService.GetUserById(id);
            if(result.Error != null)
            {
                return BadRequest(result.Error);
            }
           
            return new ObjectResult(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDTO user)
        {
            var result = await _userService.AddNewUser(user);

            if(result.Error != null)
            {
                return BadRequest(result.Error);
            }
            return new ObjectResult(result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserDTO user)
        {
            var result = await _userService.UpdateUser(id, user);
            if (result.Error != null)
            {
                return BadRequest(result.Error);
            }

            return new ObjectResult(result.Data);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Activate(int id)
        {
            var result = await _userService.ActivateUser(id);

            if (result.Error != null)
            {
                return BadRequest(result.Error);
            }

            return new ObjectResult(result.Data);
        }

        //// DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteUser(id);

            if (result.Error != null)
            {
                return BadRequest(result.Error);
            }

            return new ObjectResult(result.Data);
        }
    }
}
