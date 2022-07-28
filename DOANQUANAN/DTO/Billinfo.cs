using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOANQUANAN.DTO
{
    public class Menu
    {
        public Menu(int id, int billID, int foodID, int count)
        {
            this.ID = id;
            this.BillID = billID;
            this.FoodID = foodID;
            this.Count = count;
        }

        public Menu(DataRow row)
        {
            this.ID = (int)row["id"];
            this.BillID = (int)row["idBill"];
            this.FoodID = (int)row["idFood"];
            this.Count = (int)row["soluong"];
        }


        private int iD;
        private int billID;
        private int foodID;
        private int count;

        public int ID { get { return iD; } set { iD = value; } }

        public int BillID { get { return billID; } set { billID = value; } }
        public int FoodID { get { return foodID; } set { foodID = value; } }
        public int Count { get { return count; } set { count = value; } }
    }
}
