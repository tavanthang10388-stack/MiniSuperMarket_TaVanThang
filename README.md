# 🛒 MINISUPERMARKET SYSTEM
## 📅 BUỔI 3: TÍCH HỢP SQL SERVER VÀ ENTITY FRAMEWORK CORE CODE-FIRST

### 📂 3. Cấu trúc Thư mục Dự án

```text
MiniSupermarketSystem/
│
├── MiniSupermarket.API/              # Dự án Web API (Backend)
│   ├── Controllers/
│   │   ├── AuthController.cs         # Xác thực và cấp JWT Token
│   │   └── CategoriesController.cs   # CRUD và tìm kiếm danh mục từ SQL Server
│   ├── Data/
│   │   └── SupermarketDbContext.cs   # Kết nối và quản lý cơ sở dữ liệu
│   ├── Models/
│   │   ├── Category.cs               # Thực thể nhóm hàng
│   │   └── Product.cs                # Thực thể sản phẩm
│   ├── Migrations/                   # Quản lý phiên bản CSDL bằng EF Core
│   ├── appsettings.json              # Cấu hình SQL Server và JWT
│   └── Program.cs                    # Đăng ký DbContext và Middleware
│
└── MiniSupermarket.WinForms/         # Dự án Windows Forms (Frontend Client)
    ├── FormLogin.cs                  # Giao diện đăng nhập
    ├── FormCategoryManagement.cs     # Giao diện quản lý danh mục CRUD
    ├── SessionManager.cs             # Quản lý JWT Token
    ├── ApiClientService.cs           # Kết nối và gọi Web API
    └── Program.cs                    # Cấu hình khởi chạy ứng dụng
```

---

### 🚀 4. Hướng dẫn Chạy và Kiểm thử Dự án

#### Bước 1: Kiểm tra kết nối SQL Server

1. Mở phần mềm **SQL Server Management Studio (SSMS)**.

2. Kết nối đến SQL Server bằng tài khoản được cấu hình trên máy thực hành.

3. Kiểm tra cơ sở dữ liệu `TaVanThang_MiniSupermarketDb`.

4. Đảm bảo cơ sở dữ liệu chứa các bảng:
   - `Categories`: Quản lý nhóm hàng.
   - `Products`: Quản lý sản phẩm.
   - `__EFMigrationsHistory`: Lưu lịch sử Migration.

5. Kiểm tra bảng `Categories` có 5 nhóm hàng mẫu được khởi tạo bằng Data Seeding.

#### Bước 2: Chạy Entity Framework Core Migrations

1. Mở Solution bằng **Visual Studio 2022**.

2. Nhấp chuột phải vào project `MiniSupermarket.API`, chọn **Set as Startup Project**.

3. Kiểm tra chuỗi kết nối `DefaultConnection` trong file `appsettings.json`, đảm bảo trỏ đến SQL Server và cơ sở dữ liệu của dự án.

4. Mở **Tools → NuGet Package Manager → Package Manager Console**.

5. Chọn **Default project:** `MiniSupermarket.API`.

6. Nếu chưa tạo Migration, thực hiện:

   `Add-Migration InitialCreateDatabase1`

7. Áp dụng Migration vào SQL Server:

   `Update-Database`

8. Kiểm tra thông báo `Done.` và xác nhận các bảng được tạo trong SSMS.

**Lưu ý:** Nếu Migration đã tồn tại thì không cần chạy lại lệnh Add-Migration. Chỉ chạy Update-Database khi còn Migration chưa được áp dụng.

#### Bước 3: Chạy phía Backend (Web API)

1. Nhấp chuột phải vào project `MiniSupermarket.API` chọn **Set as Startup Project**.

2. Nhấn **F5** để chạy Web API.

3. Trình duyệt sẽ mở giao diện **Swagger UI**.

4. Đăng nhập thông qua `POST /api/auth/login` để lấy JWT Token nếu API đang được bảo vệ bằng `[Authorize]`.

5. Kiểm thử các phương thức API kết nối SQL Server:

   - `GET /api/categories`: Lấy danh sách nhóm hàng.
   - `GET /api/categories/{id}`: Xem chi tiết nhóm hàng.
   - `GET /api/categories/search?keyword=...`: Tìm kiếm nhóm hàng.
   - `POST /api/categories`: Thêm nhóm hàng mới.
   - `PUT /api/categories/{id}`: Cập nhật nhóm hàng.
   - `DELETE /api/categories/{id}`: Xóa nhóm hàng.

6. Kiểm tra dữ liệu trả về từ SQL Server, đặc biệt là 5 nhóm hàng mẫu đã được thêm bằng Data Seeding.

7. Thực hiện thêm mới một nhóm hàng và mở SSMS để xác nhận dữ liệu đã được lưu xuống bảng `Categories`.

#### Bước 4: Chạy phía Frontend (WinForms Client)

1. Đảm bảo cổng (Port) trong `ApiClientService` hoặc `HttpClient` của WinForms trùng với cổng `https://localhost:XXXXX` của Web API.

2. Nhấp chuột phải vào project `MiniSupermarket.WinForms`, chọn **Debug → Start New Instance**.

3. Đăng nhập bằng tài khoản được cấu hình ở Buổi 2.

4. Sau khi đăng nhập thành công, hệ thống chuyển sang giao diện `FormCategoryManagement`.

5. Thử nghiệm các chức năng:
   - **Tải danh sách:** Lấy dữ liệu nhóm hàng từ SQL Server.
   - **Thêm mới:** Thêm nhóm hàng và lưu vào cơ sở dữ liệu.
   - **Sửa:** Cập nhật thông tin nhóm hàng.
   - **Xóa:** Xóa nhóm hàng khỏi cơ sở dữ liệu.
   - **Tìm kiếm:** Tìm nhóm hàng theo tên.

6. Mở SSMS để đối chiếu dữ liệu sau mỗi thao tác.

#### Bước 5: Kiểm thử tính lưu trữ lâu dài của dữ liệu

1. Khởi chạy Backend và WinForms Client.

2. Thêm một nhóm hàng mới, ví dụ: **Đồ dùng gia đình**.

3. Kiểm tra nhóm hàng vừa thêm xuất hiện trên DataGridView.

4. Tắt cả ứng dụng WinForms và Web API.

5. Khởi động lại Backend và WinForms.

6. Nhấn **Tải danh sách** để kiểm tra dữ liệu.

**Kết quả mong đợi:** Nhóm hàng vừa thêm vẫn tồn tại trong SQL Server và hiển thị trên WinForms sau khi khởi động lại ứng dụng.

Điều này chứng minh hệ thống đã chuyển từ lưu trữ tạm thời In-Memory sang lưu trữ lâu dài bằng Microsoft SQL Server.

---

### 👨‍💻 5. Tác giả

- **Họ tên sinh viên:** Tạ Văn Thắng
- **Mã sinh viên:** 2124110256
- **Lớp học phần:** CCQ2411D
- **Học kỳ:** Học kỳ 1 - Năm học 2026-2027
