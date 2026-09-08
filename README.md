
```markdown
# 🏛️ Hệ thống Quản lý và Tra cứu Văn bản - UBND Tủa Chùa

Hệ thống web hỗ trợ số hóa quy trình quản lý, phê duyệt và phân phối văn bản hành chính, được thiết kế đặc thù cho các cơ quan nhà nước. Dự án giúp tối ưu hóa thời gian tra cứu tài liệu, đảm bảo tính bảo mật dữ liệu giữa các phòng ban và cung cấp tiện ích tải tài liệu nhanh qua thiết bị di động.

## 🚀 Các tính năng nổi bật

### 👤 Phân hệ Người dùng (Cán bộ / Chuyên viên)
* **Tra cứu thông minh:** Tìm kiếm văn bản theo Số/Ký hiệu, Trích yếu, Loại văn bản và Ngày ban hành.
* **Lọc văn bản theo thẩm quyền:** Chỉ hiển thị các văn bản Công khai hoặc thuộc quyền truy cập của phòng ban mà người dùng đang công tác.
* **Tải & Quét QR Code:** Tải tệp tin trực tiếp hoặc dùng điện thoại quét mã QR trên màn hình để tải tài liệu nhanh chóng trong các cuộc họp.
* **Đánh dấu lưu trữ:** Tính năng "Ngôi sao" giúp lưu lại các văn bản quan trọng, thường xuyên sử dụng.

### 🛡️ Phân hệ Quản trị viên (Admin)
* **Phê duyệt & Phân quyền Văn bản:** Xét duyệt các tài liệu được tải lên và cấp quyền hiển thị cho toàn cơ quan hoặc giới hạn trong 6 phòng ban đặc thù:
  * *Văn phòng HĐND & UBND*
  * *Phòng Tư pháp*
  * *Phòng Tài nguyên - Môi trường*
  * *Phòng Tài chính - Kế hoạch*
  * *Phòng Giáo dục & Đào tạo*
  * *Phòng Nông nghiệp & PTNT*
* **Quản lý Hồ sơ Cán bộ:** Thêm, sửa, xóa hồ sơ (CRUD) tích hợp giao diện Modal không cần tải lại trang.
* **Liên kết Tài khoản:** Quản lý danh sách tài khoản hệ thống và gán nhân sự vào các phòng ban tương ứng.
* **Đóng dấu bản quyền (Watermark):** Tự động đóng dấu mờ vào tài liệu PDF để bảo mật thông tin.

## 💻 Công nghệ sử dụng
* **Backend:** C#, ASP.NET Core MVC (.NET 6/7/8)
* **Cơ sở dữ liệu:** SQL Server, Entity Framework Core (Code-First)
* **Frontend:** HTML5, CSS3, Razor Pages, Bootstrap 5
* **Thư viện tích hợp:** iTextSharp (Xử lý PDF), QR Code API

## ⚙️ Hướng dẫn Cài đặt & Chạy dự án

Vui lòng làm theo các bước sau để chạy dự án trên môi trường cục bộ (Localhost):

**Bước 1: Clone dự án về máy**
```bash
git clone [https://github.com/Taikhoancuaban/QuanLyVanBan-TuaChua.git](https://github.com/Taikhoancuaban/QuanLyVanBan-TuaChua.git)

```

**Bước 2: Cấu hình Cơ sở dữ liệu**

1. Mở file `appsettings.json`.
2. Thay đổi chuỗi kết nối `DefaultConnection` cho phù hợp với SQL Server của bạn (VD: sử dụng `Server=.;` cho local server).

**Bước 3: Chạy Migration để tạo Database**
Mở **Package Manager Console** trong Visual Studio và chạy lệnh:

```powershell
Update-Database

```

**Bước 4: Khởi chạy hệ thống**

* Nhấn `F5` hoặc nút **Run** trên Visual Studio để khởi động ứng dụng.
* Đăng nhập bằng tài khoản Admin mặc định (nếu đã được cấu hình trong DB) hoặc tạo tài khoản mới để trải nghiệm.

## 📷 Giao diện hệ thống

*(Kéo thả các ảnh chụp màn hình dự án của bạn vào đây, ví dụ: Trang chủ, Form phê duyệt văn bản, Quản lý cán bộ...)*

## 🎓 Tác giả

* **Phạm Duy Hòa**
* **Lớp:** CNTT 17-02 - Đại học Đại Nam
* **Đề tài:** Thực tập tốt nghiệp / Báo cáo Đồ án

---

*Cảm ơn bạn đã quan tâm đến dự án này. Nếu thấy hữu ích, hãy cho dự án một ⭐️ nhé!*

```

**Mẹo nhỏ cho bạn:** Ở phần `## 📷 Giao diện hệ thống` trên GitHub, bạn có thể chỉnh sửa file `README.md` trực tiếp trên web, sau đó copy 1-2 bức ảnh chụp màn hình web của bạn rồi dán thẳng (Ctrl+V) vào khung soạn thảo. GitHub sẽ tự động tạo link ảnh hiển thị rất đẹp mắt!

```
