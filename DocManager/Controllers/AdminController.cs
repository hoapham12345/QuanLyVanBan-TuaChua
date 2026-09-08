using DocManager.Data;
using DocManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Hiển thị danh sách cần duyệt
    public async Task<IActionResult> PendingApprovals()
    {
        var pendingDocs = await _context.Documents
                .Where(d => d.Status == ApprovalStatus.Pending)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();

        return View(pendingDocs);
    }

    // Xử lý thay đổi trạng thái
    [HttpPost]
    public async Task<IActionResult> ChangeStatus(int id, ApprovalStatus newStatus)
    {
        var document = await _context.Documents.FindAsync(id);
        if (document == null)
        {
            return NotFound();
        }

        // Cập nhật trạng thái mới (Approved hoặc Rejected)
        document.Status = newStatus;
        await _context.SaveChangesAsync();

        // Quay lại màn hình danh sách sau khi xử lý xong
        return RedirectToAction(nameof(PendingApprovals));
    }
    // 3. MÀN HÌNH NHẬT KÝ TRUY CẬP (AUDIT LOGS)
    // =========================================================================
    public async Task<IActionResult> AuditLogs()
    {
        // Kết hợp bảng DocumentLogs và Documents để lấy tên văn bản
        var logs = await _context.DocumentLogs
            .Join(_context.Documents,
                log => log.DocumentId,
                doc => doc.Id,
                (log, doc) => new
                {
                    LogId = log.Id,
                    DocumentNumber = doc.DocumentNumber,
                    DocumentTitle = doc.Title,
                    ActionType = log.ActionType,
                    UserName = log.UserName,
                    Timestamp = log.Timestamp,
                    IPAddress = log.IPAddress
                })
            .OrderByDescending(l => l.Timestamp)
            .Take(200) // Chỉ lấy 200 lượt tải gần nhất để trang load nhanh
            .ToListAsync();

        // Truyền dữ liệu sang View thông qua ViewBag
        ViewBag.AuditLogs = logs;

        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveWithPermissions(int id, string[] adminSelectedDepts)
    {
        var document = await _context.Documents.FindAsync(id);
        if (document == null) return NotFound();

        if (adminSelectedDepts != null && adminSelectedDepts.Length > 0)
        {
            document.AllowedDepartments = adminSelectedDepts.Contains("ALL")
                ? "ALL"
                : string.Join(",", adminSelectedDepts);
        }

        document.Status = ApprovalStatus.Approved;
        _context.Update(document);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(PendingApprovals));
    }
    // Hiển thị danh sách User
    public async Task<IActionResult> ManageUsers()
    {
        // Lấy danh sách user từ Identity
        var users = await _context.Users.ToListAsync();
        // Lấy danh sách phòng ban đã gán
        ViewBag.UserDepts = await _context.UserDepartments.ToListAsync();

        return View(users);
    }

    // Xử lý khi Admin bấm Lưu phòng ban cho 1 User
    [HttpPost]
    public async Task<IActionResult> SetUserDepartment(string email, string departmentCode)
    {
        if (string.IsNullOrEmpty(email)) return RedirectToAction(nameof(ManageUsers));

        var existing = await _context.UserDepartments.FirstOrDefaultAsync(u => u.UserName == email);
        if (existing != null)
        {
            existing.DepartmentCode = departmentCode;
            _context.Update(existing);
        }
        else
        {
            _context.UserDepartments.Add(new UserDepartment { UserName = email, DepartmentCode = departmentCode });
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(ManageUsers));
    }
    // 1. MÀN HÌNH QUẢN LÝ HỒ SƠ CÁN BỘ
    public async Task<IActionResult> ManageStaff()
    {
        var staffList = await _context.StaffProfiles.OrderBy(s => s.DepartmentCode).ToListAsync();

        // Lấy toàn bộ danh sách Email tài khoản trong hệ thống để truyền ra Form
        ViewBag.SystemUsers = await _context.Users.Select(u => u.Email).ToListAsync();

        return View(staffList);
    }

    // 2. XỬ LÝ THÊM MỚI CÁN BỘ (Cách lấy trực tiếp, vượt qua lỗi ModelState)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateStaff(string FullName, DateTime DateOfBirth, string PhoneNumber, string Position, string DepartmentCode, string Email)
    {
        // Chỉ cần người dùng nhập Tên là chúng ta cho phép lưu
        if (!string.IsNullOrEmpty(FullName))
        {
            var newStaff = new StaffProfile
            {
                FullName = FullName,
                DateOfBirth = DateOfBirth,
                // Nếu các ô khác bị bỏ trống, ta tự động gán cho nó là chuỗi rỗng để không bị lỗi CSDL
                PhoneNumber = PhoneNumber ?? "",
                Position = Position ?? "",
                DepartmentCode = DepartmentCode ?? "",
                Email = Email ?? ""
            };

            _context.StaffProfiles.Add(newStaff);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(ManageStaff));
    }

    // 3. XỬ LÝ SỬA HỒ SƠ CÁN BỘ
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditStaff(int Id, string FullName, DateTime DateOfBirth, string PhoneNumber, string Position, string DepartmentCode, string Email)
    {
        var staff = await _context.StaffProfiles.FindAsync(Id);
        if (staff != null && !string.IsNullOrEmpty(FullName))
        {
            staff.FullName = FullName;
            staff.DateOfBirth = DateOfBirth;
            staff.PhoneNumber = PhoneNumber ?? "";
            staff.Position = Position ?? "";
            staff.DepartmentCode = DepartmentCode ?? "";
            staff.Email = Email ?? "";

            _context.Update(staff);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(ManageStaff));
    }

    // 4. XỬ LÝ XÓA HỒ SƠ CÁN BỘ
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteStaff(int Id)
    {
        var staff = await _context.StaffProfiles.FindAsync(Id);
        if (staff != null)
        {
            _context.StaffProfiles.Remove(staff);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(ManageStaff));
    }
}