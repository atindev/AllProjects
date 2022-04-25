namespace Simple.OData.Client.WestInternal
{
    public interface INameMatchResolver
    {
        bool IsMatch(string actualName, string requestedName);
    }
}