using System.Collections.Generic;

namespace Geta.DdsAdmin.Dds.Responses
{
    public class ReadResponse : BaseResponse
    {
        public int TotalCount { get; set; }
        public List<List<string>> Data { get; set; }
    }
}
