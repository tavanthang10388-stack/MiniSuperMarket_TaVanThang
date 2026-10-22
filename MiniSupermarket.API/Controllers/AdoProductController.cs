/*
 * Ho ten sinh vien: Ta Van Thang
 * Mon hoc: Phat trien Ung dung Doanh nghiep voi .NET Core & SQL Server
 * Mo ta: Controller ADO.NET thuan - Tra cuu ma vach sieu toc va demo DataTable
 */
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MiniSupermarket.API.Models;
using System.Data;

namespace MiniSupermarket.API.Controllers
{

    [Route("api/ado-products")]
    [ApiController]
    public class AdoProductController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        // Constructor tiêm IConfiguration de doc chuoi ket noi tu appsettings.json
        public AdoProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // =========================================================================
        // THAO TÁC 1: TRA CỨU THEO MÃ VẠCH (CONNECTED MODE - SQLDATAREADER)
        // =========================================================================
        // GET: api/ado-products/barcode/893500123001
        [HttpGet("barcode/{barcode}")]
        public IActionResult GetProductByBarcode(string barcode)
        {
            // 1. Kiem tra tinh hop le cua tham so dau vao
            if (string.IsNullOrWhiteSpace(barcode))
            {
                return BadRequest(new { message = "Mã vạch không được để trống!" });
            }

            // 2. Lay chuoi ket noi toi SQL Server
            string connectionString = _configuration.GetConnectionString("DefaultConnection")!;

            // 3. Su dung khoi using de dam bao SqlConnection luon duoc dong va giai phong
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // 4. Cau lenh SELECT co tham so hoa @Barcode chong SQL Injection
                string query = @"SELECT ProductId, Barcode, ProductName, Price, StockQuantity 
                                 FROM Products 
                                 WHERE Barcode = @Barcode";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // 5. Gan gia tri an toan qua SqlParameter
                    command.Parameters.AddWithValue("@Barcode", barcode.Trim());

                    try
                    {
                        // 6. Mo ket noi vat ly toi SQL Server
                        connection.Open();

                        // 7. Khoi tao luong doc nhanh Forward-Only SqlDataReader
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // 8. Doc ban ghi: reader.Read() tra ve true neu tim thay dong du lieu
                            if (reader.Read())
                            {
                                var productDto = new ProductLookupDto
                                {
                                    ProductId = Convert.ToInt32(reader["ProductId"]),
                                    Barcode = reader["Barcode"].ToString() ?? string.Empty,
                                    ProductName = reader["ProductName"].ToString() ?? string.Empty,
                                    Price = Convert.ToDecimal(reader["Price"]),
                                    StockQuantity = Convert.ToInt32(reader["StockQuantity"])
                                };

                                // Tra ve ma 200 OK kem du lieu san pham
                                return Ok(productDto);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        // Bat loi neu ket noi CSDL gap su co
                        return StatusCode(500, new { message = "Lỗi truy vấn SQL: " + ex.Message });
                    }
                }
            } // Ket thuc using, connection.Close() tu dong duoc thuc thi tai day

            // 9. Neu khong tim thay san pham nao khop ma vach -> Tra ve 404 Not Found
            return NotFound(new { message = $"Không tìm thấy sản phẩm có mã vạch: {barcode}" });
        }

        // =========================================================================
        // THAO TÁC 2: TẢI DANH SÁCH LƯU ĐỆM (DISCONNECTED MODE - DATATABLE & ADAPTER)
        // =========================================================================
        // GET: api/ado-products/disconnected-cache
        [HttpGet("disconnected-cache")]
        public IActionResult GetProductsDisconnected()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection")!;

            // 1. Khoi tao bang ao DataTable tren RAM
            DataTable productTable = new DataTable("CachedProducts");

            // 2. Mo ket noi va su dung SqlDataAdapter de nap du lieu
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT ProductId, Barcode, ProductName, Price, StockQuantity 
                                 FROM Products 
                                 ORDER BY ProductId ASC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // 3. SqlDataAdapter lam cau noi giua Command va DataTable
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        // 4. Phuong thuc Fill(): Tu dong Open connection -> Doc du lieu do vao DataTable -> Tu dong Close connection
                        adapter.Fill(productTable);
                    }
                }
            } // Tai day, ket noi CSDL DA DUOC DONG HOAN TOAN!

            // 5. Thao tac doc lap tren bo nho RAM (Khong can ket noi toi SQL Server nua)
            var resultList = new List<ProductLookupDto>();

            // Duyet qua tung dong (DataRow) trong tap hop Rows cua DataTable
            foreach (DataRow row in productTable.Rows)
            {
                resultList.Add(new ProductLookupDto
                {
                    ProductId = Convert.ToInt32(row["ProductId"]),
                    Barcode = row["Barcode"].ToString() ?? string.Empty,
                    ProductName = row["ProductName"].ToString() ?? string.Empty,
                    Price = Convert.ToDecimal(row["Price"]),
                    StockQuantity = Convert.ToInt32(row["StockQuantity"])
                });
            }

            // Tra ve danh sach san pham da duoc luu dem tren RAM
            return Ok(new
            {
                totalLoaded = productTable.Rows.Count,
                cachedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                data = resultList
            });
        }

        // =========================================================================
        // BÀI TẬP 2.1: TRA CỨU THEO KHOẢNG TỒN KHO (SQLDATAREADER CẢNH BÁO NHẬP HÀNG)
        // =========================================================================
        // GET: api/ado-products/filter-stock?minStock=50
        [HttpGet("filter-stock")]
        public IActionResult GetProductsByMinStock([FromQuery] int minStock = 50)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection")!;
            var results = new List<ProductLookupDto>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT ProductId, Barcode, ProductName, Price, StockQuantity 
                                 FROM Products 
                                 WHERE StockQuantity <= @MinStock 
                                 ORDER BY StockQuantity ASC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MinStock", minStock);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new ProductLookupDto
                                {
                                    ProductId = Convert.ToInt32(reader["ProductId"]),
                                    Barcode = reader["Barcode"].ToString() ?? string.Empty,
                                    ProductName = reader["ProductName"].ToString() ?? string.Empty,
                                    Price = Convert.ToDecimal(reader["Price"]),
                                    StockQuantity = Convert.ToInt32(reader["StockQuantity"])
                                });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        return StatusCode(500, new { message = "Lỗi truy vấn SQL: " + ex.Message });
                    }
                }
            }

            return Ok(new
            {
                threshold = minStock,
                totalFound = results.Count,
                data = results
            });
        }

        // =========================================================================
        // BÀI TẬP 2.2: ĐẾM SỐ LƯỢNG SẢN PHẨM THEO NHÓM DANH MỤC VỚI DATATABLE
        // =========================================================================
        // GET: api/ado-products/category-counts
        [HttpGet("category-counts")]
        public IActionResult GetProductCountByCategory()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection")!;
            DataTable countTable = new DataTable("CategoryCounts");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT CategoryId, COUNT(*) AS Total 
                                 FROM Products 
                                 GROUP BY CategoryId 
                                 ORDER BY CategoryId ASC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(countTable);
                    }
                }
            }

            var result = new List<CategoryProductCountDto>();
            foreach (DataRow row in countTable.Rows)
            {
                result.Add(new CategoryProductCountDto
                {
                    CategoryId = Convert.ToInt32(row["CategoryId"]),
                    TotalProducts = Convert.ToInt32(row["Total"])
                });
            }

            return Ok(new
            {
                totalCategories = countTable.Rows.Count,
                data = result
            });
        }
    }
}
