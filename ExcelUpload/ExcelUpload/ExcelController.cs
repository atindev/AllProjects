using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using test.Repository;

namespace test.Controllers
{
    [Route("api/excel")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private IExcelRepository excelRepository;

        public ExcelController(IExcelRepository excel)
        {
            excelRepository = excel;


        }

        [HttpPost("UploadExcelData")]
        public async Task<IActionResult> UploadData([FromBody] objss data)
        {
            try
            {
                var dataUploaded = await excelRepository.UploadDatatoDB(data.data);
                if (dataUploaded)
                {
                    return this.Ok("Upload Successful");
                }
                else
                {
                    return this.Problem("Issue occured");
                }
            }
            catch
            {
                return this.BadRequest("Error");
            }
        }

        [HttpPost("Testt")]
        public IActionResult Testt([FromBody] objss data)
        {
            return this.Ok("Hello " + data.data);
        }
    }

    public class objss
    {
        public string data { get; set; }
    }
}