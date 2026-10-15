# 🛒 HỆ THỐNG QUẢN LÝ SIÊU THỊ MINI (MINISUPERMARKET SYSTEM)

**Môn học:** Lập trình Ứng dụng .NET Core  
**Mã môn học:** 229162  
**Buổi thực hành:** Buổi 4 – Thiết kế kiến trúc giao diện WinForms Shell và điều hướng phân quyền  
**Sinh viên:** Tạ Văn Thắng
**Mã sinh viên:** 2124110256
**Lớp học phần:** CCQ2411D  

---

## 1. 📝 Giới thiệu buổi thực hành

Buổi thực hành 4 tập trung xây dựng kiến trúc giao diện WinForms Shell theo mô hình Single-Form Architecture, thiết kế Sidebar và Main Workspace, đồng thời triển khai cơ chế điều hướng và phân quyền người dùng theo ba vai trò: Admin, Cashier và Warehouse.

## 2. 🖥️ Xây dựng giao diện WinForms Shell

### 2.1. FormMainShell.cs

Thiết kế giao diện chính của hệ thống với các thành phần:

- **`panelSidebar`**: Thanh điều hướng bên trái, có chiều rộng 230px, màu xanh đen, chứa logo và các nút chức năng.
- **`panelTopHeader`**: Thanh tiêu đề phía trên, cao 60px, nền trắng, hiển thị tiêu đề màn hình và thông tin người dùng đăng nhập.
- **`panelMainContent`**: Vùng nội dung trung tâm, sử dụng `Dock = Fill` để hiển thị các Form con.
- **`lblTitle`**: Hiển thị tiêu đề màn hình hiện tại.
- **`lblUserInfo`**: Hiển thị thông tin tài khoản đăng nhập.

Các nút chức năng trên Sidebar:

- `btnPOS`: Bán hàng POS.
- `btnCategory`: Quản lý danh mục.
- `btnProduct`: Quản lý sản phẩm.
- `btnCustomer`: Quản lý khách hàng.
- `btnReports`: Báo cáo doanh thu.
- `btnUserManage`: Quản lý tài khoản.
- `btnLogout`: Đăng xuất.

## 3. 🔄 Nhúng động Form con

### 3.1. Hàm OpenChildForm

Xây dựng cơ chế nhúng các Form nghiệp vụ vào `panelMainContent` thay vì mở nhiều cửa sổ riêng biệt.

Các bước thực hiện:

1. Thiết lập `TopLevel = false` cho Form con.
2. Thiết lập `FormBorderStyle = None` để loại bỏ viền cửa sổ.
3. Thiết lập `Dock = Fill` để Form con vừa khít vùng nội dung.
4. Đóng và dọn dẹp Form con cũ trước khi hiển thị Form mới.
5. Đưa Form mới lên phía trước bằng `BringToFront`.
6. Cập nhật tiêu đề màn hình trên `lblTitle`.
7. Sử dụng `HighlightActiveButton` để làm nổi bật nút điều hướng đang được chọn.

**Kết quả:** Giao diện chuyển đổi giữa các chức năng trong cùng một cửa sổ chính, hạn chế việc mở nhiều cửa sổ và tạo trải nghiệm sử dụng thống nhất.

## 4. 🛡️ Phân quyền người dùng (RBAC)

### 4.1. Hàm ApplyRolePermissions

Triển khai cơ chế phân quyền hiển thị chức năng dựa trên vai trò của tài khoản đăng nhập.

| Vai trò | Quyền truy cập |
|---|---|| Admin | Toàn quyền truy cập các chức năng quản lý |
| Cashier | Bán hàng POS và quản lý khách hàng |
| Warehouse | Quản lý danh mục và sản phẩm |

### 4.2. Hàm OpenDefaultScreenByRole

Sau khi đăng nhập thành công, hệ thống tự động mở màn hình làm việc mặc định tương ứng với vai trò người dùng.

- **Admin:** Mở màn hình Quản lý danh mục.
- **Cashier:** Mở màn hình Bán hàng POS.
- **Warehouse:** Mở màn hình Quản lý sản phẩm.

