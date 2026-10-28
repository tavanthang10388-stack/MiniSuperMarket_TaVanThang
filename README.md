# 🛒 MINISUPERMARKET SYSTEM

## 📅 BUỔI 4: THIẾT KẾ GIAO DIỆN WINFORMS SHELL VÀ ĐIỀU HƯỚNG PHÂN QUYỀN (ADMIN / CASHIER / WAREHOUSE)

### 📂 3. Cấu trúc Thư mục Dự án

```text
MiniSuperMarket_TaVanThang/
│
├── MiniSupermarket.API/               # Dự án Web API (Backend)
│   ├── Controllers/
│   │   ├── AuthController.cs           # Xác thực và đăng nhập JWT
│   │   ├── CategoriesController.cs     # API quản lý danh mục
│   │   ├── ProductsController.cs       # API quản lý sản phẩm
│   │   ├── CustomersController.cs      # API quản lý khách hàng
│   │   ├── UsersController.cs          # API quản lý tài khoản
│   │   ├── RolesController.cs          # API quản lý vai trò
│   │   ├── OrdersController.cs         # API quản lý đơn hàng
│   │   ├── CheckoutController.cs       # API xử lý thanh toán
│   │   └── ReportsController.cs        # API báo cáo doanh thu
│   │
│   ├── Data/
│   │   ├── SupermarketDbContext.cs     # Quản lý kết nối SQL Server
│   │   └── UserStore.cs                # Thành phần lưu trữ người dùng
│   │
│   ├── Models/                         # Các thực thể dữ liệu hệ thống
│   ├── Migrations/                     # Quản lý phiên bản cơ sở dữ liệu
│   ├── appsettings.json                # Cấu hình SQL Server và JWT
│   └── Program.cs                      # Cấu hình dịch vụ và Middleware
│
├── MiniSupermarket.WinForms/           # Dự án Windows Forms (Frontend)
│   ├── FormLogin.cs                    # Giao diện đăng nhập
│   ├── FormMainShell.cs                # Giao diện chính Sidebar & Workspace
│   ├── FormCategoryManagement.cs       # Quản lý danh mục nhóm hàng
│   ├── FormProductManagement.cs        # Quản lý sản phẩm và tồn kho
│   ├── FormCustomerManagement.cs       # Quản lý khách hàng
│   ├── FormPOS.cs                       # Giao diện bán hàng POS
│   ├── FormQuickReport.cs              # Giao diện báo cáo nhanh
│   ├── FormUserManagement.cs           # Quản lý tài khoản nhân viên
│   ├── FormRoleManagement.cs           # Quản lý vai trò người dùng
│   ├── SessionManager.cs               # Quản lý JWT Token và phiên đăng nhập
│   ├── Services/
│   │   └── ApiClientService.cs         # Kết nối Backend Web API
│   └── Program.cs                      # Khởi chạy ứng dụng WinForms
│
└── MiniSuperMarket_TaVanThang.sln      # Solution Visual Studio 2022
```

---

### 🚀 4. Hướng dẫn Chạy và Kiểm thử Dự án

#### Bước 1: Chạy phía Backend (Web API)

1. Mở Solution `MiniSuperMarket_TaVanThang.sln` bằng **Visual Studio 2022**.

2. Nhấp chuột phải vào project `MiniSupermarket.API`, chọn **Set as Startup Project**.

3. Kiểm tra kết nối SQL Server trong `appsettings.json`, đảm bảo trỏ đến cơ sở dữ liệu của dự án.

4. Nhấn **F5** để chạy Web API và mở giao diện **Swagger UI** nếu đã cấu hình Swagger.

5. Kiểm tra API đăng nhập `POST /api/auth/login` và các API quản lý danh mục, sản phẩm, khách hàng, tài khoản, vai trò.

6. Xác nhận các API yêu cầu xác thực chỉ cho phép truy cập khi có JWT Token hợp lệ.

#### Bước 2: Chạy phía Frontend (WinForms Client)

1. Đảm bảo cổng (Port) trong `Services/ApiClientService.cs` khớp với địa chỉ `https://localhost:XXXXX` của Web API.

2. Nhấp chuột phải vào project `MiniSupermarket.WinForms`, chọn **Debug → Start New Instance**.

3. Đăng nhập bằng tài khoản đã được cấu hình trong hệ thống.

4. Sau khi đăng nhập thành công, mở giao diện chính `FormMainShell`.

5. Kiểm tra ba khu vực giao diện:
   - **panelSidebar:** Thanh menu điều hướng bên trái.
   - **panelTopHeader:** Hiển thị tiêu đề và thông tin tài khoản.
   - **panelMainContent:** Khu vực hiển thị các Form chức năng.

