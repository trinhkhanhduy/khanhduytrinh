﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOANQUANAN.DAO
{
    public class DataProvider

    {
        private static DataProvider instance;

        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return DataProvider.instance; }
            private set { DataProvider.instance = value; }
        }


        private DataProvider()
        {

        }

        private string connectionSTR = @"Data Source=DUY\SQLEXPRESS;Initial Catalog=QuanLyQuanAn;Integrated Security=True";


        public DataTable ExecuteQuery(string query, object[] parameter = null)

        {
            DataTable data = new DataTable();
            using (SqlConnection Conn = new SqlConnection(connectionSTR))
            {

                Conn.Open();

                SqlCommand command = new SqlCommand(query, Conn);
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }




                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(data);

                Conn.Close();
            }


            return data;



        }


        public int ExecuteNonQuery(string query, object[] parameter = null)

        {
            int data = 0;
            using (SqlConnection Conn = new SqlConnection(connectionSTR))
            {

                Conn.Open();

                SqlCommand command = new SqlCommand(query, Conn);
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }

                data = command.ExecuteNonQuery();
                Conn.Close();

            }


            return data;



        }

        public object ExecuteScalar(string query, object[] parameter = null)

        {
            object data = 0;
            using (SqlConnection Conn = new SqlConnection(connectionSTR))
            {

                Conn.Open();

                SqlCommand command = new SqlCommand(query, Conn);
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }




                data = command.ExecuteScalar();

                Conn.Close();
            }


            return data;



        }
    }
}
