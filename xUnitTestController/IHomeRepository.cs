using System.Threading.Tasks;

namespace xUnitTestController
{
    public interface IHomeRepository
    {
        Task<string> RemoveProject(int projectID, string email);
    }
}