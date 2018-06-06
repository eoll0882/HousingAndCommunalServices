using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RoleService.Models;

namespace RoleService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        // GET: api/Role

        private readonly RoleContext _context;

        public RoleController(RoleContext context)
        {
            _context = context;
        }

        // GET: api/Roles
        [HttpGet]
        public ActionResult<IEnumerable<Role>> Roles()
        {
            return _context.Role;
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = await _context.Role.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }
        
        // PUT: api/Role/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole([FromRoute] int id, [FromBody] Role role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                        if (id != role.RoleId)
            {
                return BadRequest();
            }
            _context.Entry(role).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] JObject role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Role.Add(new Role
            {
                RoleName = (string)role["roleName"]
            });
            await _context.SaveChangesAsync();
            return Ok();
        }


        // POST: /EditRoleName

        [HttpPost("EditRoleName")]
        public async Task<ActionResult> EditRoleName([FromBody]JObject result)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var role = await _context.Role.FindAsync(((int)result["roleId"]));
            if (role == null)
            {
                return NotFound();
            }
            role.RoleName = (string)result["roleName"];
            _context.Update(role);
            await _context.SaveChangesAsync();
            return Ok();
        }


        // DELETE: api/Role/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var role = await _context.Role.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            _context.Role.Remove(role);
            await _context.SaveChangesAsync();
            return Ok(role);
        }

        private bool RoleExists(int id)
        {
            return _context.Role.Any(e => e.RoleId == id);
        }

    }
}
