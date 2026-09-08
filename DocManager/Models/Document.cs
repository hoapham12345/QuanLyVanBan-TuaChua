using System;
using System.ComponentModel.DataAnnotations;

namespace DocManager.Models // (Lưu ý: Đổi chữ DocManager thành tên project của bạn nếu đặt tên khác)
{
    // 1. Enum định nghĩa các trạng thái của văn bản
    public enum ApprovalStatus
    {
        Draft = 0,      // Nháp (User lưu nháp, chưa gửi)
        Pending = 1,    // Chờ duyệt (User gửi đi, chờ Admin duyệt)
        Approved = 2,   // Đã duyệt (Hiển thị công khai cho mọi người)
        Rejected = 3    // Từ chối (Bị Admin trả về)
    }

    public class Document
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string DocumentNumber { get; set; } = "";

        [Required]
        public string Title { get; set; } = "";

        public string? Content { get; set; }

        public string Category { get; set; } = "";

        public DateTime IssueDate { get; set; }

        public string? PersonInCharge { get; set; }

        public string? FilePath { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // 2. Thuộc tính Trạng thái mới thêm vào (Mặc định khi tạo mới là Pending - Chờ duyệt)
        public ApprovalStatus Status { get; set; } = ApprovalStatus.Pending;

        public string? AllowedDepartments { get; set; }
    }
}