using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOANQUANAN.DTO
{
    public class Bill
    {
        public Bill(int id, DateTime? dateCheckin, DateTime? dateCheckOut, int status, int discount = 0)
        {
            this.ID = id;
            this.DateCheckIn = dateCheckin;
            this.DateCheckOut = dateCheckOut;
            this.Status = status;
            this.Discount = discount;
        }

        public Bill(DataRow row)
        {
            this.ID = (int)row["id"];
            this.DateCheckIn = (DateTime?)row["DateCheckIn"];
            var dateCheckOutTemp = row["DateCheckOut"];
            if (dateCheckOutTemp.ToString() != "")
                this.DateCheckOut = (DateTime?)dateCheckOutTemp;



            this.Status = (int)row["stathanhtoan"];
            this.Discount = (int)row["discount"];

        }
        private int iD;
        private int status;
        private int discount;

        private DateTime? dateCheckIn;
        private DateTime? dateCheckOut;

        public int ID { get { return iD; } set { iD = value; } }
        public DateTime? DateCheckIn { get { return dateCheckIn; } set { dateCheckIn = value; } }
        public DateTime? DateCheckOut { get { return dateCheckOut; } set { dateCheckOut = value; } }
        public int Status { get { return status; } set { status = value; } }

        public int Discount { get => discount; set => discount = value; }
    }
}
