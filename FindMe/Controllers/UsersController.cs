using AutoMapper;
using FindMe.DTO;
using FindMe.Error;
using FindMe.Models;
using FindMe.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace FindMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        ApplicationContext db;
        private readonly IMapper mapper;

        public UsersController(ApplicationContext context, IMapper mapper)
        {
            db = context;
            this.mapper = mapper;

            if (!db.Users.Any())
            {
                db.Users.Add(new User { Email = "Tom33@gmail.com", Salt = RandomNumberGenerator.GetBytes(128 / 8), Hash = "dertyujkii", PhoneNumber = "885521477"});

                db.SaveChanges();
            }
        }

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfoDTO>>> Get()
        {
            return await db.Users
                           .Where(p => p.Active)
                           .Select(p => mapper.Map<UserInfoDTO>(p))
                           .ToListAsync();
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound("The user is not found.");
            }

            return new ObjectResult(mapper.Map<UserInfoDTO>(user));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDTO user)
        {
            if(user.Email == null && user.PhoneNumber == null)
            {
                return BadRequest(Result<UserDTO>.GetError(ErrorCode.NotFound, "Enter email or phone number."));
            }

            var hashedPassword = PasswordUtils.GetHashedPassword(user.Password);

            var newUser = mapper.Map<User>(user);
            newUser.Salt = hashedPassword.salt;
            newUser.Hash = hashedPassword.hashed;

            try
            {
                db.Users.Add(newUser);
                await db.SaveChangesAsync();
            }
            catch(DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return new ObjectResult(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserDTO user)
        {
            if (user.Email == null && user.PhoneNumber == null)
            {
                return BadRequest("Enter email or phone number.");
            }

            var checkedUser = await db.Users
                                      .Where(x => x.Id == id && x.Active)
                                      .Select(x => new { x.CreatedDate, x.Id })
                                      .FirstOrDefaultAsync();

            if (checkedUser == null)
            {
                return NotFound("The user is not found.");
            }

            var userToUpdate = mapper.Map<User>(user);
            userToUpdate.Id = id;
            userToUpdate.CreatedDate = checkedUser.CreatedDate;

            try
            {
                db.Users.Update(userToUpdate);
                await db.SaveChangesAsync();

                return Ok(userToUpdate);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Activate(int id)
        {
            var checkedUser = await db.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (checkedUser == null)
            {
                return NotFound("The user is not found.");
            }

            checkedUser.Active = true;
            await db.SaveChangesAsync();

            return Ok(mapper.Map<UserInfoDTO>(checkedUser));
        }

        //// DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userToDelete = await db.Users.FirstOrDefaultAsync(x => x.Id == id);

            if(userToDelete == null)
            {
                return NotFound("The user is not found.");
            }

            userToDelete.Active = false;
            await db.SaveChangesAsync();

            return Ok(mapper.Map<UserInfoDTO>(userToDelete));
        }
    }
}
