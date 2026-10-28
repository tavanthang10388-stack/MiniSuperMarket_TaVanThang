# 🛒 MINISUPERMARKET SYSTEM
## 📅 BUỔI 2: BẢO MẬT VÀ PHÂN QUYỀN JWT CHO WEB API

### 📂 3. Cấu trúc Thư mục Dự án

```text
MiniSupermarketSystem/
│
├── MiniSupermarket.API/           # Dự án Web API (Backend)
│   ├── Controllers/
│   │   ├── AuthController.cs      # Xử lý đăng nhập và cấp JWT Token
│   │   └── CategoriesController.cs # CRUD, tìm kiếm và phân quyền API
│   ├── Models/
│   │   └── Category.cs           # Lớp thực thể nhóm hàng
│   ├── appsettings.json          # Cấu hình khóa bí mật JWT
│   └── Program.cs                # Cấu hình JWT Authentication và Middleware
│
└── MiniSupermarket.WinForms/      # Dự án Windows Forms (Frontend Client)
    ├── FormLogin.cs              # Giao diện đăng nhập hệ thống
    ├── FormCategoryManagement.cs # Giao diện quản lý danh mục
    ├── SessionManager.cs         # Lưu JWT Token và quyền người dùng
    ├── ApiClientService.cs       # Gọi API kèm Bearer Token
    └── Program.cs                # Cấu hình khởi chạy FormLogin
```

---

### 🚀 4. Hướng dẫn Chạy và Kiểm thử Dự án

#### Bước 1: Chạy phía Backend (Web API)

1. Mở Solution bằng **Visual Studio 2022**.

2. Nhấp chuột phải vào project `MiniSupermarket.API`, chọn **Set as Startup Project**.

3. Nhấn **F5** để chạy Web API. Trình duyệt sẽ mở giao diện **Swagger UI**.

4. Kiểm tra endpoint `POST /api/auth/login` bằng tài khoản mẫu:
   - **Admin:** `admin / 123456`
   - **Cashier:** `cashier / 123456`

5. Khi đăng nhập thành công, hệ thống trả về JWT Token và vai trò người dùng.

6. Sử dụng JWT Token để kiểm thử các endpoint được bảo vệ bằng `[Authorize]`.

#### Bước 2: Kiểm thử Bảo mật và Phân quyền JWT

Thực hiện các trường hợp sau trên Swagger UI:

- **Chưa đăng nhập:** Gọi `GET /api/categories` không có Token → `401 Unauthorized`.
- **Đăng nhập Admin:** Truy cập `GET /api/categories/admin-dashboard` → `200 OK`.
- **Đăng nhập Cashier:** Truy cập `GET /api/categories/staff-pos` → `200 OK`.
- **Cashier truy cập chức năng Admin:** Gọi `GET /api/categories/admin-dashboard` → `403 Forbidden`.
- **Đăng nhập sai mật khẩu:** API trả về `401 Unauthorized`.

Các mã trạng thái trên là kết quả mong đợi khi cấu hình xác thực và phân quyền đúng theo tài liệu thực hành.

#### Bước 3: Chạy phía Frontend (WinForms Client)

1. Đảm bảo cổng (Port) trong `ApiClientService` hoặc `HttpClient` của WinForms khớp với cổng `https://localhost:XXXXX` của Web API đang chạy.

2. Nhấp chuột phải vào project `MiniSupermarket.WinForms`, chọn **Debug → Start New Instance**.

3. Màn hình **FormLogin** xuất hiện, yêu cầu nhập tài khoản và mật khẩu.

4. Đăng nhập bằng tài khoản Admin hoặc Cashier.

5. Khi đăng nhập thành công, hệ thống lưu JWT Token vào `SessionManager` và chuyển đến giao diện `FormCategoryManagement`.

6. Thử nghiệm các chức năng **Tải danh sách, Thêm mới, Sửa, Xóa và Tìm kiếm nhóm hàng**. Các yêu cầu gửi đến API được bảo vệ phải kèm Bearer Token hợp lệ.

7. Kiểm tra việc phân quyền theo vai trò và thông báo lỗi khi Token không hợp lệ hoặc hết hạn.

---
### 👨‍💻 5. Tác giả

- **Họ tên sinh viên:** Tạ Văn Thắng
- **Mã sinh viên:** 2124110256
- **Lớp học phần:** CCQ2411D
- **Học kỳ:** Học kỳ 1 - Năm học 2026-2027
