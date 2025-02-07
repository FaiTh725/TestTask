using Application.Shared.Enums;

namespace Application.Shared.Responses
{
    public class BaseResponse
    {
        public string Description { get; set; } = string.Empty;

        public StatusCode StatusCode { get; set; }
    }
}
