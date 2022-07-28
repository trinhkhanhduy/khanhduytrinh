using DOANQUANAN.DAO;
using DOANQUANAN.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DOANQUANAN.AccountProfile;

namespace DOANQUANAN
{
    public partial class TableManager : Form


    {
        

        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; changeAccount(loginAccount.Type); }
        }

        public TableManager(Account acc)
        {
            InitializeComponent();

            this.LoginAccount = acc;
            LoadTable();
            LoadCategory();
            loadComboboxTable(cbChuyenBan);

        }

        void changeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += "(" + LoginAccount.DisplayName + ")";
        }

        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbDanhMuc.DataSource = listCategory;
            cbDanhMuc.DisplayMember = "Name";
        }

        void loadFoodListCateID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }

        void LoadTable()
        {
            flTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();

            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Trangthai;
                btn.Click += button1_Click;
                btn.Tag = item;
                switch (item.Trangthai)
                {
                    case "Trống":
                        btn.BackColor = Color.Aqua;
                        break;
                    default:
                        btn.BackColor = Color.GreenYellow;
                        break;
                }
                flTable.Controls.Add(btn);

            }
        }
        void ShowBill(int id)

        {
            lstBill.Items.Clear();

            List<Menu0> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);

            float totalPrice = 0;
            foreach (Menu0 item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString("c"));
                lsvItem.SubItems.Add(item.TotalPrice.ToString("c"));
                totalPrice += item.TotalPrice;
                lstBill.Items.Add(lsvItem);

            }
            CultureInfo culture = new CultureInfo("vi-VN");
            txtTongtien.Text = totalPrice.ToString("c");

        }

        void loadComboboxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int TableID = ((sender as Button).Tag as Table).ID;
            lstBill.Tag = (sender as Button).Tag;
            ShowBill(TableID);
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AccountProfile f = new AccountProfile(loginAccount);
            f.UpdateAccount += F_UpdateAccount;
            f.ShowDialog();
        }

        private void F_UpdateAccount(object sender, AccountEvent e)
        {

            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản(" + e.Acc.DisplayName + ")";
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Admin f = new Admin();
            f.loginAccount = LoginAccount;
            f.InsertFood += F_InsertFood;
            f.DeleteFood += F_DeleteFood;
            f.UpdateFood += F_UpdateFood;
            f.InsertTable += F_InsertTable;
            f.XoaTable += F_XoaTable;
            f.ShowDialog();
        }

        private void F_XoaTable(object sender, EventArgs e)
        {
            LoadTable();
            
        }

        private void F_InsertTable(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void F_UpdateFood(object sender, EventArgs e)
        {
            loadFoodListCateID((cbDanhMuc.SelectedItem as Category).ID);
            if (lstBill.Tag != null)
                ShowBill((lstBill.Tag as Table).ID);
        }

        private void F_DeleteFood(object sender, EventArgs e)
        {
            loadFoodListCateID((cbDanhMuc.SelectedItem as Category).ID);
            if (lstBill.Tag != null)
                ShowBill((lstBill.Tag as Table).ID);
            LoadTable();
        }

        private void F_InsertFood(object sender, EventArgs e)
        {
            loadFoodListCateID((cbDanhMuc.SelectedItem as Category).ID);
            if (lstBill.Tag != null)
                ShowBill((lstBill.Tag as Table).ID);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void lstBill_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedItem == null)
                return;

            Category select = cb.SelectedItem as Category;

            id = select.ID;
            loadFoodListCateID(id);
        }

        private void btnAddFodd_Click(object sender, EventArgs e)
        {
            Table table = lstBill.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn ăn");
                return;
            }

            int idBill = BillDAO.Instance.GetBillIDByTableID(table.ID);
            int foodID = (cbFood.SelectedItem as Food).ID;

            int count = (int)nCountFood.Value;

            if (idBill == -1)
            {
                
                BillDAO.Instance.InsertBill(table.ID);
                BillinfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxBill(), foodID, count);
                
 
            }


            else
            {
                BillinfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
                
                this.nCountFood.Value = 1;


            }

            ShowBill(table.ID);
            LoadTable();
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {

            Table table = lstBill.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn ăn");
                return;
            }
            int idBill = BillDAO.Instance.GetBillIDByTableID(table.ID);
            int discount = (int)nGiamGia.Value;
            double totalPrice = Convert.ToDouble(txtTongtien.Text.Split(',')[0]);
            double finaltotalprice = totalPrice - (totalPrice / 100) * discount;

            double totaldiscount = (totalPrice / 100) * discount;



            if (idBill != -1)
            {
                if (MessageBox.Show(String.Format("Bạn có chắc thanh toán hóa đơn {0}\n Giá : {1}\n Khuyến mãi : {2} %  =  {3}\n Thành tiền : {4}", table.Name, totalPrice, discount, totaldiscount, finaltotalprice), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, discount, (float)finaltotalprice);
                    ShowBill(table.ID);
                    LoadTable();
                }
            }






        }

        private void btnChuyenBan_Click(object sender, EventArgs e)
        {
            int id1 = (lstBill.Tag as Table).ID;
            int id2 = (cbChuyenBan.SelectedItem as Table).ID;
            if (MessageBox.Show(String.Format("Bạn có muốn chuyển bàn {0} qua bàn {1}", (lstBill.Tag as Table).Name, (cbChuyenBan.SelectedItem as Table).Name), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {

                TableDAO.Instance.chuyenban(id1, id2);
                LoadTable();
            }
        }

       
        private void thanhToánToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnThanhToan_Click(this, new EventArgs());
        }

        private void thêmMónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnAddFodd_Click(this, new EventArgs());
        }

        private void btnGiamGia_Click(object sender, EventArgs e)
        {

        }

        private void btnReLoadBan_Click(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void nCountFood_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtTongtien_TextChanged(object sender, EventArgs e)
        {

        }

        private void nGiamGia_ValueChanged(object sender, EventArgs e)
        {

        }

     
    }
}
