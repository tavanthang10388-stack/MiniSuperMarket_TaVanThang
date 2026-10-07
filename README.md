# 🛒 DỰ ÁN HỆ THỐNG QUẢN LÝ SIÊU THỊ MINI (MINISUPERMARKET SYSTEM)

## 👤 THÔNG TIN SINH VIÊN BÁO CÁO
* **Họ và tên:** Tạ Văn Thắng
* **Mã sinh viên:** 2124110256
* **Lớp học phần:** CCQ2411D
* **Học kỳ:** Học kỳ 1 - Năm học 2026-2027

---

## 📅 BÁO CÁO CHI TIẾT - BUỔI 3: TÍCH HỢP SQL SERVER VÀ ENTITY FRAMEWORK CORE CODE-FIRST

### 1. MỤC TIÊU VÀ BỐI CẢNH THỰC TẾ
* **Mục tiêu:** 
  * Thay thế hoàn toàn cơ chế lưu tạm trên RAM (In-Memory) ở Buổi 1 và Buổi 2 bằng hệ quản trị cơ sở dữ liệu quan hệ **Microsoft SQL Server**.
  * Làm chủ kỹ thuật **Entity Framework Core (EF Core) Code-First**: Định nghĩa cấu trúc lớp C# (Entities) trước rồi tự động sinh cấu trúc bảng tương ứng trong CSDL.
  * Nắm vững các công cụ dòng lệnh **EF Core Migrations** và tối ưu hiệu năng truy vấn dữ liệu bất đồng bộ bằng **LINQ (async/await)**.
* **Bối cảnh thực tế đồ án:** Khi nhân viên thu ngân thực hiện các thao tác thêm mới, sửa, hoặc xóa dữ liệu nhóm hàng (Category) và sản phẩm (Product) trên giao diện desktop WinForms, toàn bộ dữ liệu phải được lưu trữ vĩnh viễn trong CSDL `TaVanThang_MiniSupermarketDb`. Khi tắt ứng dụng hoặc khởi động lại API, các bản ghi dữ liệu vẫn được đảm bảo toàn vẹn.

---

### 📂 2. CẤU TRÚC THƯ MỤC TOÀN CỤC CỦA DỰ ÁN
```text
MiniSupermarketSystem/
│
├── MiniSupermarket.API/          # Dự án Web API (Backend)
│   ├── Controllers/              
│   │   └── CategoriesController.cs # Xử lý API CRUD kết nối trực tiếp với DB (async/await)
│   ├── Data/                     
│   │   └── SupermarketDbContext.cs # Quản lý kết nối Database và cấu hình nạp sẵn dữ liệu mồi
│   ├── Migrations/               # Thư mục tự động sinh ra khi thực thi lệnh Migration quản lý DB
│   ├── Models/                   
│   │   ├── Category.cs           # Thực thể Nhóm hàng (Data Annotations & Quan hệ 1-N)
│   │   └── Product.cs            # Thực thể Sản phẩm (Cấu hình kiểu dữ liệu & Khóa ngoại)
│   ├── appsettings.json          # Cấu hình Chuỗi kết nối kết nối (ConnectionStrings - sa/123456)
│   └── Program.cs                # Đăng ký DbContext vào kiến trúc Dependency Injection (DI)
│
└── MiniSupermarket.WinForms/     # Dự án Windows Forms (Frontend Client)
    └── FormCategoryManagement.cs # Giao diện đồ họa tương tác và thực thi gọi API Client
```

---

### 🛠️ 3. NỘI DUNG CÁC BƯỚC ĐÃ THỰC HIỆN

#### Bước 1: Thiết lập thư viện nền tảng (NuGet Packages)
Đã cài đặt thành công 3 gói thư viện nền tảng dành cho dự án Backend (`MiniSupermarket.API`):
1. `Microsoft.EntityFrameworkCore.SqlServer` - Trình điều khiển giao tiếp hạ tầng SQL Server.
2. `Microsoft.EntityFrameworkCore.Tools` - Cung cấp bộ lệnh quản trị cơ sở dữ liệu trong PMC.
3. `Microsoft.EntityFrameworkCore.Design` - Hỗ trợ môi trường sinh mã tự động cấu trúc bảng CSDL.

