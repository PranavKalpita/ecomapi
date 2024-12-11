using ecomapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecomapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment environment;

        public UserController(AppDbContext context, IWebHostEnvironment environment)
        {
            this.context = context; 
            this.environment = environment;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> newUser([FromForm]User user)
        {
            if (ModelState.IsValid)
            {
                var entity = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if(entity!=null)
                {
                    return BadRequest(new { Message = "Email is already registered" });
                }
                
                context.Users.Add(user);
                await context.SaveChangesAsync();

                return Ok(new { Message = "User Register Successfully" });
            }
            return BadRequest(new { Message = "All the Fields are required" });
        }

        [HttpGet]
        [Route("view")]
        public async Task<IActionResult> ViewUser(string email, string password)
        {
            if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
            {
                var entity = await context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
                if (entity == null)
                    return BadRequest(new { Message = "Invalid Username and Password" });

                return Ok(entity);
            }
            return BadRequest(new { Message = "Email and Password are required." });
        }

        [HttpGet]
        [Route("viewall")]
        public async Task<IActionResult> viewAllUser()
        {
            var entity= await context.Users.ToListAsync();
            if (entity == null)
                return BadRequest(new { Message = "No User Found" });

            return Ok(entity);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> deleteUser(string Email)
        {
            var entity = await context.Users.FirstOrDefaultAsync(u => u.Email == Email);
            if (entity == null)
                return BadRequest(new { Message = "User Not Found" });
            context.Users.Remove(entity);
            await context.SaveChangesAsync();
            return Ok(new { Message = "User Deleted Successfully" });
        }

        [HttpGet]
        [Route("update")]
        public async Task<IActionResult> updateUser(string Email)
        {
            var entity = await context.Users.FirstOrDefaultAsync(u => u.Email == Email);
            if (entity == null)
                return BadRequest(new { Message = "No User Found" });
            return Ok(entity);
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> updateUser(string Email, [FromForm]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Field all Details" });
            }
          
            var entity = await context.Users.FirstOrDefaultAsync(u => u.Email == Email);

            if (entity == null)
            {
                return NotFound(new { Message = "User not found with the provided email." });
            }

            entity.Name = user.Name;
            entity.Email = user.Email;
            entity.Password = user.Password;
            entity.Gender = user.Gender;
            entity.Phone = user.Phone;
            entity.Address = user.Address;
            entity.City = user.City;
            entity.Role = user.Role;

           await context.SaveChangesAsync();
            return Ok(new {Message ="All Details are update successfully"});
        }
    }
}
