namespace Geta.DdsAdmin.Dds.Responses
{
    public abstract class BaseResponse
    {
        public bool Success { get; set; }
        public int? StatusCode { get; set; }
    }
}
