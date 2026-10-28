/*
 * Ho ten sinh vien: Ta Van Thang
 * Mon hoc: Phat trien Ung dung Doanh nghiep voi .NET Core & SQL Server
 * Mo ta: Controller thu nghiem truc tiep cac bai tap Tiet 1 & Tiet 4 (ADO.NET Can ban & SqlTransaction Rollback)
 */
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/ado-diagnostics")]
    [ApiController]
    public class AdoDiagnosticsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AdoDiagnosticsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // =========================================================================
        // TIẾT 1 - BÀI TẬP 1: ĐẾM TỔNG SỐ LƯỢNG MẶT HÀNG BẰNG EXECUTESCALAR()
        // GET: api/ado-diagnostics/count-products
        // =========================================================================
        [HttpGet("count-products")]
        public IActionResult CountProductsScalar()
        {
            string connString = _configuration.GetConnectionString("DefaultConnection")!;

            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    string query = "SELECT COUNT(*) FROM Products";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        object result = command.ExecuteScalar()!;
                        int totalCount = Convert.ToInt32(result);

                        return Ok(new
                        {
                            method = "ExecuteScalar()",
                            description = "Đếm tổng số mặt hàng hiện có trong kho siêu thị",
                            totalProducts = totalCount,
                            status = "Thành công"
                        });
                    }
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { message = "Lỗi SQL: " + ex.Message });
                }
            }
        }

        // =========================================================================
        // TIẾT 1 - BÀI TẬP 2: CẬP NHẬT TỒN KHO AN TOÀN VỚI SQLPARAMETER & EXECUTENONQUERY()
        // PUT: api/ado-diagnostics/update-stock?productId=1&newStock=250
        // =========================================================================
        [HttpPut("update-stock")]
        public IActionResult UpdateStockParametrized([FromQuery] int productId = 1, [FromQuery] int newStock = 250)
        {
            string connString = _configuration.GetConnectionString("DefaultConnection")!;

            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    string updateSql = @"UPDATE Products 
                                         SET StockQuantity = @Stock 
                                         WHERE ProductId = @Id";

                    using (SqlCommand command = new SqlCommand(updateSql, connection))
                    {
                        command.Parameters.AddWithValue("@Stock", newStock);
                        command.Parameters.AddWithValue("@Id", productId);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return Ok(new
                            {
                                method = "ExecuteNonQuery()",
                                description = "Cập nhật tồn kho an toàn bằng SqlParameter chống SQL Injection",
                                productId = productId,
                                newStock = newStock,
                                rowsAffected = rowsAffected,
                                status = "Thành công"
                            });
                        }
                        return NotFound(new { message = $"Không tìm thấy sản phẩm có ID = {productId}" });
                    }
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { message = "Lỗi SQL: " + ex.Message });
                }
            }
        }

        // =========================================================================
        // TIẾT 1 - BÀI TẬP 3: DUYỆT DANH SÁCH BẰNG SQLDATAREADER
        // GET: api/ado-diagnostics/stream-products
        // =========================================================================
        [HttpGet("stream-products")]
        public IActionResult StreamProductsReader()
        {
            string connString = _configuration.GetConnectionString("DefaultConnection")!;
            var list = new List<object>();

            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    string query = @"SELECT ProductId, Barcode, ProductName, Price, StockQuantity 
                                     FROM Products 
                                     ORDER BY ProductId ASC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new
                                {
                                    productId = Convert.ToInt32(reader["ProductId"]),
                                    barcode = reader["Barcode"].ToString(),
                                    productName = reader["ProductName"].ToString(),
                                    price = Convert.ToDecimal(reader["Price"]),
                                    stockQuantity = Convert.ToInt32(reader["StockQuantity"])
                                });
                            }
                        }
                    }

                    return Ok(new
                    {
                        method = "ExecuteReader()",
                        description = "Luồng đọc một chiều (Forward-Only) tối ưu RAM",
                        totalRead = list.Count,
                        data = list
                    });
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { message = "Lỗi SQL: " + ex.Message });
                }
            }
        }

        // =========================================================================
        // TIẾT 4 - THỰC HÀNH MÔ PHỎNG CƠ CHẾ ROLLBACK CỦA SQLTRANSACTION KHI CÓ NGOẠI LỆ
        // POST: api/ado-diagnostics/simulate-transaction-rollback
        // =========================================================================
        [HttpPost("simulate-transaction-rollback")]
        public IActionResult SimulateTransactionRollback([FromQuery] bool simulateError = true)
        {
            string connString = _configuration.GetConnectionString("DefaultConnection")!;
            var logs = new List<string>();

            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    logs.Add("1. Bắt đầu phiên SqlTransaction bảo vệ ACID...");

                    // Bước 1: Trừ 5 đơn vị tồn kho của sản phẩm ID = 1
                    string sqlStep1 = "UPDATE Products SET StockQuantity = StockQuantity - @Qty WHERE ProductId = @Id";
                    using (SqlCommand cmd1 = new SqlCommand(sqlStep1, connection, transaction))
                    {
                        cmd1.Parameters.AddWithValue("@Qty", 5);
                        cmd1.Parameters.AddWithValue("@Id", 1);
                        int r1 = cmd1.ExecuteNonQuery();
                        logs.Add($"2. Bước 1 hoàn thành: Đã trừ 5 đơn vị SP ID=1 (Dòng tác động: {r1}).");
                    }

                    // Bước 2: Kiểm tra cờ giả lập lỗi
                    if (simulateError)
                    {
                        logs.Add("3. Bước 2: Phát hiện mặt hàng thứ 2 bị hết kho -> Ném lỗi InvalidOperationException.");
                        throw new InvalidOperationException("Mặt hàng thứ hai đã hết tồn kho thực tế!");
                    }

                    // Bước 3: Commit nếu không có lỗi
                    transaction.Commit();
                    logs.Add("4. GIAO DỊCH COMMIT THÀNH CÔNG (Dữ liệu đã ghi vĩnh viễn vào CSDL).");

                    return Ok(new
                    {
                        success = true,
                        committed = true,
                        logs = logs
                    });
                }
                catch (Exception ex)
                {
                    // Hoàn tác sạch sẽ
                    transaction.Rollback();
                    logs.Add($"[PHÁT SINH LỖI]: {ex.Message}");
                    logs.Add("-> [HỆ THỐNG]: Đã kích hoạt transaction.Rollback()!");
                    logs.Add("-> Toàn bộ thay đổi của Bước 1 đã được hoàn trả, tồn kho SP 1 KHÔNG hề bị trừ!");

                    return BadRequest(new
                    {
                        success = false,
                        committed = false,
                        message = "Giao dịch đã được Rollback an toàn theo chuẩn ACID!",
                        error = ex.Message,
                        auditLogs = logs
                    });
                }
            }
        }
    }
}