6. Nhấn vào các nút chức năng để kiểm tra việc chuyển đổi màn hình mà không mở nhiều cửa sổ độc lập.

#### Bước 3: Kiểm thử phân quyền người dùng

Kiểm thử với ba nhóm vai trò theo tài liệu thực hành:

| Vai trò | Chức năng được phép |
|---|---|
| Admin | Toàn bộ chức năng quản trị |
| Cashier | Bán hàng POS, quản lý khách hàng |
| Warehouse | Quản lý danh mục và sản phẩm |

**Admin:**
- Hiển thị các chức năng quản trị trong Sidebar.
- Có quyền truy cập quản lý danh mục, sản phẩm, khách hàng, tài khoản, vai trò và báo cáo.

**Cashier:**
- Hiển thị chức năng Bán hàng POS và Quản lý khách hàng.
- Không hiển thị chức năng quản trị và báo cáo doanh thu.

**Warehouse:**
- Hiển thị chức năng Quản lý danh mục và Quản lý sản phẩm.
- Không hiển thị chức năng POS, quản lý tài khoản và báo cáo.

Kết quả phân quyền cần được kiểm tra thực tế theo tài khoản và JWT Role mà Backend cấp.

#### Bước 4: Kiểm thử điều hướng Form động

1. Đăng nhập bằng tài khoản có vai trò Admin.

2. Nhấn **Quản lý danh mục** để hiển thị `FormCategoryManagement`.

3. Nhấn **Quản lý sản phẩm** để hiển thị `FormProductManagement`.

4. Nhấn **Quản lý khách hàng** để hiển thị `FormCustomerManagement`.

5. Nhấn **Bán hàng POS** để mở `FormPOS`.

6. Nhấn **Báo cáo doanh thu** để mở `FormQuickReport`.

7. Nhấn **Quản lý tài khoản** để mở `FormUserManagement`.

8. Kiểm tra màn hình quản lý vai trò `FormRoleManagement` khi có đường điều hướng tương ứng.

9. Xác nhận các Form chức năng được nhúng trong `panelMainContent` bằng kỹ thuật `TopLevel = false` và `Dock = Fill`.

10. Kiểm tra nút Sidebar đang chọn được làm nổi bật và tiêu đề màn hình thay đổi tương ứng.

#### Bước 5: Kiểm thử các chức năng nghiệp vụ

Thực hiện kiểm tra giao diện theo từng phân hệ:

- **Quản lý danh mục:** Tải danh sách, thêm, sửa, xóa và tìm kiếm nhóm hàng.
- **Quản lý sản phẩm:** Hiển thị danh sách, quản lý giá bán, số lượng tồn kho và nhóm hàng.
- **Quản lý khách hàng:** Hiển thị và cập nhật thông tin khách hàng.
- **Bán hàng POS:** Kiểm tra giao diện giỏ hàng, tra cứu sản phẩm bằng Barcode và tính tổng tiền.
- **Báo cáo nhanh:** Kiểm tra giao diện hiển thị thông tin doanh thu.
- **Quản lý tài khoản:** Kiểm tra danh sách người dùng và chức năng quản trị được cấp phép.
- **Quản lý vai trò:** Kiểm tra danh sách và thông tin các vai trò trong hệ thống.

Các chức năng phụ thuộc API cần được kiểm tra với Backend đang chạy và dữ liệu hợp lệ.

#### Bước 6: Kiểm thử đăng xuất và bảo mật

1. Đăng nhập thành công vào hệ thống.

2. Nhấn nút **Đăng xuất** trên giao diện chính.

3. Xác nhận đăng xuất khi hệ thống hiển thị hộp thoại.

4. Kiểm tra JWT Token và thông tin phiên đăng nhập trong `SessionManager` được xóa.

5. Kiểm tra hệ thống quay về màn hình `FormLogin`.

6. Đăng nhập bằng tài khoản có vai trò khác để kiểm tra lại giao diện và quyền hạn.

**Kết quả mong đợi:** Giao diện chính tự động điều chỉnh các chức năng theo vai trò người dùng, điều hướng các Form trong một cửa sổ và không cho phép truy cập trái quyền vào các API được bảo vệ.

---

### 👨‍💻 5. Tác giả

- **Họ tên sinh viên:** Tạ Văn Thắng
- **Mã sinh viên:** 2124110256
- **Lớp học phần:** CCQ2411D
- **Học kỳ:** Học kỳ 1 - Năm học 2026-2027
