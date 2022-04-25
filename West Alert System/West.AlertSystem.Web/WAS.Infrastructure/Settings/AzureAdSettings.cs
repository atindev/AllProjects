namespace WAS.Infrastructure.Settings
{
    public class AzureAdSettings
    {
        public string BaseUrl { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Instance { get; set; }

        public string Domain { get; set; }

        public string TenantId { get; set; }

        public string CallbackPath { get; set; }

        public string Scopes { get; set; }

        public string GraphScopes { get; set; }

        public string GraphResourceId { get; set; }

        public string ApiResourceUrl { get; set; }

        public string TokenAuthority { get; set; }
    }
}
