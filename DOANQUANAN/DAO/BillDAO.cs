using DOANQUANAN.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOANQUANAN.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
            private set { instance = value; }
        }

        private BillDAO() { }

        public int GetBillIDByTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Bill WHERE idTable = " + id + " AND stathanhtoan = 0");
            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;
            }
            return -1;
        }
        public void CheckOut(int id, int discount, float tongtien)
        {
            string query = "UPDATE dbo.Bill SET DateCheckOut = GETDATE() , stathanhtoan = 1, " + "discount = " + discount + ", totalPrice = " + tongtien + "WHERE id = " + id;
            DataProvider.Instance.ExecuteNonQuery(query);
        }







        public void InsertBill(int id)
        {
            DataProvider.Instance.ExecuteNonQuery("EXEC InserBill @idTable", new object[] { id });
        }

        public DataTable GetBillListByDate(DateTime checkin, DateTime checkout)
        {
            return DataProvider.Instance.ExecuteQuery("EXEC GetBillDate @checkIN , @checkOut", new object[] { checkin, checkout });

        }
        public DataTable GetBillListByDateAndPage(DateTime checkin, DateTime checkout, int PageNumber)
        {
            return DataProvider.Instance.ExecuteQuery("EXEC GetListBillByDateAndPage @checkIN , @checkOut , @page", new object[] { checkin, checkout, PageNumber });

        }

        public int GetNumberBill(DateTime checkin, DateTime checkout)
        {
            return (int)DataProvider.Instance.ExecuteScalar("EXEC GetNumBillByDate @checkIN , @checkOut", new object[] { checkin, checkout });

        }



        public int GetMaxBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("SELECT MAX(id) FROM dbo.Bill");
            }
            catch
            {
                return 1;
            }
        }

        public void Deletebill(int id)
        {
            DataProvider.Instance.ExecuteNonQuery("EXEC DeleteBill @idTable", new object[] { id });
        }
    }
}
