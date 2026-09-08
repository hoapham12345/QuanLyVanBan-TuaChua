using System;
using System.ComponentModel.DataAnnotations;

namespace DocManager.Models // (Đổi tên namespace theo dự án của bạn nếu cần)
{
    public class DocumentLog
    {
        [Key]
        public int Id { get; set; }

        public int DocumentId { get; set; }

        [Required]
        public string ActionType { get; set; } = ""; // Ghi nhận hành động: "Download" hoặc "View"

        public string UserName { get; set; } = ""; // Người thực hiện

        public DateTime Timestamp { get; set; } = DateTime.Now; // Thời gian thực hiện

        public string? IPAddress { get; set; } // Địa chỉ IP của người dùng
    }
}