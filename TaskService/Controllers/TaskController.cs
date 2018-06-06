using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TaskService.Models;

namespace TaskService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskContext _context;

        public TaskController(TaskContext context)
        {
            _context = context;
        }

        // GET: api/Tasks
        [HttpGet]
        public ActionResult<IEnumerable<Models.Task>> Tasks()
        {
            return _context.Task;
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }



        // PUT: api/Tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask([FromRoute] int id, [FromBody] Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != task.TaskId)
            {
                return BadRequest();
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
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

        // POST: api/Tasks
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] JObject task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Task.Add(new Models.Task
            {
                TaskShortName = (string)task["taskShortName"],
                TaskDescription = (string)task["taskDescription"],
                DeadLine = (DateTime)task["deadLine"],
                IsCompleted = false,
                Executor = (int)task["executor"]
            });
            await _context.SaveChangesAsync();
            return Ok();
        }


        // POST: /EditTask

        [HttpPost("EditTask")]
        public async Task<ActionResult> EditTask([FromBody]JObject taskJson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var task = await _context.Task.FindAsync(((int)taskJson["taskId"]));
            if (task == null)
            {
                return NotFound();
            }
            task.TaskShortName = (string)taskJson["shortName"];
            task.TaskDescription = (string)taskJson["description"];
            task.DeadLine = (DateTime)taskJson["deadLine"];
            task.Executor = (int)taskJson["executor"];
            _context.Update(task);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("AppointExecutor")]
        public async Task<ActionResult> AppointExecutor([FromBody]JObject taskJson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var task = await _context.Task.FindAsync(((int)taskJson["taskId"]));
            if (task == null)
            {
                return NotFound();
            }
            task.Executor = (int)taskJson["executor"];
            _context.Update(task);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("FinishTask")]
        public async Task<ActionResult> FinishTask([FromBody]JObject taskJson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var task = await _context.Task.FindAsync(((int)taskJson["taskId"]));
            if (task == null)
            {
                return NotFound();
            }
            task.IsCompleted = (bool)taskJson["isComleted"];
            _context.Update(task);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpGet("SearchTasks")]
        public ActionResult<IEnumerable<Models.Task>> SearchTasks([FromBody]JObject result)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var tasks = _context.Task.Where((x => x.TaskShortName == (string)result["shortName"]));
            if (tasks == null)
            {
                return NotFound();
            }
            return tasks.ToList();
        }


        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            _context.Task.Remove(task);
            await _context.SaveChangesAsync();
            return Ok(task);
        }

        private bool TaskExists(int id)
        {
            return _context.Task.Any(e => e.TaskId == id);
        }
    }
}
