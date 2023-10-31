using Geta.DdsAdmin.Dds;

namespace Geta.DdsAdmin.ViewModels
{
    public class StoreViewModel
    {
        public string StoreName { get; set; }

        public string CustomHeading { get; set; }

        public string CustomMessage { get; set; }

        public StoreMetadata StoreMetadata { get; set; }

        public int[] HiddenColumns { get; set; }

    }
}
