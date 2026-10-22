/*
 * Ho ten sinh vien: Ta Van Thang
 * Mon hoc: Phat trien Ung dung Doanh nghiep voi .NET Core & SQL Server
 * Mo ta: Cac lop DTO phuc vu nghiep vu thanh toan gio hang POS
 */
namespace MiniSupermarket.API.Models
{
    // DTO bieu dien tung dong san pham trong gio hang
    public class CartItemCheckoutDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    // DTO nhan yeu cau thanh toan tong the tu may POS (Tiết 5 - Buổi 5)
    public class CheckoutRequestDto
    {
        // Tai khoan thu ngan dang dung ca truc
        public string CashierUsername { get; set; } = string.Empty;

        // Ma dinh danh khach hang (neu la khach vang lai, truyen CustomerId = 15 hoac 1)
        public int CustomerId { get; set; }

        // Danh sach cac mon hang can thanh toan
        public List<CartItemCheckoutDto> Items { get; set; } = new();
    }

    // Alias để tương thích cả AdoCheckoutRequestDto lẫn CheckoutRequestDto
    public class AdoCheckoutRequestDto : CheckoutRequestDto
    {
    }
}
