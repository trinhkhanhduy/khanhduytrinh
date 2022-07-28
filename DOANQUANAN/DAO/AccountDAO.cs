using DOANQUANAN.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DOANQUANAN.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return instance; }
            private set { instance = value; }
        }
        private AccountDAO() { }

        public bool Loginn(string userName, string passoword)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(passoword);
            byte[] hashData = new MD5CryptoServiceProvider().ComputeHash(temp);
            string hashPass = "";
            foreach (byte item in hashData)
            {
                hashPass += item;
            }
            string query = "acc_login @userName , @Pass";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { userName, hashPass });
            return result.Rows.Count > 0;
        }

        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExecuteQuery("SELECT  UserName,displayName,kieu FROM dbo.Account");
        }

        public Account GetAccount(string userName)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from Account where userName = '" + userName + "'");

            foreach (DataRow item in data.Rows)
            {
                return new Account(item);
            }
            return null;
        }
        public bool UpdateAccount(string userName, string displayName, string pass, string newpass)
        {

            byte[] temp = ASCIIEncoding.ASCII.GetBytes(pass);
            byte[] temp1 = ASCIIEncoding.ASCII.GetBytes(newpass);

            byte[] hashData = new MD5CryptoServiceProvider().ComputeHash(temp);

            byte[] hashData1 = new MD5CryptoServiceProvider().ComputeHash(temp1);
            string hashPass = "";
            string hashPass1 = "";
            foreach (byte item in hashData)
            {
                hashPass += item;
            }
            foreach (byte item1 in hashData1)
            {
                hashPass1 += item1;
            }
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC UpdateAccount @username , @displayname , @pass , @newpass", new object[] { userName, displayName, hashPass, hashPass1 });
            return result > 0;
        }

        public bool Insertacc(string username, string displayname, int kieu)
        {
            string query = string.Format("Insert dbo.Account ( UserName, displayName, kieu, Pass )Values (N'{0}' , N'{1}' , {2} ,N'{3}')", username, displayname, kieu, "33354741122871651676713774147412831195");

            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool Updateacc(string username, string displayname, int kieu)
        {
            string query = string.Format("UPDATE dbo.Account SET displayName = N'{1}', kieu = {2} WHERE UserName = N'{0}'", username, displayname, kieu);

            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool Deleteacc(string name)
        {

            string query = string.Format("Delete dbo.Account where UserName = N'{0}'", name);

            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }



        public bool Resetpass(string name)
        {
            string query = string.Format("Update Account set Pass = N'33354741122871651676713774147412831195' where UserName = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
