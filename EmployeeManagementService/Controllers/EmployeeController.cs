using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace EmployeeManagementService.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _context;

        public EmployeeController(EmployeeContext context)
        {
            _context = context;
        }

        // GET: Employee
        [HttpGet]
        public ActionResult<IEnumerable<Employee>> Employee()
        {
            return _context.Employee;
        }

        // GET: Employee/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }



        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee([FromRoute] int id, [FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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

        // POST: api/Employee
        [HttpPost]
        public async Task<IActionResult> HireEmployee([FromBody] JObject employeeJson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var employee = new Employee
            {
                EmployeeName = (string)employeeJson["employeeName"],
                DateBirth = (DateTime)employeeJson["dateBirth"],
                Post = (string)employeeJson["post"],
                Cash = 0,
                Salary = (int)employeeJson["salary"]
            };
            _context.Employee.Add(employee);
            await _context.SaveChangesAsync();
            return Ok(employee.EmployeeId);
        }


        // POST: /ChangePost
        /// <summary>
        /// Назначить должность рабочему
        /// </summary>
        /// <param name="employeeId">id</param>
        /// <param name="post">Должность</param>
        /// <returns></returns>
        [HttpPost("ChangePost")]
        public async Task<IActionResult> ChangePost([FromBody]JObject result)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var employee = await _context.Employee.FindAsync(((int)result["employeeId"]));
            if (employee == null)
            {
                NotFound();
            }
            employee.Post = (string)result["post"];
            _context.Update(employee);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // POST: /CalculateSalary
        /// <summary>
        /// Определить зарплату
        /// </summary>
        /// <param name="employeeId">id</param>
        /// <param name="salary">ZP</param>
        /// <returns></returns>
        [HttpPost("CalculateSalary")]
        public async Task<IActionResult> CalculateSalary([FromBody]JObject result)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var employee = await _context.Employee.FindAsync((int)result["employeeId"]);
            if (employee == null)
            {
                return NotFound();
            }
            employee.Salary = (int)result["salary"];
            _context.Update(employee);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // POST: /GiveMoneyToEmployee
        /// <summary>
        /// Пополнить счет рабочему
        /// </summary>
        /// <param name="employeeId">id</param>
        /// <param name="money">Денежные средства</param>
        /// <returns></returns>
        [HttpPost("GiveMoneyToEmployee")]
        public async Task<IActionResult> GiveMoneyToEmployee([FromBody] JObject result)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var employee = await _context.Employee.FindAsync((int)result["employeeId"]);
            if (employee == null)
            {
                return NotFound();
            }
            employee.Cash += (int)result["cash"];
            _context.Update(employee);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DismissEmployee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok(employee);
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.EmployeeId == id);
        }
    }
}