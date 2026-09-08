using System;
using System.ComponentModel.DataAnnotations;

namespace DocManager.Models
{
    public class FavoriteDocument
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; } = ""; // Lưu tên tài khoản (email) của người dùng

        public int DocumentId { get; set; } // ID của văn bản được yêu thích

        public DateTime AddedAt { get; set; } = DateTime.Now; // Thời gian lưu
    }
}