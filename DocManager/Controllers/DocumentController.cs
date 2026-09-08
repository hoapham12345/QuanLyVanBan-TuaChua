using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DocManager.Data;
using DocManager.Models;
using QRCoder;
using Microsoft.AspNetCore.Authorization;

namespace DocManager.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DocumentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================================================================
        // 1. TRANG DANH SÁCH & BỘ MÁY TRA CỨU VĂN BẢN (INDEX)
        // =========================================================================
        public async Task<IActionResult> Index(string searchString, string category, DateTime? fromDate, DateTime? toDate)
        {
            // 1. Khai báo currentUser dùng chung cho cả hàm ở ngay đầu tiên (Sửa lỗi trùng tên)
            var currentUser = User.Identity?.Name ?? "";

            // 2. Lấy toàn bộ văn bản đã duyệt
            var documents = _context.Documents.Where(d => d.Status == ApprovalStatus.Approved).AsQueryable();

            // 3. KIỂM TRA QUYỀN VÀ LỌC TÀI LIỆU
            bool isAdmin = User.IsInRole("Admin");
            if (!isAdmin)
            {
                // Tìm xem user này đang thuộc phòng ban nào
                var userDeptObj = await _context.UserDepartments.FirstOrDefaultAsync(u => u.UserName == currentUser);
                string myDeptCode = userDeptObj?.DepartmentCode ?? "";

                // Thêm d.AllowedDepartments != null để sửa lỗi cảnh báo Null
                documents = documents.Where(d =>
                    d.AllowedDepartments == "ALL" ||
                    (myDeptCode != "" && d.AllowedDepartments != null && d.AllowedDepartments.Contains(myDeptCode))
                );
            }

            // 4. LỌC THEO TỪ KHÓA, DANH MỤC, NGÀY THÁNG (Giữ lại logic cũ của bạn)
            if (!string.IsNullOrEmpty(searchString))
            {
                documents = documents.Where(d => d.Title.Contains(searchString) || d.DocumentNumber.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(category))
            {
                documents = documents.Where(d => d.Category == category);
            }
            if (fromDate.HasValue)
            {
                documents = documents.Where(d => d.IssueDate >= fromDate.Value);
            }
            if (toDate.HasValue)
            {
                documents = documents.Where(d => d.IssueDate <= toDate.Value);
            }

            // Lưu lại giá trị tìm kiếm để hiển thị giữ nguyên trạng thái trên Form
            ViewData["CurrentSearch"] = searchString;
            ViewData["CurrentCategory"] = category;
            ViewData["FromDate"] = fromDate?.ToString("yyyy-MM-dd");
            ViewData["ToDate"] = toDate?.ToString("yyyy-MM-dd");

            // 5. LẤY DANH SÁCH NGÔI SAO DÙNG CHUNG BIẾN currentUser Ở TRÊN
            ViewBag.FavoriteDocIds = await _context.FavoriteDocuments
                .Where(f => f.UserName == currentUser)
                .Select(f => f.DocumentId)
                .ToListAsync();

            return View(await documents.OrderByDescending(d => d.IssueDate).ToListAsync());
        }

        // =========================================================================
        // 2. TRANG TẠO MỚI - GET (MỞ FORM NHẬP)
        // =========================================================================
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // =========================================================================
        // 3. XỬ LÝ LƯU & UPLOAD FILE - POST
        // =========================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Document document, IFormFile uploadFile, string[] selectedDepts)
        {
            // Bỏ qua kiểm tra 2 trường này vì hệ thống sẽ tự điền
            ModelState.Remove("FilePath");
            ModelState.Remove("CreatedBy");

            if (ModelState.IsValid)
            {
                // ====================================================
                // 1. XỬ LÝ UPLOAD FILE 
                // ====================================================
                if (uploadFile != null && uploadFile.Length > 0)
                {
                    var uploadsFolder = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!System.IO.Directory.Exists(uploadsFolder))
                    {
                        System.IO.Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadFile.FileName;
                    var filePath = System.IO.Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                    {
                        await uploadFile.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn vào database để hiển thị nút Tải về
                    document.FilePath = "/uploads/" + uniqueFileName;
                }

                // ====================================================
                // 2. GÁN NGƯỜI TẠO & PHÂN QUYỀN PHÒNG BAN
                // ====================================================
                document.CreatedBy = User.Identity?.Name ?? "Hệ thống";
                document.CreatedAt = DateTime.Now;

                if (selectedDepts != null && selectedDepts.Length > 0)
                {
                    if (selectedDepts.Contains("ALL"))
                    {
                        document.AllowedDepartments = "ALL";
                    }
                    else
                    {
                        document.AllowedDepartments = string.Join(",", selectedDepts);
                    }
                }
                else
                {
                    document.AllowedDepartments = "ALL"; // Mặc định
                }

                // ====================================================
                // 3. LOGIC PHÊ DUYỆT (PENDING / APPROVED)
                // ====================================================
                if (User.IsInRole("Admin"))
                {
                    // Admin up thì tự động duyệt và hiện lên trang chủ
                    document.Status = ApprovalStatus.Approved;
                }
                else
                {
                    // User thường up thì đưa vào hàng chờ, KHÔNG ĐƯỢC HIỆN lên trang chủ
                    document.Status = ApprovalStatus.Pending;
                }

                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(document);
        }

        // =========================================================================
        // 4. CHỨC NĂNG TẢI FILE VỀ
        // =========================================================================
        [Authorize]
        public async Task<IActionResult> Download(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return NotFound("Không tìm thấy đường dẫn file.");
            }

            // 1. GHI LỊCH SỬ TẢI XUỐNG VÀO DATABASE
            var document = await _context.Documents.FirstOrDefaultAsync(d => d.FilePath == filePath);
            var currentUser = User.Identity?.Name ?? "Khách";

            if (document != null)
            {
                var log = new DocumentLog
                {
                    DocumentId = document.Id,
                    ActionType = "Download",
                    UserName = currentUser,
                    Timestamp = DateTime.Now,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
                };
                _context.DocumentLogs.Add(log);
                await _context.SaveChangesAsync();
            }

            var exactPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));
            if (!System.IO.File.Exists(exactPath))
            {
                return NotFound("File gốc không còn tồn tại trên máy chủ.");
            }

            // ====================================================================
            // 2. NẾU LÀ FILE PDF -> TIẾN HÀNH ĐÓNG DẤU MỜ (WATERMARK)
            // ====================================================================
            if (exactPath.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                using (var reader = new iTextSharp.text.pdf.PdfReader(exactPath))
                {
                    using (var ms = new System.IO.MemoryStream())
                    {
                        using (var stamper = new iTextSharp.text.pdf.PdfStamper(reader, ms))
                        {
                            int pageCount = reader.NumberOfPages;
                            // Nội dung chữ chìm (Viết không dấu để tránh lỗi font mặc định)
                            var watermarkText = $"Tai boi: {currentUser} - Ngay: {DateTime.Now:dd/MM/yyyy} - Luu hanh noi bo";

                            // Cấu hình Font chữ (Helvetica mặc định) và độ mờ
                            var baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA, iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);
                            var gstate = new iTextSharp.text.pdf.PdfGState { FillOpacity = 0.25f, StrokeOpacity = 0.25f }; // Độ mờ 25%

                            // Lặp qua từng trang PDF để đóng dấu
                            for (int i = 1; i <= pageCount; i++)
                            {
                                var pdfPage = stamper.GetOverContent(i);
                                pdfPage.SetGState(gstate);
                                pdfPage.BeginText();
                                pdfPage.SetFontAndSize(baseFont, 32); // Kích cỡ chữ 32
                                pdfPage.SetColorFill(new iTextSharp.text.BaseColor(100, 100, 100));

                                // Lấy kích thước trang để căn giữa hoàn hảo
                                var pageSize = reader.GetPageSizeWithRotation(i);
                                float x = pageSize.Width / 2;
                                float y = pageSize.Height / 2;

                                // Chèn chữ vào giữa trang, nghiêng 45 độ
                                pdfPage.ShowTextAligned(iTextSharp.text.Element.ALIGN_CENTER, watermarkText, x, y, 45);
                                pdfPage.EndText();
                            }
                        }
                        // Trả file PDF đã đóng dấu về cho trình duyệt
                        return File(ms.ToArray(), "application/pdf", System.IO.Path.GetFileName(exactPath));
                    }
                }
            }

            // ====================================================================
            // 3. NẾU KHÔNG PHẢI LÀ FILE PDF (VD: DOCX, XLSX) -> CHO TẢI BÌNH THƯỜNG
            // ====================================================================
            var memory = new System.IO.MemoryStream();
            using (var stream = new System.IO.FileStream(exactPath, System.IO.FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "application/octet-stream", System.IO.Path.GetFileName(exactPath));
        }
        // 5. TRANG THỐNG KÊ (DASHBOARD)
        // =========================================================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Statistics()
        {
            var allDocs = await _context.Documents.ToListAsync();

            // Tính toán các con số thống kê bằng LINQ
            var model = new DocumentStatisticsViewModel
            {
                TotalDocuments = allDocs.Count,

                // Nhóm theo loại văn bản và đếm số lượng
                DocumentsByCategory = allDocs
                    .GroupBy(d => string.IsNullOrEmpty(d.Category) ? "Chưa phân loại" : d.Category)
                    .ToDictionary(g => g.Key, g => g.Count()),

                // Nhóm theo Năm và Tháng ban hành
                DocumentsByMonth = allDocs
                    .GroupBy(d => new { d.IssueDate.Year, d.IssueDate.Month })
                    .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                    .ToDictionary(g => $"Tháng {g.Key.Month}/{g.Key.Year}", g => g.Count())
            };

            return View(model);
        }
        // =========================================================================
        // 6. TẠO MÃ QR CHO TÀI LIỆU
        // =========================================================================
        public IActionResult GenerateQRCode(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return BadRequest();
            }

            // Khởi tạo bộ sinh mã QR
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                // Mức độ ECCLevel.Q (25%) giúp mã QR vẫn đọc tốt nếu giấy in bị mờ hoặc rách nhẹ
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);

                // PngByteQRCode là class tối ưu nhất cho web .NET 8 để xuất file PNG
                using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
                {
                    // Số 10 là kích thước pixel của từng ô vuông đen trắng trong mã QR
                    byte[] qrCodeImage = qrCode.GetGraphic(10);
                    return File(qrCodeImage, "image/png");
                }
            }
        }
        // 7. SỬA VĂN BẢN (GET & POST)
        // =========================================================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var document = await _context.Documents.FindAsync(id);
            if (document == null) return NotFound();
            return View(document);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Document document, IFormFile? uploadFile)
        {
            if (id != document.Id) return NotFound();

            ModelState.Remove("FilePath");
            ModelState.Remove("CreatedBy");
            ModelState.Remove("uploadFile");

            if (ModelState.IsValid)
            {
                try
                {
                    var existingDoc = await _context.Documents.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
                    document.FilePath = existingDoc?.FilePath;
                    document.CreatedBy = existingDoc?.CreatedBy;
                    document.CreatedAt = existingDoc?.CreatedAt ?? DateTime.Now;

                    if (uploadFile != null && uploadFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await uploadFile.CopyToAsync(stream);
                        }
                        document.FilePath = "/uploads/" + uniqueFileName;
                    }

                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Documents.Any(e => e.Id == document.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(document);
        }
        // 8. XÓA VĂN BẢN (POST)
        // =========================================================================
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document != null)
            {
                if (!string.IsNullOrEmpty(document.FilePath))
                {
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", document.FilePath.TrimStart('/'));
                    if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
                }
                _context.Documents.Remove(document);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ToggleFavorite(int documentId)
        {
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName)) return Json(new { success = false });

            // Kiểm tra xem đã lưu chưa
            var existingFav = await _context.FavoriteDocuments
                .FirstOrDefaultAsync(f => f.DocumentId == documentId && f.UserName == userName);

            if (existingFav != null)
            {
                // Nếu đã có -> Xóa (Bỏ thích)
                _context.FavoriteDocuments.Remove(existingFav);
                await _context.SaveChangesAsync();
                return Json(new { isFavorite = false });
            }
            else
            {
                // Nếu chưa có -> Thêm mới (Yêu thích)
                _context.FavoriteDocuments.Add(new FavoriteDocument { DocumentId = documentId, UserName = userName });
                await _context.SaveChangesAsync();
                return Json(new { isFavorite = true });
            }
        }

        // 2. Màn hình "Tài liệu của tôi"
        [Authorize]
        public async Task<IActionResult> MyFavorites()
        {
            var userName = User.Identity?.Name;

            var favDocIds = await _context.FavoriteDocuments
                .Where(f => f.UserName == userName)
                .Select(f => f.DocumentId)
                .ToListAsync();

            var docs = await _context.Documents
                .Where(d => favDocIds.Contains(d.Id))
                .OrderByDescending(d => d.IssueDate)
                .ToListAsync();

            return View(docs);
        }

    // XỬ LÝ LƯU CẬP NHẬT VĂN BẢN (Dành cho Admin)
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditDocument(int Id, string DocumentNumber, string Title, string Category, DateTime IssueDate, string[] editSelectedDepts)
    {
        var document = await _context.Documents.FindAsync(Id);
        if (document != null)
        {
            // Cập nhật các thông tin cơ bản
            document.DocumentNumber = DocumentNumber;
            document.Title = Title;
            document.Category = Category;
            document.IssueDate = IssueDate;

            // Cập nhật lại Phân quyền phòng ban
            if (editSelectedDepts != null && editSelectedDepts.Length > 0)
            {
                document.AllowedDepartments = editSelectedDepts.Contains("ALL")
                    ? "ALL"
                    : string.Join(",", editSelectedDepts);
            }
            else
            {
                document.AllowedDepartments = "ALL";
            }

            _context.Update(document);
            await _context.SaveChangesAsync();
        }
        // Sửa xong tự động load lại trang tra cứu hiện tại
        return RedirectToAction(nameof(Index));
    }

    }
}
