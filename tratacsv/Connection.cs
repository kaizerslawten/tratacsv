using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
//Extras
using System.Data.SqlClient;

    public class Connection
    {
       string deco()
        {
            String dec = "";

            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(Directory.GetCurrentDirectory() + "\\config.cfg");
                byte[] b = (Convert.FromBase64String(file.ReadLine()));
                dec = Encoding.UTF8.GetString(b);
                return dec;
            }
            catch (System.IO.FileNotFoundException ex)
            {
                return dec;
            }
        }
        
        public SqlConnection cone()
        {
            SqlConnection con = new SqlConnection(deco()+";Initial Catalog=GER_AMB_TIVIT");            
            con.Open();
            return con;
        }
    }


