# 🛒 MINISUPERMARKET SYSTEM

## 📅 BUỔI 6: ENTITY FRAMEWORK CORE NÂNG CAO, PHÂN TRANG LINQ VÀ TÍCH HỢP TOÀN DIỆN HỆ THỐNG POS CLIENT - SERVER

### 📂 3. Cấu trúc Thư mục Dự án

```text
MiniSuperMarket_TaVanThang/
│
├── MiniSupermarket.API/                 # Dự án Web API (Backend)
│   ├── Controllers/
│   │   ├── AuthController.cs            # Đăng nhập và xác thực JWT
│   │   ├── CategoriesController.cs      # API quản lý danh mục
│   │   ├── ProductsController.cs        # CRUD, tìm kiếm và phân trang sản phẩm
│   │   ├── CustomersController.cs       # API quản lý khách hàng
│   │   ├── UsersController.cs           # API quản lý tài khoản
│   │   ├── RolesController.cs           # API quản lý vai trò
│   │   ├── AdoProductController.cs      # Tra cứu mã vạch bằng ADO.NET
│   │   ├── OrdersController.cs          # API quản lý hóa đơn
│   │   ├── CheckoutController.cs        # Xử lý thanh toán POS
│   │   └── ReportsController.cs         # API báo cáo doanh thu
│   │
│   ├── Data/
│   │   ├── SupermarketDbContext.cs      # Quản lý SQL Server bằng EF Core
│   │   └── UserStore.cs                 # Thành phần lưu trữ người dùng
│   │
│   ├── Models/
│   │   ├── Category.cs                  # Thực thể nhóm hàng
│   │   ├── Product.cs                   # Thực thể sản phẩm
│   │   ├── PagedResponseDto.cs          # DTO phân trang và thông tin sản phẩm
│   │   └── CashierRevenueDto.cs         # DTO thống kê doanh thu
│   │
│   ├── Migrations/                      # Quản lý phiên bản CSDL
│   ├── appsettings.json                 # Cấu hình SQL Server và JWT
│   └── Program.cs                       # Đăng ký dịch vụ và Middleware
│
├── MiniSupermarket.WinForms/            # Dự án Windows Forms (Frontend)
│   ├── FormLogin.cs                     # Giao diện đăng nhập
│   ├── FormMainShell.cs                 # Giao diện Sidebar và Workspace
│   ├── FormCategoryManagement.cs        # Quản lý danh mục
│   ├── FormProductManagement.cs         # CRUD, tìm kiếm và phân trang sản phẩm
│   ├── FormCustomerManagement.cs        # Quản lý khách hàng
│   ├── FormPOS.cs                       # Giao diện bán hàng POS
│   ├── FormQuickReport.cs               # Báo cáo doanh thu
│   ├── FormUserManagement.cs            # Quản lý tài khoản
│   ├── FormRoleManagement.cs            # Quản lý vai trò
│   ├── SessionManager.cs                # Lưu JWT Token và Role
│   ├── Services/
│   │   └── ApiClientService.cs          # HttpClient dùng chung, gắn Bearer Token
│   └── Program.cs                       # Khởi chạy ứng dụng WinForms
│
└── MiniSuperMarket_TaVanThang.sln        # Solution Visual Studio 2022
```

**Lưu ý:** Cấu trúc trên kế thừa các phân hệ từ những buổi trước và bổ sung `PagedResponseDto.cs` theo tài liệu Buổi 6. Đây là cấu trúc mục tiêu cần đối chiếu với mã nguồn thực tế.

---

### 🚀 4. Hướng dẫn Chạy và Kiểm thử Dự án

#### Bước 1: Kiểm tra cơ sở dữ liệu SQL Server

1. Mở phần mềm **SQL Server Management Studio (SSMS)**.

2. Kết nối đến SQL Server đang sử dụng.

3. Chọn cơ sở dữ liệu `TaVanThang_MiniSupermarketDb`.

4. Kiểm tra bảng `Products` và `Categories`, đảm bảo có dữ liệu phục vụ phân trang.

