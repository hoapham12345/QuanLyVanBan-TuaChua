using System.ComponentModel.DataAnnotations;

namespace DocManager.Models
{
    public class UserDepartment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; } = ""; // Lưu Email đăng nhập của nhân viên

        public string DepartmentCode { get; set; } = ""; // Lưu mã phòng ban (VanPhong, TuPhap...)
    }
}