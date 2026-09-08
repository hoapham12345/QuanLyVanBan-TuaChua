using System.Collections.Generic;

namespace DocManager.Models
{
    public class DocumentStatisticsViewModel
    {
        public int TotalDocuments { get; set; } // Tổng số văn bản
        public Dictionary<string, int> DocumentsByCategory { get; set; } = new Dictionary<string, int>(); // Phân loại tài liệu
        public Dictionary<string, int> DocumentsByMonth { get; set; } = new Dictionary<string, int>(); // Số lượng theo tháng
    }
}