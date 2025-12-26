using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
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
            //lấy thời gian hiện tại theo hệ thống
            DateTime gioht = DateTime.Now;
            //hiển thị thời gian lên thanh tiêu đề của form
            this.Text = gioht.ToString();

        }
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
            catch
            {
                lblStatus.Text = "Status: Unknown (Need Admin)";
                lblStatus.ForeColor = Color.Gray;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CheckStatus();  
            //tiêu đề của form khi load ở label1
            lbltitle.Text= "VerboseTool - Active Detail Notify";
        }

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
