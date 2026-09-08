// QUAN TRỌNG: Bạn cần using namespace chứa class Document của bạn
// (Thay DocManager bằng đúng tên Project của bạn nếu bạn đặt tên khác)
using DocManager.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;

namespace DocManager.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Bắt buộc phải là DbSet<Document> thay vì chỉ viết DbSet
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocManager.Models.DocumentLog> DocumentLogs { get; set; }
        public DbSet<DocManager.Models.FavoriteDocument> FavoriteDocuments { get; set; }
        public DbSet<DocManager.Models.UserDepartment> UserDepartments { get; set; }
        public DbSet<DocManager.Models.StaffProfile> StaffProfiles { get; set; }
    }
}