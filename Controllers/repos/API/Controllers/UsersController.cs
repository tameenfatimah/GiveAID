using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private MyDbContext _db;
        public UsersController(MyDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<ActionResult<List<Users>>> GetUsers()
        {
            var data = await _db.Users.ToListAsync();
            return Ok(data);
        }
        [HttpPost]
        public async Task<ActionResult<Users>> AddUsers(Users stud)
        {
            await _db.Users.AddAsync(stud);
            await _db.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Users>> DeleteUsers(int id, Users stud)
        {
            await _db.Users.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            _db.Users.Remove(stud);
            await _db.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Users>> UpdateUsers([FromForm] Users stud, int id)
        {
            if (id != stud.Id)
            {
                return BadRequest();
            }
            _db.Entry(stud).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return Ok(stud);
        }
    }
}
