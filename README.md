

```markdown
# 🏛️ Hệ Thống Hỗ Trợ Quản Lý & Tra Cứu Văn Bản Nội Bộ - UBND Xã Tủa Chùa

![.NET Core](https://img.shields.io/badge/.NET%20Core-8.0-512BD4?style=flat&logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?style=flat&logo=microsoftsqlserver)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=flat&logo=bootstrap)

**Khóa luận / Báo cáo Thực tập Tốt nghiệp**  
* **Cơ sở đào tạo:** Trường Đại học Đại Nam  
* **Ngành:** Công nghệ thông tin  
* **Chuyên ngành:** Hệ thống thông tin  
* **Sinh viên thực hiện:** Phạm Duy Hòa  
* **Đơn vị thực tập:** Văn phòng HĐND & UBND xã Tủa Chùa, tỉnh Điện Biên  

---

## 📖 Giới thiệu tổng quan

Hệ thống được nghiên cứu và phát triển nhằm số hóa toàn diện quy trình tiếp nhận, phê duyệt, phân phối và tra cứu tài liệu hành chính tại **UBND xã Tủa Chùa**. 

Ứng dụng giải quyết triệt để các hạn chế của phương thức quản lý hồ sơ giấy truyền thống: khắc phục tình trạng tra cứu chậm trễ, ngăn ngừa thất lạc tài liệu, đồng thời thiết lập cơ chế kiểm soát truy cập và bảo mật văn bản nghiêm ngặt giữa các phòng ban chức năng.

---

## 🚀 Các tính năng chính

### 1. Phân hệ Người dùng (Cán bộ / Chuyên viên)
* **Tra cứu đa tiêu chí:** Tìm kiếm linh hoạt theo Số/Ký hiệu, Trích yếu nội dung, Loại văn bản và Khoảng thời gian ban hành.
* **Cơ chế Hiển thị thông minh (Smart Filter):** Tự động nhận diện tài khoản đăng nhập để phân phối dữ liệu; chỉ hiển thị các văn bản được cấu hình "Công khai" hoặc văn bản thuộc đúng phòng ban của cán bộ đó.
* **Tiện ích Tải nhanh qua mã QR:** Tự động sinh mã QR cho mỗi tài liệu; hỗ trợ cán bộ sử dụng thiết bị di động quét mã trực tiếp trên màn hình để tải văn bản ngay trong các cuộc họp.
* **Quản lý tài liệu yêu thích:** Đánh dấu sao các văn bản quan trọng để truy cập nhanh khi cần.
* **Đăng tải văn bản đề xuất:** Cán bộ chuyên môn có thể chủ động tải lên các dự thảo/văn bản mới kèm tệp đính kèm (`.pdf`, `.doc`, `.docx`) để chuyển lên cấp quản lý phê duyệt.

### 2. Phân hệ Quản trị viên (Admin)
* **Quy trình Phê duyệt & Cấp quyền:** Xét duyệt các tài liệu đang ở trạng thái chờ duyệt. Admin có toàn quyền quyết định tài liệu sẽ được công khai toàn cơ quan hay giới hạn trong các phòng ban chuyên trách:
  * Văn phòng HĐND & UBND
  * Phòng Tư pháp
  * Phòng Tài nguyên - Môi trường
  * Phòng Tài chính - Kế hoạch
  * Phòng Giáo dục & Đào tạo
  * Phòng Nông nghiệp & Phát triển Nông thôn
* **Tự động đóng dấu bản quyền (Watermark):** Tích hợp xử lý đóng dấu mờ bản quyền vào các tệp PDF nhằm chống sao chép và phát tán trái phép ngoài cơ quan.
* **Quản trị Tài khoản & Phân quyền:** Quản lý hồ sơ cán bộ, liên kết tài khoản hệ thống và gán nhân sự vào từng phòng ban công tác.
* **Báo cáo & Thống kê:** Thống kê tổng hợp số lượng văn bản theo chủng loại, trạng thái xử lý và biểu đồ phát sinh theo thời gian.

---

## 💻 Công nghệ & Công cụ sử dụng

* **Môi trường phát triển:** Microsoft Visual Studio, SQL Server Management Studio (SSMS).
* **Kiến trúc phần mềm:** Mô hình MVC (Model - View - Controller).
* **Backend:** C#, ASP.NET Core MVC.
* **Cơ sở dữ liệu & ORM:** SQL Server, Entity Framework Core (phương pháp tiếp cận Code-First).
* **Frontend:** Razor Pages, HTML5, CSS3, JavaScript.
* **Thư viện giao diện:** Bootstrap 5 (Responsive Layout, Modal Popups, Badges).
* **Thư viện bổ trợ:** 
  * `iTextSharp`: Thao tác xử lý tệp PDF và đóng dấu Watermark tự động.
  * `QR Code API`: Khởi tạo mã phản hồi nhanh 2D.

---

## ⚙️ Hướng dẫn Cài đặt & Chạy ứng dụng

### Yêu cầu môi trường
* .NET SDK 8.0 trở lên.
* Microsoft SQL Server (LocalDB hoặc SQL Server Express).
* Microsoft Visual Studio 2022 (tích hợp ASP.NET and web development).

### Các bước thực hiện

**Bước 1: Clone kho mã nguồn về máy**
```bash
git clone [https://github.com/Taikhoancuaban/QuanLyVanBan-TuaChua.git](https://github.com/Taikhoancuaban/QuanLyVanBan-TuaChua.git)

```

**Bước 2: Cấu hình chuỗi kết nối Database**

Mở file `appsettings.json` trong thư mục gốc của dự án và điều chỉnh chuỗi kết nối tại `DefaultConnection` cho phù hợp với máy của bạn:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=DocManagementDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}

```

**Bước 3: Cập nhật Cơ sở dữ liệu (Migration)**

Mở **Package Manager Console** trong Visual Studio và chạy lệnh sau để tự động sinh toàn bộ các bảng trong CSDL:

```powershell
Update-Database

```

**Bước 4: Khởi chạy dự án**

* Nhấn **F5** hoặc chọn nút **Run** (chế độ `https`) trên thanh công cụ Visual Studio.
* Trình duyệt sẽ tự động điều hướng tới địa chỉ `https://localhost:7xxx`.

---

## 📝 Bản quyền & Ghi chú

* Dự án được xây dựng phục vụ báo cáo thực tập tốt nghiệp ngành Công nghệ thông tin.
* Toàn bộ dữ liệu văn bản, hồ sơ mẫu trong hệ thống đều là dữ liệu phục vụ mục đích kiểm thử và minh họa giải pháp phần mềm.

```

```
