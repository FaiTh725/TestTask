namespace Application.Shared.Responses
{
    public class DataResponse<T> : BaseResponse
    {
        public required T Data { get; set; }
    }
}
