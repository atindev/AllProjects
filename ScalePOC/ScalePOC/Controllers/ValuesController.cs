using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace ScalePOC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private Scale objScale = null;
        public ValuesController()
        {
            if (objScale == null)
            {
                objScale = new Scale("127.0.0.1", 1337);
                //objScale = new Scale("127.0.0.1", 9229);
            }
        }

        #region Commented        
        // GET api/values
        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5
        //[HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        //{
        //    return "value";
        //}

        // POST api/values
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
        #endregion

        [HttpGet]
        [Route("GetScale")]
        public async Task<ActionResult<string>> GetScale()
        {
            string ScaleWeight = await objScale.GetWeight();
            return this.Ok(ScaleWeight);
        }
    }
}
