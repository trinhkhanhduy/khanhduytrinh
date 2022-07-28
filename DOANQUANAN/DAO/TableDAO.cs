using DOANQUANAN.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOANQUANAN.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;

        public static TableDAO Instance
        {
            get { if (instance == null) instance = new TableDAO(); return TableDAO.instance; }
            private set { TableDAO.instance = value; }
        }

        public static int TableWidth = 90;
        public static int TableHeight = 90;
        private TableDAO() { }

        public void chuyenban(int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("ChuyenBan @idTable1 , @idTable2", new object[] { id1, id2 });
        }

        public List<Table> LoadTableList()
        {
            List<Table> list = new List<Table>();
            string query = "select * from TableFood";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                list.Add(table);
            }
            return list;

        }

        public bool InsertTable(string name, string trangthai = "Trống")
        {
            string query = string.Format("Insert dbo.TableFood (name, trangthai) Values (N'{0}', N'{1}')",name,trangthai);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        
        public bool XoaTable(int id)
        {
            BillDAO.Instance.Deletebill(id);
            string query = string.Format("Delete dbo.TableFood where id ={0}", id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
            
        }
    }
}
