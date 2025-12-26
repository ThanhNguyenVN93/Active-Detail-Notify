using Microsoft.Win32;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace frmactivenotify
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblClock.Text = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
        }
        /// <summary>
        /// Đọc giá trị <c>VerboseStatus</c> từ registry và cập nhật
        /// trạng thái hiển thị trên <c>lblStatus</c> cùng text của <c>btnexcute</c>.
        /// </summary>
        private void CheckStatus()
        {
            string registryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
            string valueName = "VerboseStatus";

            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, false))
                {
                    // Kiểm tra xem Key và Giá trị có tồn tại không
                    object val = (key != null) ? key.GetValue(valueName) : null;

                    if (val != null && Convert.ToInt32(val) == 1)
                    {
                        lblStatus.Text = "Status: ENABLED (ON)";
                        lblStatus.ForeColor = Color.Green;
                        btnexcute.Text = "Disable Verbose"; // Đổi text nút luôn cho tiện
                    }
                    else
                    {
                        lblStatus.Text = "Status: DISABLED (OFF)";
                        lblStatus.ForeColor = Color.Red;
                        btnexcute.Text = "Enable Verbose";
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                lblStatus.Text = "Status: Unknown (Need Admin)";
                lblStatus.ForeColor = Color.Gray;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CheckStatus();
            lbltitle.Text = "VerboseTool - Active Detail Notify";
            this.Text = "VerboseTool";
        }

        /// <summary>
        /// Xử lý sự kiện khi người dùng nhấn nút thực thi.
        /// Đọc trạng thái hiện tại của registry value <c>VerboseStatus</c>
        /// dưới key <c>HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System</c>,
        /// sau đó đảo ngược giá trị (0 → 1 hoặc 1 → 0) và cập nhật lại giao diện.
        /// </summary>
        /// <remarks>
        /// Yêu cầu quyền Administrator để ghi vào registry HKLM.
        /// Thay đổi chỉ có hiệu lực sau khi khởi động lại máy tính.
        /// </remarks>
        /// <param name="sender">Đối tượng phát sinh sự kiện (nút <c>btnexcute</c>).</param>
        /// <param name="e">Thông tin sự kiện Click, không sử dụng trong phương thức này.</param>
        private void btnexcute_Click(object sender, EventArgs e)
        {
            string registryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
            string valueName = "VerboseStatus";

            try
            {
                // 1. Mở key với quyền ghi (true)
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, true))
                {
                    if (key != null)
                    {
                        // 2. Kiểm tra trạng thái hiện tại từ Registry
                        object val = key.GetValue(valueName);
                        int currentState = (val != null) ? Convert.ToInt32(val) : 0;

                        // 3. Đảo trạng thái (Nếu đang 1 thì về 0, nếu đang 0 thì lên 1)
                        int newState = (currentState == 1) ? 0 : 1;
                        key.SetValue(valueName, newState, RegistryValueKind.DWord);

                        // 4. Thông báo cho người dùng
                        string msg = (newState == 1)
                            ? "Verbose notification mode is ENABLED!"
                            : "Verbose notification mode is DISABLED!";

                        MessageBox.Show(msg + "\n\nPlease restart your computer to apply changes.",
                                        "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 5. Cập nhật lại giao diện (màu sắc, chữ) ngay lập tức
                        CheckStatus();
                    }
                    else
                    {
                        MessageBox.Show("Error: System Registry path not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Error: Administrator privileges are required to repair the system!", "Permissions", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
