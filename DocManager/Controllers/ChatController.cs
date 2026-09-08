using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DocManager.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DocManager.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string userMessage)
        {
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                return Json(new { reply = "Bạn hãy nhập từ khóa để tôi tìm kiếm nhé!" });
            }

            string lowerMsg = userMessage.ToLower();

            // 1. Kịch bản chào hỏi cơ bản
            if (lowerMsg == "chào" || lowerMsg == "hi" || lowerMsg == "xin chào")
            {
                return Json(new { reply = "Chào bạn! Tôi là trợ lý ảo của hệ thống. Bạn đang muốn tìm tài liệu, công văn hay văn bản nào?" });
            }

            // 2. Kịch bản tìm kiếm tài liệu (Quét qua Tên, Số VB và Nội dung)
            var docs = await _context.Documents
                .Where(d => d.Title.Contains(userMessage) ||
                            d.DocumentNumber.Contains(userMessage) ||
                            (d.Content != null && d.Content.Contains(userMessage)))
                .OrderByDescending(d => d.IssueDate)
                .Take(3) // Chỉ đề xuất top 3 tài liệu mới nhất để chat không bị quá dài
                .ToListAsync();

            if (docs.Any())
            {
                string response = "Tôi tìm thấy một vài tài liệu phù hợp với yêu cầu của bạn:<br/><ul class='mt-2 mb-0' style='padding-left: 20px;'>";

                foreach (var d in docs)
                {
                    // Tạo link trỏ thẳng về trang tra cứu với từ khóa tương ứng
                    response += $"<li><a href='/Document?searchString={System.Net.WebUtility.UrlEncode(d.DocumentNumber)}' class='text-primary fw-bold text-decoration-none'>{d.DocumentNumber}</a> - {d.Title}</li>";
                }

                response += "</ul>";
                return Json(new { reply = response });
            }

            // 3. Không tìm thấy
            return Json(new { reply = "Xin lỗi, tôi không tìm thấy tài liệu nào khớp với từ khóa <b>'" + userMessage + "'</b>. Bạn có thể thử dùng từ khóa khác hoặc kiểm tra lại lỗi chính tả nhé!" });
        }
    }
}