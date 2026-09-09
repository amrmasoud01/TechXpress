using System.ComponentModel.DataAnnotations;

namespace techXpress.UI.ActionRequests
{
    public class CreateProductReviewActionRequest
    {
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string? Comment { get; set; }
    }
}
