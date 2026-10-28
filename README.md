# 🛒 MINISUPERMARKET SYSTEM

## 📅 BUỔI 5: ADO.NET NÂNG CAO, STORED PROCEDURE VÀ GIAO DỊCH THANH TOÁN POS (SQLTRANSACTION)

### 📂 3. Cấu trúc Thư mục Dự án

```text
MiniSuperMarket_TaVanThang/
│
├── MiniSupermarket.API/               # Dự án Web API (Backend)
│   ├── Controllers/
│   │   ├── AuthController.cs           # Xác thực và phân quyền JWT
│   │   ├── CategoriesController.cs     # API quản lý danh mục
│   │   ├── ProductsController.cs       # API quản lý sản phẩm
│   │   ├── CustomersController.cs      # API quản lý khách hàng
│   │   ├── UsersController.cs          # API quản lý tài khoản
│   │   ├── RolesController.cs          # API quản lý vai trò
│   │   ├── AdoProductController.cs     # Tra cứu Barcode bằng ADO.NET
│   │   ├── ReportsController.cs        # Báo cáo doanh thu bằng Stored Procedure
│   │   ├── OrdersController.cs         # API quản lý hóa đơn
│   │   └── CheckoutController.cs       # Thanh toán POS bằng SqlTransaction
│   │
│   ├── Data/
│   │   ├── SupermarketDbContext.cs     # Quản lý kết nối EF Core
│   │   └── UserStore.cs                # Thành phần lưu trữ người dùng
│   │
│   ├── Models/
│   │   ├── Category.cs                 # Thực thể nhóm hàng
│   │   ├── Product.cs                  # Thực thể sản phẩm
│   │   ├── CheckoutDtos.cs             # DTO giỏ hàng và thanh toán
│   │   └── CashierRevenueDto.cs        # DTO thống kê doanh thu thu ngân
│   │
│   ├── Migrations/                     # Quản lý phiên bản CSDL
│   ├── appsettings.json                # Cấu hình SQL Server và JWT
│   └── Program.cs                      # Cấu hình dịch vụ và Middleware
│
├── MiniSupermarket.WinForms/           # Dự án Windows Forms (Frontend)
│   ├── FormLogin.cs                    # Giao diện đăng nhập
│   ├── FormMainShell.cs                # Giao diện Sidebar và Workspace
│   ├── FormCategoryManagement.cs       # Quản lý danh mục
│   ├── FormProductManagement.cs        # Quản lý sản phẩm và tồn kho
│   ├── FormCustomerManagement.cs       # Quản lý khách hàng
│   ├── FormPOS.cs                       # Bán hàng, giỏ hàng và thanh toán
│   ├── FormQuickReport.cs              # Hiển thị báo cáo doanh thu
│   ├── FormUserManagement.cs           # Quản lý tài khoản
│   ├── FormRoleManagement.cs           # Quản lý vai trò
│   ├── SessionManager.cs               # Quản lý phiên đăng nhập JWT
│   ├── Services/
│   │   └── ApiClientService.cs         # Giao tiếp Backend Web API
│   └── Program.cs                      # Khởi chạy ứng dụng
│
└── MiniSuperMarket_TaVanThang.sln      # Solution Visual Studio 2022
```

**Lưu ý:** Cấu trúc trên kết hợp dự án Buổi 4 với các thành phần mới được yêu cầu trong tài liệu Buổi 5. Các file `AdoProductController.cs`, `CheckoutDtos.cs` và `CashierRevenueDto.cs` cần được đối chiếu với mã nguồn sau khi thực hành.

---

### 🚀 4. Hướng dẫn Chạy và Kiểm thử Dự án

#### Bước 1: Kiểm tra SQL Server và ADO.NET

1. Mở **SQL Server Management Studio (SSMS)**.

2. Kết nối đến SQL Server đang sử dụng trong dự án.

3. Chọn cơ sở dữ liệu `TaVanThang_MiniSupermarketDb`.

4. Kiểm tra các bảng dữ liệu phục vụ nghiệp vụ:
   - `Products`: Thông tin sản phẩm và tồn kho.
   - `Categories`: Danh mục nhóm hàng.
   - `Customers`: Thông tin khách hàng và điểm tích lũy.
   - `Orders`: Thông tin hóa đơn bán hàng.
   - `OrderDetails`: Chi tiết sản phẩm trong hóa đơn.

