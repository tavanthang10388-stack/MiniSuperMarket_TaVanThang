/*
 * Ho ten sinh vien: Ta Van Thang
 * Mon hoc: Phat trien Ung dung Doanh nghiep voi .NET Core & SQL Server
 * Mo ta: DTO hung du lieu ket qua thong ke tu Stored Procedure
 */
namespace MiniSupermarket.API.Models
{
    public class CashierRevenueDto
    {
        // Ten dang nhap cua thu ngan chiu trach nhiem ca ban
        public string CashierUsername { get; set; } = string.Empty;

        // Tong so luong don hang thu ngan da xu ly thanh cong
        public int TotalOrders { get; set; }

        // Tong so tien thu ve cho sieu thi (VND)
        public decimal TotalRevenue { get; set; }

        // Tong diem thuong da tich luy cho khach hang
        public int TotalPointsAwarded { get; set; }
    }

    public class TopSellingProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int TotalQuantitySold { get; set; }
        public decimal TotalSalesAmount { get; set; }
    }
}
