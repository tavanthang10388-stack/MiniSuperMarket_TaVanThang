-- =========================================================================
-- HỌ VÀ TÊN SINH VIÊN: TẠ VĂN THẮNG
-- MÔN HỌC: PHÁT TRIỂN ỨNG DỤNG DOANH NGHIỆP .NET CORE & SQL SERVER
-- BUỔI 5: ADO.NET NÂNG CAO, STORED PROCEDURE & GIAO DỊCH POS CHECKOUT
-- Tệp SQL: Tạo bảng Orders, OrderDetails, Seed 15 đơn hàng & 3 Stored Procedures
-- =========================================================================

USE [TaVanThang-SupermarketDb];
-- (Lưu ý: Nếu dùng MiniSupermarketDb, vui lòng sửa lại tên CSDL ở dòng trên)
GO

-- 1. BẢNG ORDERS (HÓA ĐƠN BÁN HÀNG)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Orders')
BEGIN
    CREATE TABLE Orders (
        OrderId INT IDENTITY(1,1) PRIMARY KEY,
        OrderCode VARCHAR(50) NOT NULL,
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        CashierUsername VARCHAR(50) NOT NULL,
        CustomerId INT NULL,
        TotalItems INT NOT NULL,
        FinalAmount DECIMAL(18,2) NOT NULL,
        RewardPoints INT NOT NULL DEFAULT 0
    );
    PRINT '-> Đã tạo thành công bảng Orders.';
END
GO

-- 2. BẢNG ORDERDETAILS (CHI TIẾT MẶT HÀNG TRONG HÓA ĐƠN)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderDetails')
BEGIN
    CREATE TABLE OrderDetails (
        OrderDetailId INT IDENTITY(1,1) PRIMARY KEY,
        OrderId INT NOT NULL,
        ProductId INT NOT NULL,
        Quantity INT NOT NULL,
        UnitPrice DECIMAL(18,2) NOT NULL,
        LineTotal DECIMAL(18,2) NOT NULL,
        CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY (OrderId) REFERENCES Orders(OrderId) ON DELETE CASCADE
    );
    PRINT '-> Đã tạo thành công bảng OrderDetails.';
END
GO

-- 3. NẠP DỮ LIỆU MẪU 15 HÓA ĐƠN KHỚP VỚI KẾT QUẢ TIẾT 3
-- Tổng cộng 15 đơn: cashier03 (2 đơn), cashier05 (3 đơn), cashier01 (4 đơn), cashier04 (3 đơn), cashier02 (3 đơn)
IF (SELECT COUNT(*) FROM Orders) = 0
BEGIN
    -- cashier03: 2 đơn, tổng 1.435.000 đ, 143 điểm
    INSERT INTO Orders (OrderCode, CreatedDate, CashierUsername, CustomerId, TotalItems, FinalAmount, RewardPoints)
    VALUES 
    ('HD-20260830-01', DATEADD(DAY, -10, GETDATE()), 'cashier03', 1, 10, 800000.00, 80),
    ('HD-20260830-02', DATEADD(DAY, -9, GETDATE()),  'cashier03', 2, 8,  635000.00, 63);

    -- cashier05: 3 đơn, tổng 1.328.000 đ, 132 điểm
    INSERT INTO Orders (OrderCode, CreatedDate, CashierUsername, CustomerId, TotalItems, FinalAmount, RewardPoints)
    VALUES 
    ('HD-20260830-03', DATEADD(DAY, -8, GETDATE()), 'cashier05', 3, 5, 520000.00, 52),
    ('HD-20260830-04', DATEADD(DAY, -7, GETDATE()), 'cashier05', 4, 7, 480000.00, 48),
    ('HD-20260830-05', DATEADD(DAY, -6, GETDATE()), 'cashier05', 5, 4, 328000.00, 32);

    -- cashier01: 4 đơn, tổng 1.322.000 đ, 131 điểm
    INSERT INTO Orders (OrderCode, CreatedDate, CashierUsername, CustomerId, TotalItems, FinalAmount, RewardPoints)
    VALUES 
    ('HD-20260830-06', DATEADD(DAY, -5, GETDATE()), 'cashier01', 6, 6, 450000.00, 45),
    ('HD-20260830-07', DATEADD(DAY, -4, GETDATE()), 'cashier01', 7, 5, 380000.00, 38),
    ('HD-20260830-08', DATEADD(DAY, -3, GETDATE()), 'cashier01', 8, 4, 292000.00, 29),
    ('HD-20260830-09', DATEADD(DAY, -2, GETDATE()), 'cashier01', 9, 3, 200000.00, 19);

    -- cashier04: 3 đơn, tổng 484.000 đ, 48 điểm
    INSERT INTO Orders (OrderCode, CreatedDate, CashierUsername, CustomerId, TotalItems, FinalAmount, RewardPoints)
    VALUES 
    ('HD-20260830-10', DATEADD(DAY, -2, GETDATE()), 'cashier04', 10, 4, 210000.00, 21),
    ('HD-20260830-11', DATEADD(DAY, -1, GETDATE()), 'cashier04', 11, 3, 160000.00, 16),
    ('HD-20260830-12', DATEADD(DAY, -1, GETDATE()), 'cashier04', 12, 2, 114000.00, 11);

    -- cashier02: 3 đơn, tổng 253.000 đ, 24 điểm
    INSERT INTO Orders (OrderCode, CreatedDate, CashierUsername, CustomerId, TotalItems, FinalAmount, RewardPoints)
    VALUES 
    ('HD-20260830-13', GETDATE(), 'cashier02', 13, 2, 95000.00, 9),
    ('HD-20260830-14', GETDATE(), 'cashier02', 14, 2, 88000.00, 8),
    ('HD-20260830-15', GETDATE(), 'cashier02', 15, 1, 70000.00, 7);

    -- Nạp OrderDetails mẫu liên kết với 15 đơn hàng
    INSERT INTO OrderDetails (OrderId, ProductId, Quantity, UnitPrice, LineTotal)
    VALUES 
    (1, 1, 5, 16000, 80000),
    (1, 2, 10, 11000, 110000),
    (2, 6, 4, 15000, 60000),
    (3, 11, 2, 38000, 76000),
    (4, 5, 20, 4500, 90000),
    (5, 7, 3, 18000, 54000),
    (6, 8, 4, 19000, 76000),
    (7, 3, 10, 6000, 60000),
    (8, 4, 8, 10000, 80000),
    (9, 10, 5, 14000, 70000),
    (10, 12, 3, 15000, 45000),
    (11, 13, 10, 5000, 50000),
    (12, 14, 4, 12000, 48000),
    (13, 15, 2, 20000, 40000),
    (14, 9, 2, 22000, 44000),
    (15, 1, 2, 16000, 32000);

    PRINT '-> Đã nạp thành công 15 đơn hàng mẫu và chi tiết vào CSDL.';