5. Kiểm tra chuỗi kết nối `DefaultConnection` trong `appsettings.json` trỏ đúng đến cơ sở dữ liệu.

6. Đảm bảo dự án Backend đã cài đặt thư viện `Microsoft.Data.SqlClient`.

7. Kiểm tra các thành phần ADO.NET gồm `SqlConnection`, `SqlCommand`, `SqlParameter`, `SqlDataReader`, `SqlDataAdapter` và `DataTable`.

#### Bước 2: Chạy phía Backend (Web API)

1. Mở Solution `MiniSuperMarket_TaVanThang.sln` bằng **Visual Studio 2022**.

2. Nhấp chuột phải vào project `MiniSupermarket.API`, chọn **Set as Startup Project**.

3. Nhấn **F5** để chạy Web API.

4. Trình duyệt mở giao diện **Swagger UI** nếu dự án đã cấu hình Swagger.

5. Kiểm tra các API ADO.NET, báo cáo doanh thu và thanh toán POS.

6. Nếu endpoint yêu cầu JWT Token, thực hiện đăng nhập và cấp quyền trước khi gọi API.

#### Bước 3: Kiểm thử tra cứu sản phẩm bằng Barcode

Trên Swagger UI, tìm đến `AdoProductController`.

Thực hiện kiểm thử:

**Trường hợp 1: Tra cứu mã vạch hợp lệ**

Gọi endpoint:

`GET /api/ado-products/barcode/893456789001`

Kết quả mong đợi:

- HTTP `200 OK`.
- Trả về mã sản phẩm, mã vạch, tên sản phẩm, giá bán và số lượng tồn kho.
- Dữ liệu được đọc trực tiếp từ SQL Server bằng `SqlDataReader`.

**Trường hợp 2: Tra cứu mã vạch không tồn tại**

Gọi endpoint:

`GET /api/ado-products/barcode/999999999999`

Kết quả mong đợi:

- HTTP `404 Not Found`.
- Hiển thị thông báo không tìm thấy sản phẩm.

**Trường hợp 3: Tải danh sách bằng DataTable**

Gọi endpoint:

`GET /api/ado-products/disconnected-cache`

Kết quả mong đợi:

- HTTP `200 OK`.
- Trả về danh sách sản phẩm và tổng số bản ghi đã tải.
- Sử dụng `SqlDataAdapter` kết hợp `DataTable` để đọc dữ liệu theo mô hình Disconnected Mode.

#### Bước 4: Tạo và kiểm thử Stored Procedure

1. Mở **SSMS**, chọn đúng cơ sở dữ liệu `TaVanThang_MiniSupermarketDb`.

2. Mở cửa sổ **New Query**.

3. Tạo Stored Procedure có tên:

   `sp_GetRevenueByCashier`

4. Stored Procedure thực hiện thống kê:
   - Tên tài khoản thu ngân.
   - Tổng số hóa đơn đã xử lý.
   - Tổng doanh thu.
   - Tổng điểm thưởng đã cấp cho khách hàng.

5. Thực thi Stored Procedure bằng lệnh:

   `EXEC sp_GetRevenueByCashier;`

6. Kiểm tra bảng kết quả doanh thu theo từng thu ngân.

7. Quay lại Swagger UI và gọi endpoint:

   `GET /api/reports/revenue-by-cashier`

8. Kiểm tra kết quả JSON trả về gồm tổng số thu ngân, tổng doanh thu và danh sách thống kê.

**Kết quả mong đợi:** API trả về `200 OK` với dữ liệu thống kê khớp kết quả Stored Procedure trên SQL Server.

#### Bước 5: Kiểm thử giao dịch SqlTransaction theo chuẩn ACID

1. Kiểm tra API thanh toán sử dụng `SqlTransaction`.

2. Xác nhận các thao tác ghi dữ liệu được thực hiện trong cùng một Transaction:
   - Tạo hóa đơn vào bảng `Orders`.
   - Thêm chi tiết hóa đơn vào `OrderDetails`.
   - Trừ số lượng tồn kho trong bảng `Products`.
   - Cộng điểm tích lũy vào bảng `Customers`.

3. Kiểm tra cơ chế `BeginTransaction()` khi bắt đầu thanh toán.

4. Kiểm tra `Commit()` khi toàn bộ các thao tác thành công.

