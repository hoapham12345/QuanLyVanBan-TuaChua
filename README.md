

```markdown
# 🏛️ Hệ Thống Hỗ Trợ Quản Lý & Tra Cứu Văn Bản Nội Bộ - UBND Xã Tủa Chùa

![.NET Core](https://img.shields.io/badge/.NET%20Core-8.0-512BD4?style=flat&logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?style=flat&logo=microsoftsqlserver)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=flat&logo=bootstrap)

**Khóa luận / Báo cáo Thực tập Tốt nghiệp**  
**Ngành:** Công nghệ thông tin - Đại học Đại Nam  
**Sinh viên thực hiện:** Phạm Duy Hòa  

---

## 📖 Giới thiệu dự án

Đây là giải pháp phần mềm được thiết kế và xây dựng nhằm số hóa quy trình quản lý, phê duyệt và phân phối văn bản hành chính tại **Văn phòng HĐND & UBND xã Tủa Chùa, tỉnh Điện Biên**. 

Hệ thống giúp giải quyết triệt để bài toán tra cứu văn bản thủ công tốn thời gian, đồng thời thiết lập cơ chế bảo mật và phân quyền tài liệu nghiêm ngặt giữa 6 phòng ban đặc thù của cơ quan hành chính nhà nước cấp cơ sở.

## 🚀 Các tính năng nổi bật

### 👤 Dành cho Cán bộ / Chuyên viên (Người dùng)
* **🔍 Tra cứu nâng cao:** Tìm kiếm văn bản theo Số/Ký hiệu, Trích yếu, Loại văn bản và Khoảng thời gian.
* **🛡️ Hiển thị thông minh (Smart Filter):** Hệ thống tự động nhận diện tài khoản và chỉ hiển thị các văn bản được phép tiếp cận (Công khai hoặc thuộc đúng phòng ban của cán bộ).
* **📱 Quét mã QR tiện lợi:** Tự động sinh mã QR cho mỗi văn bản. Cán bộ tham gia cuộc họp có thể dùng smartphone quét mã trên màn hình để tải ngay tài liệu về máy.
* **⭐ Đánh dấu yêu thích:** Lưu trữ nhanh các văn bản, biểu mẫu thường xuyên sử dụng.

### 👑 Dành cho Quản trị viên (Admin)
* **✅ Phê duyệt văn bản:** Kiểm duyệt các văn bản do nhân viên tải lên trước khi công khai lên hệ thống.
* **🔏 Đóng dấu bản quyền (Watermark):** Tự động đóng dấu mờ (Watermark) vào tài liệu PDF ngay khi được phê duyệt để chống sao chép trái phép.
* **👥 Quản lý & Phân quyền:** Quản lý hồ sơ cán bộ, liên kết tài khoản hệ thống và cấp quyền truy cập theo 6 phòng ban chức năng.

---

## 💻 Công nghệ & Công cụ sử dụng

* **Kiến trúc phần mềm:** Mô hình MVC (Model-View-Controller).
* **Backend:** C#, ASP.NET Core MVC.
* **Cơ sở dữ liệu:** SQL Server, Entity Framework Core (Code-First).
* **Frontend:** HTML5, CSS3, Razor Pages, Bootstrap 5.
* **Thư viện / API tích hợp:** 
  * `iTextSharp`: Xử lý PDF và đóng dấu Watermark tự động.
  * `QR Code API`: Khởi tạo mã vạch ma trận 2D.

---

## 📸 Giao diện & Kết quả thực tế

### 1. Màn hình Đăng nhập & Trang chủ
![Trang chủ hệ thống](<img width="623" height="429" alt="Picture1" src="https://github.com/user-attachments/assets/9642ae43-d012-4aa2-9a0c-b01d6bc265a6" />)

> *Giao diện tổng quan hiển thị thống kê tài liệu và bộ lọc tìm kiếm nâng cao.*

### 2. Quản lý phân quyền & Hồ sơ cán bộ (Admin)
![Phân quyền phòng ban](<img width="974" height="825" alt="image" src="https://github.com/user-attachments/assets/b190e834-7797-4385-862c-6ac8781c9d4f" />
)
> *Admin có thể gán quyền xem văn bản theo từng phòng ban cụ thể cho mỗi tài khoản cán bộ.*

### 3. Quy trình Phê duyệt & Cấp quyền văn bản
![Phê duyệt văn bản](<img width="974" height="356" alt="image" src="https://github.com/user-attachments/assets/5c9eeabc-886c-4982-b94f-56881f44e291" />
)
> *Giao diện Modal cho phép Admin phê duyệt và chỉ định văn bản này được phép hiển thị cho những phòng ban nào.*

### 4. Tiện ích Xem chi tiết & Tải qua mã QR
![Mã QR Tải tài liệu](<img width="943" height="454" alt="image" src="https://github.com/user-attachments/assets/55839ec5-9084-4eaf-abb2-27ad804e0663" />
)
> *Người dùng xem trước nội dung tóm tắt và có thể sử dụng điện thoại quét mã QR để tải trực tiếp file đính kèm.*

### 5. Kết quả đóng dấu Watermark tự động
![Watermark PDF](<img width="974" height="621" alt="image" src="https://github.com/user-attachments/assets/262d3ec1-1cbf-4814-9991-e44b3d707d44" />
)
> *File PDF tải về đã được hệ thống tự động đóng dấu bảo mật chìm.*

---

## ⚙️ Hướng dẫn Cài đặt & Chạy dự án (Local)

**Bước 1: Clone dự án về máy**
```bash
git clone [https://github.com/Taikhoancuaban/QuanLyVanBan-TuaChua.git](https://github.com/Taikhoancuaban/QuanLyVanBan-TuaChua.git)

```

**Bước 2: Cấu hình Cơ sở dữ liệu**
Mở file `appsettings.json`, chỉnh sửa chuỗi kết nối `DefaultConnection` phù hợp với cấu hình SQL Server trên máy bạn.

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=DocManagementDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true"
}

```

**Bước 3: Khởi tạo Database (Migration)**
Mở **Package Manager Console** trong Visual Studio và chạy lệnh sau để tạo các bảng dữ liệu:

```powershell
Update-Database

```

**Bước 4: Chạy ứng dụng**

* Mở file `.sln` bằng Visual Studio.
* Nhấn `F5` hoặc nút **Run** (IIS Express / Kestrel) để khởi động hệ thống.
* Sử dụng tài khoản Admin mặc định (nếu có) hoặc tạo tài khoản mới để bắt đầu trải nghiệm.

---

*📝 Dự án được thực hiện nhằm phục vụ mục đích báo cáo thực tập tốt nghiệp. Mọi thông tin, dữ liệu văn bản hiển thị trong hệ thống (nếu có) đều là dữ liệu giả lập/thử nghiệm.*
