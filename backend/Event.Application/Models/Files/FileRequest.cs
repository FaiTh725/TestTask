namespace Event.Application.Models.Files
{
    public class FileRequest
    {
        public string Name { get; set; } = string.Empty;

        public string ContentType {  get; set; } = string.Empty;

        public required Stream Content { get; set; }
    }
}
