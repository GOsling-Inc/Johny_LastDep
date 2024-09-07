using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253502_YASTREMSKIY.Helpers;

namespace WEB_253502_YASTREMSKIY.Models
{
    public class IndexViewModel
    {
        public int SelectionId { get; set; }
        public SelectList Items { get; set; }
    }
}
