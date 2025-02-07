namespace Event.API.Configuraions
{
    public class BlobStorage
    {
        public int Port { get; set; }

        public string Key { get; set; } = string.Empty;

        public string AccountName { get; set; } = string.Empty;

        public string BaseUrl { get; set; } = string.Empty;
    }
}