5. Kiểm tra quan hệ khóa ngoại giữa `Products.CategoryId` và `Categories.CategoryId`.

6. Đối chiếu bộ dữ liệu mẫu gồm 15 sản phẩm thuộc 5 nhóm hàng theo tài liệu thực hành.

7. Đảm bảo thông tin kết nối trong `appsettings.json` trỏ đúng cơ sở dữ liệu.

#### Bước 2: Chạy phía Backend (Web API)

1. Mở Solution `MiniSuperMarket_TaVanThang.sln` bằng **Visual Studio 2022**.

2. Nhấp chuột phải vào project `MiniSupermarket.API`, chọn **Set as Startup Project**.

3. Kiểm tra `ProductsController.cs` đã được bổ sung chức năng phân trang sử dụng Entity Framework Core.

4. Kiểm tra các kỹ thuật được áp dụng:
   - `Include()`: Truy vấn dữ liệu quan hệ giữa sản phẩm và danh mục.
   - `AsNoTracking()`: Tối ưu truy vấn chỉ đọc.
   - `Contains()`: Tìm kiếm sản phẩm theo từ khóa.
   - `Skip()`: Bỏ qua các bản ghi thuộc trang trước.
   - `Take()`: Lấy số lượng bản ghi theo kích thước trang.
   - `CountAsync()` và `ToListAsync()`: Thực thi truy vấn bất đồng bộ.

5. Nhấn **F5** để chạy Web API và mở giao diện **Swagger UI**.

6. Đăng nhập lấy JWT Token nếu API được bảo vệ bằng `[Authorize]`.

#### Bước 3: Kiểm thử API phân trang sản phẩm trên Swagger

Mở endpoint:

`GET /api/products/paged`

Nhập các tham số:

- `keyword`: Để trống.
- `pageNumber`: `1`.
- `pageSize`: `5`.

Nhấn **Try it out → Execute**.

Kết quả mong đợi khi cơ sở dữ liệu có đúng 15 sản phẩm mẫu:

- HTTP `200 OK`.
- `currentPage`: 1.
- `pageSize`: 5.
- `totalItems`: 15.
- `totalPages`: 3.
- `items`: Danh sách 5 sản phẩm đầu tiên.

Tiếp tục kiểm thử:

- **Trang 2:** `pageNumber=2&pageSize=5` → Hiển thị 5 sản phẩm tiếp theo.
- **Trang 3:** `pageNumber=3&pageSize=5` → Hiển thị 5 sản phẩm cuối.
- **Tìm kiếm:** `keyword=Coca&pageNumber=1&pageSize=5` → Trả về sản phẩm phù hợp.
- **Trang không hợp lệ:** `pageNumber=0` → Hệ thống tự điều chỉnh về trang 1.
- **PageSize vượt giới hạn:** `pageSize=100` → Hệ thống sử dụng giá trị mặc định 5 theo tài liệu.

#### Bước 4: Kiểm thử Eager Loading và dữ liệu quan hệ

1. Truy cập endpoint `GET /api/products/paged`.

2. Thực hiện truy vấn danh sách sản phẩm.

3. Kiểm tra kết quả JSON trả về có đầy đủ:

   - ProductId.
   - Barcode.
   - ProductName.
   - Price.
   - StockQuantity.
   - CategoryId.
   - CategoryName.

4. Kiểm tra trường `CategoryName` hiển thị đúng tên nhóm hàng liên kết với sản phẩm.

5. Mở cửa sổ Output của Visual Studio để theo dõi các câu truy vấn SQL do Entity Framework Core sinh ra, nếu đã bật logging phù hợp.

6. Xác nhận cơ chế phân trang được thực hiện tại SQL Server thay vì tải toàn bộ bảng về WinForms rồi mới phân trang.

#### Bước 5: Chạy phía Frontend (WinForms Client)

1. Đảm bảo Backend Web API đang chạy.

2. Kiểm tra địa chỉ trong `Services/ApiClientService.cs` khớp với cổng `https://localhost:XXXXX` của Web API.

