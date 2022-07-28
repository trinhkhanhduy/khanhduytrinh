using DOANQUANAN.DAO;
using DOANQUANAN.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DOANQUANAN
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có thật sự muốn thoát không!", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }



        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string userName = txtDangNhap.Text;
            string password = txtPass.Text;
            if (Loginn(userName, password))
            {
                Account loginAccount = AccountDAO.Instance.GetAccount(userName);

                TableManager f = new TableManager(loginAccount);
                this.Hide();
                f.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Sai tên tài khoản hoặc mật khẩu!");
            }
        }
        bool Loginn(string userName, string password)
        {
            return AccountDAO.Instance.Loginn(userName, password);
        }

        private void txtDangNhap_TextChanged(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnDangNhap_Click(sender, e);
            }
           
        }
    }
}
