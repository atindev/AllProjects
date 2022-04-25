using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace OdataLangTry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetedataController : ControllerBase
    {
        [HttpGet]
        public XmlDocument Metdate()
        {
            return null;
        }
    }
}