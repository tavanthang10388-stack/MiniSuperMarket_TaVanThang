/*
 * Họ tên sinh viên: Tạ Văn Thắng
 * Mã sinh viên: 2124110256
 * Lớp học phần: CCQ2411D
 * Mô tả: DTO phục vụ tra cứu mã vạch và tải đệm ngoại tuyến (Tiết 2 - Buổi 5)
 */
namespace MiniSupermarket.API.Models
{
    // DTO tra cứu sản phẩm cho quầy POS
    public class ProductLookupDto
    {
        public int ProductId { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }

    // DTO đếm số lượng sản phẩm theo danh mục (Bài tập tự luyện 2.2)
    public class CategoryProductCountDto
    {
        public int CategoryId { get; set; }
        public int TotalProducts { get; set; }
    }
}
