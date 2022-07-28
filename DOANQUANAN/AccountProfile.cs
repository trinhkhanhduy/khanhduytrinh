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
    public partial class AccountProfile : Form
    {
        private Account loginAccount;
        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; changeAccount(loginAccount); }
        }

        public AccountProfile(Account acc)
        {
            InitializeComponent();
            LoginAccount = acc;
        }
        void changeAccount(Account acc)
        {
            txtDangNhap.Text = loginAccount.UserName;
            txtTenHienThi.Text = loginAccount.DisplayName;

        }

        void updateAccountInfo()
        {
            string displayName = txtTenHienThi.Text;
            string pass = txtPass.Text;
            string newpass = txtNewPass.Text;
            string repass = txtReNewPass.Text;
            string username = txtDangNhap.Text;

            if (!newpass.Equals(repass))
            {
                MessageBox.Show("Vui lòng nhập lại đúng mật khẩu mới!");
            }
            else 
            {
                if (MessageBox.Show("Bạn có chắc cập nhập lại mật khẩu không","Thông báo",MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (AccountDAO.Instance.UpdateAccount(username, displayName, pass, newpass)){

                        MessageBox.Show("Cập nhật thành công");
                        if (updateAccount != null)
                        {
                            updateAccount(this, new AccountEvent(AccountDAO.Instance.GetAccount(username)));
                            this.txtPass.ResetText();
                            this.txtNewPass.ResetText();
                            this.txtReNewPass.ResetText();
                        } 
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng nhập đúng mật khẩu!");
                    }
                }
                else
                {
                    MessageBox.Show("Cập nhật mật khẩu thất bại");
                }

            }
        }

        private event EventHandler<AccountEvent> updateAccount;
        public event EventHandler<AccountEvent> UpdateAccount
        {
            add { updateAccount += value; }
            remove { updateAccount -= value; }
        }




        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            updateAccountInfo();
        }

        public class AccountEvent : EventArgs
        {
            private Account acc;

            public Account Acc { get => acc; set => acc = value; }

            public AccountEvent(Account acc)
            {
                this.Acc = acc;
            }
        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void AccountProfile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnCapNhat_Click(sender, e);
            }
        }

        private void AccountProfile_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }
    }
}
