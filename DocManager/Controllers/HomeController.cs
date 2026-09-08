using DocManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DocManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // 1. Kiểm tra xem người dùng đã đăng nhập chưa
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                // 2. Nếu là Admin -> Đẩy thẳng sang trang Thống kê
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Statistics", "Document");
                }

                // 3. Nếu là User bình thường -> Đẩy sang trang Tra cứu
                return RedirectToAction("Index", "Document");
            }

            // 4. Nếu chưa đăng nhập -> Trả về giao diện Landing Page giới thiệu (View)
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
