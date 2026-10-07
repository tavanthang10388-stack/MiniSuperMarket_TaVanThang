using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Data
{
    public class SupermarketDbContext : DbContext
    {
        public SupermarketDbContext(DbContextOptions<SupermarketDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Bánh kẹo & Đồ ăn vặt", Description = "Snack khoai tây, bánh quy, kẹo dẻo và đồ ăn nhẹ" },
                new Category { CategoryId = 2, CategoryName = "Nước giải khát có ga & Trà", Description = "Nước ngọt, nước tăng lực, trà đóng chai các loại" },
                new Category { CategoryId = 3, CategoryName = "Nước khoáng & Nước tinh khiết", Description = "Nước suối đóng chai, nước khoáng thiên nhiên" },
                new Category { CategoryId = 4, CategoryName = "Sữa & Sản phẩm từ sữa", Description = "Sữa tươi tiệt trùng, sữa chua uống, phô mai que" },
                new Category { CategoryId = 5, CategoryName = "Mì gói & Thực phẩm ăn liền", Description = "Mì ly, phở khô, cháo gói và xúc xích tiệt trùng" },
                new Category { CategoryId = 6, CategoryName = "Cà phê & Đồ uống hòa tan", Description = "Cà phê lon, cà phê đen/sữa đóng gói, trà hòa tan" },
                new Category { CategoryId = 7, CategoryName = "Bánh mì & Thức ăn nhanh", Description = "Bánh mì sandwich, hamburger, cơm nắm Onigiri" },
                new Category { CategoryId = 8, CategoryName = "Kem & Đồ tráng miệng lạnh", Description = "Kem que, kem ốc quế, sữa chua giải nhiệt" },
                new Category { CategoryId = 9, CategoryName = "Đồ hộp & Chế biến sẵn", Description = "Cá hộp, pate gan, thịt hộp và sốt cà đóng hộp" },
                new Category { CategoryId = 10, CategoryName = "Gia vị & Dầu ăn tiện lợi", Description = "Nước mắm, hạt nêm, tương ớt, dầu ăn chai nhỏ" },
                new Category { CategoryId = 11, CategoryName = "Hóa mỹ phẩm & Chăm sóc cá nhân", Description = "Dầu gội, sữa tắm, bàn chải và kem đánh răng" },
                new Category { CategoryId = 12, CategoryName = "Khăn giấy & Vệ sinh cá nhân", Description = "Khăn ướt tiệt trùng, khăn giấy lụa, túi rác mini" },
                new Category { CategoryId = 13, CategoryName = "Văn phòng phẩm & Tiện ích nhỏ", Description = "Bút bi, sổ tay ghi chú mini, bật lửa, kéo" },
                new Category { CategoryId = 14, CategoryName = "Vật dụng cá nhân & Y tế mini", Description = "Khẩu trang kháng khuẩn, băng cá nhân, áo mưa tiện lợi" },
                new Category { CategoryId = 15, CategoryName = "Trái cây & Đồ tươi ăn liền", Description = "Trái cây gọt sẵn, salad đóng hộp, rau củ quả tươi" }
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerId = 1, CustomerName = "Nguyễn Thị Lan", PhoneNumber = "0903456789", Address = "123 Lê Văn Việt, TP. Thủ Đức, TP.HCM", RewardPoints = 250, MembershipRank = "Vàng" },
                new Customer { CustomerId = 2, CustomerName = "Trần Văn Hùng", PhoneNumber = "0918765432", Address = "45 Nguyễn Hữu Thọ, Quận 7, TP.HCM", RewardPoints = 95, MembershipRank = "Bạc" },
                new Customer { CustomerId = 3, CustomerName = "Lê Thị Mai", PhoneNumber = "0987654321", Address = "88 Đường Số 8, Quận Gò Vấp, TP.HCM", RewardPoints = 20, MembershipRank = "Chuẩn" },
                new Customer { CustomerId = 4, CustomerName = "Phạm Minh Quân", PhoneNumber = "0908112233", Address = "15 Tô Ngọc Vân, Quận 4, TP.HCM", RewardPoints = 420, MembershipRank = "Kim cương" },
                new Customer { CustomerId = 5, CustomerName = "Hoàng Anh Tú", PhoneNumber = "0912334455", Address = "67 Phạm Văn Đồng, TP. Thủ Đức, TP.HCM", RewardPoints = 180, MembershipRank = "Vàng" },
                new Customer { CustomerId = 6, CustomerName = "Bùi Thị Hạnh", PhoneNumber = "0977554433", Address = "31 Võ Văn Ngân, TP. Thủ Đức, TP.HCM", RewardPoints = 320, MembershipRank = "Vàng" },
                new Customer { CustomerId = 7, CustomerName = "Đỗ Quốc Khánh", PhoneNumber = "0966443322", Address = "12 Nguyễn Trãi, Quận 5, TP.HCM", RewardPoints = 40, MembershipRank = "Bạc" },
                new Customer { CustomerId = 8, CustomerName = "Võ Thị Nhi", PhoneNumber = "0934567788", Address = "64 Lý Thái Tổ, Quận 10, TP.HCM", RewardPoints = 120, MembershipRank = "Chuẩn" },
                new Customer { CustomerId = 9, CustomerName = "Nguyễn Hoàng Nam", PhoneNumber = "0922334455", Address = "240 Kinh Dương Vương, Quận Bình Tân, TP.HCM", RewardPoints = 260, MembershipRank = "Vàng" },
                new Customer { CustomerId = 10, CustomerName = "Lý Thị Hồng", PhoneNumber = "0944556677", Address = "20 Huỳnh Mẫn Đạt, Quận 5, TP.HCM", RewardPoints = 90, MembershipRank = "Bạc" },
                new Customer { CustomerId = 11, CustomerName = "Phan Thanh Tuấn", PhoneNumber = "0938123456", Address = "54 Hai Bà Trưng, Quận 1, TP.HCM", RewardPoints = 310, MembershipRank = "Vàng" },
                new Customer { CustomerId = 12, CustomerName = "Đặng Thu Thảo", PhoneNumber = "0979888999", Address = "102 Cách Mạng Tháng 8, Quận 3, TP.HCM", RewardPoints = 550, MembershipRank = "Kim cương" },
                new Customer { CustomerId = 13, CustomerName = "Vũ Trọng Phúc", PhoneNumber = "0909654321", Address = "73 Hoàng Hoa Thám, Quận Bình Thạnh, TP.HCM", RewardPoints = 15, MembershipRank = "Chuẩn" },
                new Customer { CustomerId = 14, CustomerName = "Trương Mỹ Linh", PhoneNumber = "0917223344", Address = "215 Nguyễn Thị Thập, Quận 7, TP.HCM", RewardPoints = 85, MembershipRank = "Bạc" },
                new Customer { CustomerId = 15, CustomerName = "Hà Quốc Bảo", PhoneNumber = "0988112244", Address = "36 Quang Trung, Quận Gò Vấp, TP.HCM", RewardPoints = 160, MembershipRank = "Vàng" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, Barcode = "893500123001", ProductName = "Snack khoai tây Lay's vị tự nhiên 56g", Price = 16000m, StockQuantity = 120, CategoryId = 1 },
                new Product { ProductId = 2, Barcode = "893500123002", ProductName = "Nước ngọt Coca-Cola Sleek lon 320ml", Price = 11000m, StockQuantity = 200, CategoryId = 2 },
                new Product { ProductId = 3, Barcode = "893500123003", ProductName = "Nước khoáng thiên nhiên La Vie 500ml", Price = 6000m, StockQuantity = 250, CategoryId = 3 },
                new Product { ProductId = 4, Barcode = "893500123004", ProductName = "Sữa tươi tiệt trùng Vinamilk ít đường 180ml", Price = 10000m, StockQuantity = 150, CategoryId = 4 },
                new Product { ProductId = 5, Barcode = "893500123005", ProductName = "Mì Hảo Hảo tôm chua cay 75g", Price = 4500m, StockQuantity = 300, CategoryId = 5 },
                new Product { ProductId = 6, Barcode = "893500123006", ProductName = "Cà phê sữa Nescafé 3in1 lon 180ml", Price = 15000m, StockQuantity = 80, CategoryId = 6 },
                new Product { ProductId = 7, Barcode = "893500123007", ProductName = "Cơm nắm cá hồi nướng Onigiri 100g", Price = 18000m, StockQuantity = 45, CategoryId = 7 },
                new Product { ProductId = 8, Barcode = "893500123008", ProductName = "Kem que sô cô la Cornetto 80ml", Price = 19000m, StockQuantity = 60, CategoryId = 8 },
                new Product { ProductId = 9, Barcode = "893500123009", ProductName = "Cá nục sốt cà Ba Cô Gái hộp 155g", Price = 22000m, StockQuantity = 75, CategoryId = 9 },
                new Product { ProductId = 10, Barcode = "893500123010", ProductName = "Tương ớt Chin-su chai tiện lợi 250g", Price = 14000m, StockQuantity = 90, CategoryId = 10 },
                new Product { ProductId = 11, Barcode = "893500123011", ProductName = "Sữa tắm dưỡng ẩm Romano Classic 150g", Price = 38000m, StockQuantity = 50, CategoryId = 11 },
                new Product { ProductId = 12, Barcode = "893500123012", ProductName = "Khăn ướt kháng khuẩn Caryn 20 tờ", Price = 15000m, StockQuantity = 110, CategoryId = 12 },
                new Product { ProductId = 13, Barcode = "893500123013", ProductName = "Bút bi gel Thiên Long xanh 0.5mm", Price = 5000m, StockQuantity = 140, CategoryId = 13 },
                new Product { ProductId = 14, Barcode = "893500123014", ProductName = "Khẩu trang y tế 4 lớp (gói 10 cái)", Price = 12000m, StockQuantity = 160, CategoryId = 14 },
                new Product { ProductId = 15, Barcode = "893500123015", ProductName = "Trái cây gọt sẵn ổi ruột hồng 200g", Price = 20000m, StockQuantity = 35, CategoryId = 15 }
            );

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
