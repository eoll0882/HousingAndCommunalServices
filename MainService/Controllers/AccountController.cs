
using MainService.Extentions;
using MainService.Models;
using MainService.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MainService.Controllers
{
    //[Route("/[controller]")]
    //[ApiController]
    public class AccountController : Controller
    {
        private UserContext db;
        public AccountController(UserContext context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await db.User.FirstOrDefaultAsync(u => u.UserName == model.UserName && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(model.UserName); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await db.User.FirstOrDefaultAsync(u => u.UserName == model.UserName);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    
                    var newEmployeeId = await RequestExecutor.ExecuteRequest(Scope.EmployeeServiceUrl, 
                        new RestRequest("/Employee", Method.POST)
                            .AddHeader("Content-type", "application/json")    
                            .AddJsonBody(new
                            {
                                employeeName = "unknown",
                                dateBirth = DateTime.MinValue,
                                post = "employee",
                                cash = 0,
                                salary = 0,
                            }));
                    db.User.Add(new User { UserName = model.UserName, Password = model.Password, Role = 1, IsBlocked = false, EmployeeId = int.Parse(newEmployeeId) });
                    await db.SaveChangesAsync();
                    await Authenticate(model.UserName); // аутентификация
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpPost("BlockUser")]
        public async Task<IActionResult> BlockUser([FromBody] JObject userJson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await db.User.FindAsync(((int)userJson["userId"]));
            if (user == null)
            {
                return NotFound();
            }
            user.IsBlocked = true;
            db.Update(user);
            await db.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("UnblockUser")]
        public async Task<IActionResult> UnblockUser([FromBody] JObject userJson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await db.User.FindAsync(((int)userJson["userId"]));
            if (user == null)
            {
                return NotFound();
            }
            user.IsBlocked = false;
            db.Update(user);
            await db.SaveChangesAsync();
            return Ok();
        }

        // POST: api/Tasks
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] JObject userJson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.User.Add(new User
            {
                UserName = (string)userJson["userName"],
                Password = (string)userJson["password"],
                IsBlocked = false,
                Role = (int)userJson["roleId"],
                EmployeeId = (int)userJson["employeeId"]
            });
            await db.SaveChangesAsync();
            return Ok();
        }


        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await db.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            db.User.Remove(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }
    }
}