3. Đảm bảo dịch vụ `HttpClient` sử dụng chung và đính kèm Bearer JWT Token hợp lệ cho các API được bảo vệ.

4. Nhấp chuột phải vào project `MiniSupermarket.WinForms`, chọn **Debug → Start New Instance**.

5. Đăng nhập bằng tài khoản có quyền quản lý sản phẩm, ví dụ Admin hoặc Warehouse.

6. Trên `FormMainShell`, chọn **Quản lý sản phẩm**.

7. Giao diện `FormProductManagement` được hiển thị trong `panelMainContent`.

8. Kiểm tra danh sách nhóm hàng được nạp vào ComboBox và dữ liệu sản phẩm được hiển thị trên DataGridView.

#### Bước 6: Kiểm thử chức năng phân trang trên WinForms

1. Mở màn hình `FormProductManagement`.

2. Kiểm tra DataGridView mặc định hiển thị tối đa 5 sản phẩm trên mỗi trang.

3. Kiểm tra nhãn `lblPaging` hiển thị thông tin trang hiện tại, tổng số trang và tổng số sản phẩm.

4. Nhấn nút **Trang sau (btnNext)** để chuyển sang trang tiếp theo.

5. Nhấn nút **Trang trước (btnPrev)** để quay về trang trước.

6. Kiểm tra nút Trang trước bị vô hiệu hóa khi đang ở trang đầu tiên.

7. Kiểm tra nút Trang sau bị vô hiệu hóa khi đang ở trang cuối cùng.

8. Đảm bảo dữ liệu được cập nhật thông qua API bất đồng bộ `async/await`, không làm treo giao diện trong điều kiện hoạt động bình thường.

#### Bước 7: Kiểm thử tìm kiếm và CRUD sản phẩm

1. Nhập từ khóa **Coca** vào ô `txtSearch`.

2. Nhấn nút **Tìm kiếm (btnSearch)**.

3. Kiểm tra DataGridView chỉ hiển thị các sản phẩm phù hợp với từ khóa.

4. Kiểm tra số trang được tính lại theo kết quả tìm kiếm và tự động quay về trang 1.

5. Thử nghiệm chức năng **Thêm mới**:
   - Nhập mã vạch.
   - Nhập tên sản phẩm.
   - Chọn nhóm hàng.
   - Nhập giá bán.
   - Nhập số lượng tồn kho.
   - Nhấn nút Thêm mới.

6. Thử nghiệm chức năng **Cập nhật**: Chọn sản phẩm, thay đổi thông tin và lưu lại.

7. Thử nghiệm chức năng **Xóa**: Chọn sản phẩm và xác nhận xóa.

8. Mở SSMS để đối chiếu dữ liệu trong bảng `Products` sau các thao tác.

**Kết quả mong đợi:** Các thao tác CRUD được cập nhật trực tiếp xuống SQL Server và danh sách phân trang được tải lại phù hợp.

#### Bước 8: Kiểm thử tích hợp Client - Server

1. Khởi chạy đồng thời Backend và WinForms Client.

2. Đăng nhập hệ thống bằng tài khoản hợp lệ.

3. Mở chức năng Quản lý sản phẩm.

4. Thực hiện tìm kiếm, phân trang và các thao tác CRUD.

5. Kiểm tra dữ liệu hiển thị trên WinForms khớp với kết quả API trên Swagger.

6. Kiểm tra các yêu cầu HTTP được gửi kèm JWT Token khi API yêu cầu xác thực.

7. Tắt ứng dụng WinForms và khởi động lại.

8. Xác nhận dữ liệu sản phẩm đã được lưu thành công vẫn tồn tại trong SQL Server.

---

### 👨‍💻 5. Tác giả

- **Họ tên sinh viên:** Tạ Văn Thắng
- **Mã sinh viên:** 2124110256
- **Lớp học phần:** CCQ2411D
- **Học kỳ:** Học kỳ 1 - Năm học 2026-2027
