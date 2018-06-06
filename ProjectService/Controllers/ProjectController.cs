using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ProjectService.Models;

namespace RoleService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        // GET: api/Project

        private readonly ProjectContext _context;

        public ProjectController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/Project
        [HttpGet]
        public ActionResult<IEnumerable<Project>> Project()
        {
            return _context.Project;
        }

        // GET: api/Project/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = await _context.Project.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        // PUT: api/Project/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject([FromRoute] int id, [FromBody] Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != project.ProjectId)
            {
                return BadRequest();
            }
            _context.Entry(project).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: /Project
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] JObject project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Project.Add(new Project
            {
                Location = (string)project["location"],
                ProjectName = (string)project["projectName"]
            });
            await _context.SaveChangesAsync();
            return Ok();
        }


        // POST: /EditProject
        [HttpPost("EditProject")]
        public async Task<ActionResult> EditProject([FromBody]JObject result)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var project = await _context.Project.FindAsync(((int)result["projectId"]));
            if (project == null)
            {
                return NotFound();
            }
            project.ProjectName= (string)result["projectName"];
            project.Location = (string)result["location"];
            _context.Update(project);
            await _context.SaveChangesAsync();
            return Ok();
        }


        // DELETE: Project/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            return Ok(project);
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ProjectId == id);
        }

    }
}
