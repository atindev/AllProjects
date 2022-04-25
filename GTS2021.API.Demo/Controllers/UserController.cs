using GTS2021.API.Demo.DbContexts;
using GTS2021.API.Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GTS2021.API.Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly ToDoApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserTasksController"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UserController(ToDoApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("GetUser")]
        public IActionResult GetAllUser()
        {
            return this.Ok(context.User.AsQueryable());
        }

        [HttpGet("GetUser/{Id}")]
        public IActionResult GetUserbyId(int Id)
        {
            var userTask = context.User.Include(x => x.userTasks.Take(100)).Where(x => x.Id == Id).ToList();
            ////var userTask = context.User.Find(Id);
            if (userTask == null)
            {
                return this.NotFound();
            }
            else
            {
                return this.Ok(userTask);
            }
        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUserTask([FromBody] User obj)
        {
            var userTask = context.User.Add(obj);
            context.SaveChanges();
            return this.Created("", userTask.Entity);
        }

        [HttpDelete("DeleteTask/{Id}")]
        public IActionResult DeleteTask(int Id)
        {
            var delUser = context.User.Find(Id);
            context.User.Remove(delUser);
            context.SaveChanges();
            return this.NoContent();
        }
    }
}
