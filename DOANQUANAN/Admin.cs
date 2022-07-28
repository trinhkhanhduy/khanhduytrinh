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
    public partial class Admin : Form
    {
        BindingSource foodlist = new BindingSource();
        BindingSource Accountlist = new BindingSource();
        BindingSource TableBan = new BindingSource();

        public Account loginAccount;
        public Admin()
        {
            InitializeComponent();
            Load();



        }


        void Load()
        {
            dtgvTk.DataSource = Accountlist;
            dtgvFood.DataSource = foodlist;
            dtgvsoban.DataSource = TableBan;
            LoadDateMonth();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListFood();
            LoadListTable();
            loadaccount();
            AddFood();
            LoadCategoryIntoCb(cbFoodCategory);
            AddAcountBinding();
            AccTableBindings();
        }
        void AddAcountBinding()
        {
            txtUserTk.DataBindings.Add(new Binding("Text", dtgvTk.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txtDisplayNameTk.DataBindings.Add(new Binding("Text", dtgvTk.DataSource, "displayName", true, DataSourceUpdateMode.Never));
            nmKieu.DataBindings.Add(new Binding("Value", dtgvTk.DataSource, "kieu", true, DataSourceUpdateMode.Never));
        }

        void loadaccount()
        {
            Accountlist.DataSource = AccountDAO.Instance.GetListAccount();
        }
        void acccount(string username, string displayname, int kieu)
        {
            if (MessageBox.Show("Bạn có muốn thêm tài khoản không", "Thông báo", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                AccountDAO.Instance.Insertacc(username, displayname, kieu);
                MessageBox.Show("Thêm tài khoản thành công");

            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }
            loadaccount();
        }


        void Editacount(string username, string displayname, int kieu)
        {
            if (MessageBox.Show("Bạn có muốn sửa tài khoản không", "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                AccountDAO.Instance.Updateacc(username, displayname, kieu);
                MessageBox.Show("Sửa tài khoản thành công");

            }
            else
            {
                MessageBox.Show("Sửa tài khoản thất bại");
            }
            loadaccount();
        }

        void Xoaacccount(string username)
        {
            if (loginAccount.UserName.Equals(username))
            {
                MessageBox.Show("Không thể xóa chính bạn");
                return;
            }
            if (MessageBox.Show("Bạn có muốn xóa tài khoản không", "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                AccountDAO.Instance.Deleteacc(username);
                MessageBox.Show("Xóa tài khoản thành công");

            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }
            loadaccount();
        }
        void resetPass(string name)
        {
            if (MessageBox.Show("Bạn có muốn đặt lại mật khẩu không", "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                AccountDAO.Instance.Resetpass(name);
                MessageBox.Show("Đặt lại mật khẩu thành công");

            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
        }

        void AddFood()
        {
            txtFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txtID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));

        }
        void LoadCategoryIntoCb(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }

        List<Food> SearchFood(string name)
        {
            List<Food> listfood = FoodDAO.Instance.searchFood(name);

            return listfood;
        }

        void LoadListFood()
        {
            foodlist.DataSource = FoodDAO.Instance.GetListFood();
        }
       
        void LoadDateMonth()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }

        private void tbnView_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["categoryID"].Value;

                    Category category = CategoryDAO.Instance.GetCategorybyID(id);
                    cbFoodCategory.SelectedItem = category;
                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbFoodCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {


                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbFoodCategory.SelectedIndex = index;

                }
            }
            catch { }


        }
        


        private void btnXemFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private void btnThemFood_Click(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int IDcate = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;

            if (MessageBox.Show("Bạn có muốn thêm món không", "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                FoodDAO.Instance.InsertFood(name, IDcate, price);
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Thêm thất bại");
            }
        }

        private void btnSuaFood_Click(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int IDcate = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(txtID.Text);
            if (MessageBox.Show("Bạn có muốn sửa món không", "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                FoodDAO.Instance.UpdateFood(id, name, IDcate, price);
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Sửa thất bại");
            }
        }

        private void btnXoaFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtID.Text);
            if (MessageBox.Show("Bạn có muốn xóa món không", "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                FoodDAO.Instance.DeleteFood(id);
                MessageBox.Show("Bạn đã xóa món thành công");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Xóa thất bại");
            }
        }
        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private void btnTimFood_Click(object sender, EventArgs e)
        {
            foodlist.DataSource = SearchFood(txtSearchFoodName.Text);
        }

        private void btnXemTk_Click(object sender, EventArgs e)
        {
            loadaccount();
        }

        private void btnThemTk_Click(object sender, EventArgs e)
        {
            string username = txtUserTk.Text;
            string displayname = txtDisplayNameTk.Text;
            int kieu = (int)nmKieu.Value;

            acccount(username, displayname, kieu);

        }

        private void btnSuaTk_Click(object sender, EventArgs e)
        {
            string username = txtUserTk.Text;
            string displayname = txtDisplayNameTk.Text;
            int kieu = (int)nmKieu.Value;
            Editacount(username, displayname, kieu);

        }

        private void btnXoaTk_Click(object sender, EventArgs e)
        {
            string username = txtUserTk.Text;

            Xoaacccount(username);
        }

        private void btnResetPass_Click(object sender, EventArgs e)
        {
            string username = txtUserTk.Text;
            resetPass(username);
        }

        private void btnDau_Click(object sender, EventArgs e)
        {
            txtTrang.Text = "1";
        }

        private void btnCuoi_Click(object sender, EventArgs e)
        {
            int sum = BillDAO.Instance.GetNumberBill(dtpkFromDate.Value, dtpkToDate.Value);
            int lastpage = sum / 10;

            if (sum % 10 != 0)
            {
                lastpage++;
            }
            txtTrang.Text = lastpage.ToString();
        }

        private void txtTrang_TextChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txtTrang.Text));
        }

        private void btnke_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txtTrang.Text);
            int sum = BillDAO.Instance.GetNumberBill(dtpkFromDate.Value, dtpkToDate.Value);
            if (page < sum)
            {
                page++;

                txtTrang.Text = page.ToString();
            }
        }

        private void btnLui_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txtTrang.Text);
            if (page > 1)
            {
                page--;
                txtTrang.Text = page.ToString();
            }
        }

        

        private void dtpkFromDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtpkToDate_ValueChanged(object sender, EventArgs e)
        {

        }
        void LoadListTable()
        {
            TableBan.DataSource = TableDAO.Instance.LoadTableList();
        }

        private void btnxemban_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }

        void AccTableBindings()
        {
            txtidban.DataBindings.Add(new Binding("Text", dtgvsoban.DataSource, "id",true,DataSourceUpdateMode.Never));
            txtnameban.DataBindings.Add(new Binding("Text", dtgvsoban.DataSource, "name", true, DataSourceUpdateMode.Never));
            txttrangthai.DataBindings.Add(new Binding("Text", dtgvsoban.DataSource, "trangthai", true, DataSourceUpdateMode.Never));

        }

        private void txtSearchFoodName_TextChanged(object sender, EventArgs e)
        {

        }

        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add { insertTable += value; }
            remove { insertTable -= value; }
        }
        private event EventHandler xoaTable;
        public event EventHandler XoaTable
        {
            add { xoaTable += value; }
            remove { xoaTable -= value; }
        }


        private void btnaddban_Click(object sender, EventArgs e)
        {
            
            string name = txtnameban.Text;
            string trangthai = txttrangthai.Text;
            if (MessageBox.Show("Bạn có muốn thêm bàn không", "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {


             TableDAO.Instance.InsertTable(name,trangthai);
           
    
             if (insertTable != null)
                    insertTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Thêm thất bại");
            }
            LoadListTable();

        }

        private void btnxoaban_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtidban.Text);
           
            if (MessageBox.Show("Bạn có muốn xóa bàn không", "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {

             TableDAO.Instance.XoaTable(id);
             
             if (xoaTable != null)
                  xoaTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Xóa thất bại");
            }
            LoadListTable();
        }
    }
}