5. Kiểm tra `Rollback()` khi một thao tác phát sinh lỗi.

6. Đảm bảo các câu lệnh SQL trong cùng phiên giao dịch đều được gắn với Transaction tương ứng.

#### Bước 6: Kiểm thử API Thanh toán POS

Truy cập Swagger UI và chọn endpoint:

`POST /api/checkout`

**Trường hợp 1: Thanh toán thành công**

Nhập dữ liệu giỏ hàng hợp lệ, ví dụ:

- Thu ngân: `cashier01`.
- Khách hàng: `CustomerId = 1`.
- Sản phẩm ID 1: Số lượng 2.
- Sản phẩm ID 5: Số lượng 3.

Nhấn **Execute** để thực hiện thanh toán.

Kết quả mong đợi:

- HTTP `200 OK`.
- Tạo mới hóa đơn thành công.
- Sinh mã hóa đơn và OrderId.
- Lưu chi tiết sản phẩm.
- Trừ tồn kho đúng số lượng.
- Cộng điểm tích lũy cho khách hàng.
- Transaction được Commit thành công.

**Trường hợp 2: Thanh toán vượt số lượng tồn kho**

Thực hiện yêu cầu mua sản phẩm với số lượng lớn hơn tồn kho thực tế, ví dụ sản phẩm ID 2 với số lượng 999.

Kết quả mong đợi:

- HTTP `400 Bad Request`.
- Hiển thị thông báo sản phẩm không đủ tồn kho.
- Transaction được Rollback.
- Không tạo hóa đơn mới.
- Không lưu chi tiết hóa đơn.
- Không trừ tồn kho.
- Không cộng điểm thưởng.

#### Bước 7: Đối soát dữ liệu trên SQL Server

1. Mở **SQL Server Management Studio**.

2. Kiểm tra bảng `Orders` sau khi thanh toán thành công.

3. Kiểm tra `OrderDetails` để xác nhận danh sách sản phẩm trong hóa đơn.

4. Kiểm tra `Products` để đối chiếu số lượng tồn kho trước và sau thanh toán.

5. Kiểm tra `Customers` để đối chiếu điểm tích lũy.

6. Thực hiện kiểm thử thanh toán thất bại do vượt tồn kho.

7. Xác nhận các bảng không phát sinh bản ghi không hợp lệ hoặc thay đổi dữ liệu ngoài mong muốn sau Rollback.

#### Bước 8: Chạy phía Frontend (WinForms Client)

1. Đảm bảo Backend Web API đang hoạt động.

2. Kiểm tra địa chỉ API trong `Services/ApiClientService.cs` khớp với cổng `https://localhost:XXXXX`.

3. Nhấp chuột phải vào project `MiniSupermarket.WinForms`, chọn **Debug → Start New Instance**.

4. Đăng nhập bằng tài khoản có quyền sử dụng POS.

5. Trên `FormMainShell`, chọn chức năng **Bán hàng (POS)**.

6. Kiểm tra giao diện `FormPOS`:
   - Ô nhập hoặc quét mã Barcode.
   - Danh sách sản phẩm trong giỏ hàng.
   - Số lượng và đơn giá.
   - Tổng tiền thanh toán.
   - Thông tin khách hàng.
   - Tiền khách đưa và tiền thừa.
   - Nút Thanh toán.

7. Thêm sản phẩm vào giỏ hàng và kiểm tra tổng tiền được tính chính xác.

8. Thực hiện thanh toán khi giỏ hàng hợp lệ và Backend đã được tích hợp đúng endpoint.

9. Kiểm tra thông báo kết quả, đồng thời đối chiếu hóa đơn và tồn kho trong SQL Server.

10. Thử trường hợp sản phẩm hết hàng để kiểm tra thông báo lỗi và tính toàn vẹn dữ liệu.

**Lưu ý:** Phần WinForms chỉ hoàn thành luồng thanh toán khi `FormPOS` gọi đúng endpoint và gửi dữ liệu theo DTO mà Backend yêu cầu.

---

### 👨‍💻 5. Tác giả

- **Họ tên sinh viên:** Tạ Văn Thắng
- **Mã sinh viên:** 2124110256
- **Lớp học phần:** CCQ2411D
- **Học kỳ:** Học kỳ 1 - Năm học 2026-2027
