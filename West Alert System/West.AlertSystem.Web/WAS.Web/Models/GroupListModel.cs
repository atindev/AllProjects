namespace WAS.Web.Models
{
    public class GroupListModel
    {
        public FilterType FilterTypes { get; set; }

        public Application.Features.Groups.GetAll.Response Response { get; set; }
    }
}
