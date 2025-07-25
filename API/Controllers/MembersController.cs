using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class MembersController(AppDbContext context) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<AppUser>>> GetMembers()
        {
            var members = await context.Users.ToListAsync();
            return members;
        }
 

        [Authorize]
        [HttpGet("{id}")]

        public ActionResult<AppUser> Getmember(String id)
        {
            var members = context.Users.Find(id);
            if (members == null) return NotFound();

            return members;
        }
    }
}
