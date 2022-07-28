using DOANQUANAN.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOANQUANAN.DAO
{
    public class MenuDAO
    {
        private static MenuDAO instance;

        public static MenuDAO Instance
        {
            get { if (instance == null) instance = new MenuDAO(); return MenuDAO.instance; }
            private set { MenuDAO.instance = value; }
        }

        private MenuDAO() { }

        public List<Menu0> GetListMenuByTable(int id)
        {
            List<Menu0> listMenu = new List<Menu0>();

            string query = "SELECT f.name,bi.soluong,f.price,f.price*bi.soluong AS totalPrice FROM dbo.BillInfo AS bi ,dbo.Bill AS b,dbo.Food AS f WHERE bi.idBill=b.id AND bi.idFood=f.id AND b.stathanhtoan=0 and b.idTable= " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Menu0 menu = new Menu0(item);
                listMenu.Add(menu);
            }

            return listMenu;
        }
    }
}