### 4.3. Phân quyền ở cấp độ thao tác

Bên cạnh việc ẩn hoặc hiện các nút trên Sidebar, hệ thống bổ sung kiểm tra quyền trực tiếp tại các sự kiện xử lý chức năng.

Ví dụ: Chỉ tài khoản Admin mới được phép truy cập chức năng Báo cáo doanh thu.

Cách tiếp cận này giúp hạn chế việc truy cập trái phép vào các chức năng không thuộc quyền hạn của người dùng.

## 5. 🧩 Xây dựng các Form nghiệp vụ

### 5.1. FormPOS.cs – Màn hình bán hàng

Các chức năng chính:

- Nhập hoặc quét mã vạch sản phẩm thông qua `txtBarcode`.
- Quản lý danh sách sản phẩm trong giỏ hàng bằng `dgvCart`.
- Tính tổng tiền đơn hàng.
- Tính tiền khách đưa và tiền thừa.
- Thực hiện thanh toán đơn hàng thông qua API.

### 5.2. FormProductManagement.cs – Quản lý sản phẩm

Các chức năng chính:

- Hiển thị danh sách sản phẩm.
- Tìm kiếm sản phẩm.
- Thêm, sửa và xóa thông tin sản phẩm.
- Đồng bộ thông tin danh mục với dữ liệu từ API.

### 5.3. FormUserManagement.cs – Quản trị tài khoản

Chức năng dành riêng cho Admin, bao gồm:

- Thêm mới tài khoản người dùng.
- Quản lý thông tin tài khoản.
- Phân quyền người dùng theo các vai trò Admin, Cashier và Warehouse.

## 6. 🧪 Kiểm thử và xác minh phân quyền

### 6.1. Tài khoản kiểm thử

| Tài khoản | Mật khẩu | Vai trò | Màn hình mặc định |
|---|---|---|---|
| `admin01` | `123456` | Admin | Quản lý danh mục |
| `cashier01` | `123456` | Cashier | Bán hàng POS |
| `ware01` | `123456` | Warehouse | Quản lý sản phẩm |

### 6.2. Kết quả kiểm thử phân quyền

- [x] **Admin:** Sidebar hiển thị đầy đủ các chức năng quản lý.
- [x] **Cashier:** Sidebar chỉ hiển thị Bán hàng POS và Quản lý khách hàng.
- [x] **Warehouse:** Sidebar chỉ hiển thị Quản lý danh mục và Quản lý sản phẩm.
- [x] Hệ thống tự động mở màn hình mặc định theo vai trò.
- [x] Các Form con được nhúng vừa khít trong `panelMainContent`.
- [x] Chuyển đổi giữa các Form không tạo thêm cửa sổ chính.
- [x] Nút điều hướng đang hoạt động được đổi màu để phân biệt với các nút còn lại.

## 7. ✅ Kết quả đạt được

Sau buổi thực hành 4, hệ thống đã triển khai các nội dung chính:

- Xây dựng kiến trúc giao diện WinForms Shell theo mô hình Single-Form Architecture.
- Thiết kế Sidebar, Header và vùng làm việc trung tâm.
- Triển khai cơ chế nhúng động Form con.
- Xây dựng chức năng điều hướng giữa các màn hình nghiệp vụ.
- Áp dụng phân quyền giao diện theo ba vai trò Admin, Cashier và Warehouse.
- Tự động điều hướng đến màn hình mặc định sau khi đăng nhập.
- Bổ sung kiểm tra quyền truy cập tại cấp độ thao tác.
- Kiểm thử giao diện và xác minh kết quả phân quyền bằng các tài khoản mẫu.

## 8. 👨‍💻 Thông tin sinh viên

- **Họ và tên:** Tạ Văn Thắng
- **Mã sinh viên:** 2124110256
- **Lớp học phần:** CCQ2411D
- **Môn học:** Lập trình Ứng dụng .NET Core (229162)
