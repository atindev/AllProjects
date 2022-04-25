using System.Threading.Tasks;

namespace xUnitTestController
{
    public class HomeRepository : IHomeRepository
    {
        public async Task<string> RemoveProject(int projectID, string email)
        {
            await Task.Delay(1);
            return $"{projectID} {email}";
        }
    }
}
