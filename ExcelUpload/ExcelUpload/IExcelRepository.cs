using System.Threading.Tasks;

namespace test.Repository
{
    public interface IExcelRepository
    {
        Task<bool> UploadDatatoDB(string data);
    }
}