END
GO

-- 4. TIẾT 3: STORED PROCEDURE TỔNG HỢP DOANH THU THEO THU NGÂN
IF OBJECT_ID('sp_GetRevenueByCashier', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetRevenueByCashier;
GO

CREATE PROCEDURE sp_GetRevenueByCashier
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        CashierUsername,
        COUNT(OrderId) AS TotalOrders,
        SUM(FinalAmount) AS TotalRevenue,
        SUM(RewardPoints) AS TotalPointsAwarded
    FROM Orders
    GROUP BY CashierUsername
    ORDER BY TotalRevenue DESC;
END
GO
PRINT '-> Đã tạo Stored Procedure sp_GetRevenueByCashier.';
GO

-- 5. BÀI TẬP 3.1: STORED PROCEDURE BÁO CÁO THEO KHOẢNG NGÀY
IF OBJECT_ID('sp_GetRevenueByDateRange', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetRevenueByDateRange;
GO

CREATE PROCEDURE sp_GetRevenueByDateRange
    @FromDate DATETIME,
    @ToDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        CashierUsername,
        COUNT(OrderId) AS TotalOrders,
        SUM(FinalAmount) AS TotalRevenue
    FROM Orders
    WHERE CreatedDate >= @FromDate AND CreatedDate <= @ToDate
    GROUP BY CashierUsername
    ORDER BY TotalRevenue DESC;
END
GO
PRINT '-> Đã tạo Stored Procedure sp_GetRevenueByDateRange.';
GO

-- 6. BÀI TẬP 3.2: STORED PROCEDURE TOP SẢN PHẨM BÁN CHẠY NHẤT
IF OBJECT_ID('sp_GetTop5SellingProducts', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetTop5SellingProducts;
GO

CREATE PROCEDURE sp_GetTop5SellingProducts
    @Top INT = 5
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@Top)
        p.ProductId,
        p.ProductName,
        SUM(od.Quantity) AS TotalQuantitySold,
        SUM(od.LineTotal) AS TotalSalesAmount
    FROM OrderDetails od
    INNER JOIN Products p ON od.ProductId = p.ProductId
    GROUP BY p.ProductId, p.ProductName
    ORDER BY TotalQuantitySold DESC;
END
GO
PRINT '-> Đã tạo Stored Procedure sp_GetTop5SellingProducts.';
GO
