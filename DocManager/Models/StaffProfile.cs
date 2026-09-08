using System;
using System.ComponentModel.DataAnnotations;

namespace DocManager.Models
{
    public class StaffProfile
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string FullName { get; set; } = "";

        public DateTime DateOfBirth { get; set; }

        public string Position { get; set; } = ""; // Chức vụ (VD: Chuyên viên, Trưởng phòng)

        public string DepartmentCode { get; set; } = ""; // Mã phòng ban (VanPhong, TuPhap...)

        public string PhoneNumber { get; set; } = "";

        public string Email { get; set; } = ""; // Liên kết với tài khoản đăng nhập nếu cần
    }
}