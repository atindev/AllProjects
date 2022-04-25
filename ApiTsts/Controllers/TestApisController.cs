using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTsts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestApisController : ControllerBase
    {
        private readonly ApiTstsContext _context;

        public TestApisController(ApiTstsContext context)
        {
            _context = context;
        }

        // GET: api/TestApis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestApi>>> GetTestApi()
        {
            return await _context.TestApi.ToListAsync();
        }

        // GET: api/TestApis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestApi>> GetTestApi(int id)
        {
            var testApi = await _context.TestApi.FindAsync(id);

            if (testApi == null)
            {
                return NotFound();
            }

            return testApi;
        }

        // GET: api/TestApis/atin
        [HttpGet("{findName}")]
        public async Task<ActionResult<TestApi>> GetTestApi(string findName)
        {
            var testApi = await _context.TestApi.FindAsync();

            if (testApi == null)
            {
                return NotFound();
            }

            return testApi;
        }

        // PUT: api/TestApis/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTestApi(int id, TestApi testApi)
        {
            if (id != testApi.id)
            {
                return BadRequest();
            }

            _context.Entry(testApi).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestApiExists(id))
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

        // POST: api/TestApis
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TestApi>> PostTestApi(TestApi testApi)
        {
            _context.TestApi.Add(testApi);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTestApi", new { id = testApi.id }, testApi);
        }

        // DELETE: api/TestApis/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TestApi>> DeleteTestApi(int id)
        {
            var testApi = await _context.TestApi.FindAsync(id);
            if (testApi == null)
            {
                return NotFound();
            }

            _context.TestApi.Remove(testApi);
            await _context.SaveChangesAsync();

            return testApi;
        }

        private bool TestApiExists(int id)
        {
            return _context.TestApi.Any(e => e.id == id);
        }
    }
}
