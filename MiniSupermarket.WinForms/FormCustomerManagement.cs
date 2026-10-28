using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormCustomerManagement : Form
    {
        private List<CustomerDto> _allCustomers = new();

        public FormCustomerManagement()
        {
            InitializeComponent();
            dgvCustomers.AutoGenerateColumns = false;
        }

        private HttpClient GetAuthenticatedClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/api/")
            };

            if (!string.IsNullOrEmpty(SessionManager.JwtToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionManager.JwtToken);
            }
            return client;
        }

        private async void FormCustomerManagement_Load(object sender, EventArgs e)
        {
            if (cboFilterRank.Items.Count > 0 && cboFilterRank.SelectedIndex == -1)
            {
                cboFilterRank.SelectedIndex = 0; // "Tất cả hạng"
            }

            if (cbMembershipRank.Items.Count > 0 && cbMembershipRank.SelectedIndex == -1)
            {
                cbMembershipRank.SelectedIndex = 0;
            }

            if (!string.IsNullOrEmpty(SessionManager.CurrentRole))
            {
                lblReady.Text = $"Sẵn sàng | Quyền: {SessionManager.CurrentRole} | Phân hệ Khách hàng (EF Core)";
            }

            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                using var client = GetAuthenticatedClient();
                var response = await client.GetAsync("customers");
                if (response.IsSuccessStatusCode)
                {
                    _allCustomers = await response.Content.ReadFromJsonAsync<List<CustomerDto>>() ?? new List<CustomerDto>();
                    ApplyFilter();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Phiên làm việc hết hạn hoặc chưa đăng nhập!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Không thể tải danh sách khách hàng từ máy chủ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối API: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilter()
        {
            string keyword = txtKeyword.Text.Trim().ToLower();
            string selectedRank = cboFilterRank.SelectedItem?.ToString() ?? "Tất cả hạng";

            var filtered = _allCustomers.AsEnumerable();

            if (!string.IsNullOrEmpty(keyword))
            {
                filtered = filtered.Where(c => 
                    (!string.IsNullOrEmpty(c.CustomerName) && c.CustomerName.ToLower().Contains(keyword)) ||
                    (!string.IsNullOrEmpty(c.PhoneNumber) && c.PhoneNumber.Contains(keyword)) ||
                    (!string.IsNullOrEmpty(c.Address) && c.Address.ToLower().Contains(keyword)) ||
                    c.CustomerId.ToString() == keyword);
            }

            if (selectedRank != "Tất cả hạng" && !string.IsNullOrEmpty(selectedRank))
            {
                filtered = filtered.Where(c => string.Equals(c.MembershipRank, selectedRank, StringComparison.OrdinalIgnoreCase));
            }

            var list = filtered.ToList();
            dgvCustomers.DataSource = list;
            lblCustomerCount.Text = $"Tổng số: {list.Count} khách hàng";
        }

        private void cboFilterRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void txtKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ApplyFilter();
            }
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCustomers.Rows[e.RowIndex].DataBoundItem is CustomerDto customer)
            {
                txtCustomerId.Text = customer.CustomerId.ToString();
                txtCustomerName.Text = customer.CustomerName;
                txtPhoneNumber.Text = customer.PhoneNumber;
                txtAddress.Text = customer.Address ?? string.Empty;
                txtRewardPoints.Text = customer.RewardPoints.ToString();
                
                int rankIndex = cbMembershipRank.FindStringExact(customer.MembershipRank ?? "Chuẩn");
                cbMembershipRank.SelectedIndex = rankIndex >= 0 ? rankIndex : 0;
            }
        }

        private void dgvCustomers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null || e.CellStyle == null) return;

            // Định dạng hiển thị điểm tích lũy có dấu phân cách hàng nghìn
            if (dgvCustomers.Columns[e.ColumnIndex].Name == "colPoints" && e.Value is int points)
            {
                e.Value = points.ToString("N0");
                e.FormattingApplied = true;
            }
            // Định dạng trực quan thẻ thành viên với màu sắc & biểu tượng
            else if (dgvCustomers.Columns[e.ColumnIndex].Name == "colRank")
            {
                string rank = e.Value?.ToString() ?? "";
                switch (rank.Trim())
                {
                    case "Kim cương":
                        e.Value = "💎 Kim cương";
                        e.CellStyle.ForeColor = Color.FromArgb(102, 16, 242);
                        e.CellStyle.Font = new Font(dgvCustomers.Font, FontStyle.Bold);
                        e.FormattingApplied = true;
                        break;
                    case "Vàng":
                        e.Value = "⭐ Vàng";
                        e.CellStyle.ForeColor = Color.FromArgb(217, 119, 6);
                        e.CellStyle.Font = new Font(dgvCustomers.Font, FontStyle.Bold);
                        e.FormattingApplied = true;
                        break;
                    case "Bạc":
                        e.Value = "🥈 Bạc";
                        e.CellStyle.ForeColor = Color.FromArgb(75, 85, 99);
                        e.CellStyle.Font = new Font(dgvCustomers.Font, FontStyle.Bold);
                        e.FormattingApplied = true;
                        break;
                    case "Chuẩn":
                        e.Value = "🏷️ Chuẩn";
                        e.CellStyle.ForeColor = Color.FromArgb(16, 120, 60);
                        e.FormattingApplied = true;
                        break;
                }
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (SessionManager.CurrentRole.Equals("Cashier", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Nhân viên Thu ngân không có quyền thêm mới khách hàng! Vui lòng liên hệ Admin.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = txtCustomerName.Text.Trim();
            string phone = txtPhoneNumber.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCustomerName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhoneNumber.Focus();
                return;
            }

            int points = 0;
            int.TryParse(txtRewardPoints.Text.Trim(), out points);

            var newCustomer = new CustomerDto
            {
                CustomerName = name,
                PhoneNumber = phone,
                Address = txtAddress.Text.Trim(),
                RewardPoints = points,
                MembershipRank = cbMembershipRank.SelectedItem?.ToString() ?? "Chuẩn"
            };

            try
            {
                using var client = GetAuthenticatedClient();
                var response = await client.PostAsJsonAsync("customers", newCustomer);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Thêm mới khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadDataAsync();
                    ClearInputs();
                }
                else
                {
                    var err = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Thêm khách hàng thất bại: {err}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (SessionManager.CurrentRole.Equals("Cashier", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Nhân viên Thu ngân không có quyền chỉnh sửa thông tin khách hàng! Vui lòng liên hệ Admin.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtCustomerId.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng từ danh sách để cập nhật!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = int.Parse(txtCustomerId.Text);
            string name = txtCustomerName.Text.Trim();
            string phone = txtPhoneNumber.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Tên khách hàng không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Số điện thoại không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int points = 0;
            int.TryParse(txtRewardPoints.Text.Trim(), out points);

            var updatedCustomer = new CustomerDto
            {
                CustomerId = id,
                CustomerName = name,
                PhoneNumber = phone,
                Address = txtAddress.Text.Trim(),
                RewardPoints = points,
                MembershipRank = cbMembershipRank.SelectedItem?.ToString() ?? "Chuẩn"
            };

            try
            {
                using var client = GetAuthenticatedClient();
                var response = await client.PutAsJsonAsync($"customers/{id}", updatedCustomer);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Cập nhật thông tin khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadDataAsync();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (SessionManager.CurrentRole.Equals("Cashier", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Nhân viên Thu ngân không có quyền xóa dữ liệu khách hàng!", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtCustomerId.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = int.Parse(txtCustomerId.Text);
            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng ID = {id}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using var client = GetAuthenticatedClient();
                    var response = await client.DeleteAsync($"customers/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadDataAsync();
                        ClearInputs();
                    }
                    else
                    {
                        MessageBox.Show("Xóa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtKeyword.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                await LoadDataAsync();
                return;
            }

            try
            {
                using var client = GetAuthenticatedClient();
                var response = await client.GetAsync($"customers/search?keyword={Uri.EscapeDataString(keyword)}");
                if (response.IsSuccessStatusCode)
                {
                    _allCustomers = await response.Content.ReadFromJsonAsync<List<CustomerDto>>() ?? new List<CustomerDto>();
                    ApplyFilter();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy khách hàng phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            txtKeyword.Clear();
            if (cboFilterRank.Items.Count > 0)
            {
                cboFilterRank.SelectedIndex = 0;
            }
            await LoadDataAsync();
            ClearInputs();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void ClearInputs()
        {
            txtCustomerId.Clear();
            txtCustomerName.Clear();
            txtPhoneNumber.Clear();
            txtAddress.Clear();
            txtRewardPoints.Text = "0";
            if (cbMembershipRank.Items.Count > 0)
            {
                cbMembershipRank.SelectedIndex = 0;
            }
            txtCustomerName.Focus();
        }
    }

    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Address { get; set; }
        public int RewardPoints { get; set; }
        public string MembershipRank { get; set; } = "Chuẩn";
    }
}
