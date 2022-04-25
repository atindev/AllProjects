using GTS2021.API.Demo.DbContexts;
using GTS2021.API.Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GTS2021.API.Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTasksController : ControllerBase
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly ToDoApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserTasksController"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UserTasksController(ToDoApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("GetTask")]
        public IActionResult GetAllUserTasks()
        {
            return this.Ok(context.UserTask.AsQueryable());
        }

        [HttpGet("GetTask/{Id}")]
        public IActionResult GetUserTasksbyId(int Id)
        {
            var userTask = context.UserTask.Where(x => x.Id == Id).Include(x => x.CreatedBy).ToList();
            ////var userTask = context.UserTask.Find(Id);
            if (userTask == null)
            {
                return this.NotFound();
            }
            else
            {
                return this.Ok(userTask);
            }
        }

        [HttpPost("CreateTask")]
        public IActionResult CreateUserTask([FromBody] UserTask obj)
        {
            var userTask = context.UserTask.Add(obj);
            context.SaveChanges();
            return this.Created("", userTask.Entity);
        }

        [HttpDelete("DeleteTask/{Id}")]
        public IActionResult DeleteTask(int Id)
        {
            var delTask = context.UserTask.Find(Id);
            context.UserTask.Remove(delTask);
            context.SaveChanges();
            return this.NoContent();
        }
    }
}
