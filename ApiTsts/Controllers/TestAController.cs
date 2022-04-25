using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiTsts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAController : ControllerBase
    {
        private readonly ApiTstsContext _context;

        public TestAController(ApiTstsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<TestApi2>>> Get()
        {
            return await _context.TestApi2.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<TestApi2>> Post(TestApi2 testApi2)
        {
            TestApi2 result = (await _context.TestApi2.AddAsync(testApi2)).Entity;
            _context.SaveChanges();

            return result;
            //return _context.TestApi2.Find(x => x.TestApiId == testApi2.TestApiId);
        }
    }
}