#### Bước 2: Khai báo kết nối hệ thống phòng máy (`appsettings.json`)
Sử dụng tài khoản quản trị hệ thống cao cấp `sa` với mật khẩu được thiết lập lại đồng bộ `123456` tại phòng thực hành:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=TaVanThang_MiniSupermarketDb;User Id=sa;Password=123456;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

#### Bước 3: Đăng ký dịch vụ lõi (`Program.cs`)
Tiêm phụ thuộc cấu hình kết nối ứng dụng thông qua dịch vụ hệ thống:
```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SupermarketDbContext>(options =>
    options.UseSqlServer(connectionString));
```

#### Bước 4: Đồng bộ hóa cơ sở dữ liệu (EF Core Migrations)
Khởi chạy thành công bộ mã tạo dựng cơ sở dữ liệu trong cửa sổ *Package Manager Console*:
1. **Lệnh tạo ánh xạ cấu trúc hình thể:** 
   ```powershell
   Add-Migration InitialCreateDatabase1
   ```
2. **Lệnh áp dụng thực tế:** 
   ```powershell
   Update-Database
   ```
   *Kết quả thực thi:* Hệ thống biên dịch thành công và báo trạng thái **`Done.`**. Hệ quản trị SQL Server Management Studio (SSMS) tự động khởi tạo cơ sở dữ liệu `TaVanThang_MiniSupermarketDb` với đầy đủ ràng buộc khóa chính, khóa ngoại của hai bảng dữ liệu `Categories`, `Products`, cùng dữ liệu mồi mặc định (5 nhóm hàng).

#### Bước 5: Viết bộ điều khiển API bất đồng bộ (`CategoriesController.cs`)
Xóa bỏ hoàn toàn lớp dữ liệu bộ nhớ tạm cứng thời gian trước. Chuyển đổi sang gọi dữ liệu qua thực thể `_context` bằng kỹ thuật xử lý đa luồng độc lập như `ToListAsync()`, `FindAsync(id)`, và `SaveChangesAsync()` để ứng dụng chạy mượt mà, tối ưu tài nguyên phần cứng.

---

### 🚀 4. HƯỚNG DẪN KHỞI CHẠY VÀ KIỂM THỬ DỰ ÁN

#### Bước 1: Khởi động và chạy phía Backend (Web API)
1. Mở Solution bằng phần mềm **Visual Studio 2022**.
2. Nhấp chuột phải vào dự án **`MiniSupermarket.API`** chọn **Set as Startup Project**.
3. Nhấn phím **F5** để chạy dịch vụ. Trình duyệt mặc định tự động mở ra giao diện trực quan **Swagger UI**.
4. Thực hiện thử nghiệm tính năng `GET /api/Categories`, hệ thống trả về mã trạng thái `200 OK` cùng chuỗi nội dung dữ liệu JSON chứa đúng 5 thực thể danh mục mẫu được gọi từ SQL Server lên.

#### Bước 2: Khởi động và chạy phía Frontend (WinForms Client)
1. Cấu hình cổng kết nối (Port) HttpClient ở tầng WinForms trùng khớp với Port đang chạy thực tế của Web API (`https://localhost:XXXXX`).
2. Nhấp chuột phải vào dự án **`MiniSupermarket.WinForms`** ➔ Chọn **Debug** ➔ **Start New Instance**.
3. Trải nghiệm thao tác trên giao diện: Click nút **Tải danh sách**, thực hiện nghiệp vụ **Thêm mới**, **Sửa đổi thông tin**, và **Xóa** trực tiếp. Dữ liệu thay đổi cập nhật đồng bộ tức thì, chính xác xuống bảng cơ sở dữ liệu bên phần mềm Microsoft SQL Server Management Studio (SSMS).
