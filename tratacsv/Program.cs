using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;

namespace tratacsv
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] separators = { ".","+" };
            /*
            string caminho = @"C:\Program_Manager\AgenteTivit\Hardware\Inv_Padrao\IOA102194+CPU.csv";
            string nome = Path.GetFileName(caminho);
            string[] tabela = nome.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            //tabela[1] captura nome da tabela
            String query = "bulk insert GER_AMB_TIVIT." + tabela[1] + " from '" + caminho + @"' with (firstrow 3, fieldterminator =',', rowterminator='\n' );";
            */
            int retorno = 0;
            foreach (string file in Directory.EnumerateFiles(@"C:\Program_Manager\AgenteTivit", "*.*", SearchOption.AllDirectories))
            {
                try
                {
                    string nom = Path.GetFileName(file);
                    string[] tabela = nom.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    Connection cones1 = new Connection();
                    SqlCommand cmd1 = new SqlCommand("delete from " + tabela[1] + " where node =  '" + tabela[0] + "'", cones1.cone());
                    retorno = cmd1.ExecuteNonQuery();
                    cones1.cone().Close();

                    string[] tabel = nom.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    String query = @"bulk insert " + tabel[1] + " from '" + file + @"' with (FIRSTROW =2, fieldterminator = ',' , rowterminator='\n', DATAFILETYPE='widechar')";


                    Connection cones = new Connection();
                    SqlCommand cmd = new SqlCommand(query, cones.cone());
                    retorno = cmd.ExecuteNonQuery();
                    cones.cone().Close();

                    Connection con_data = new Connection();
                    SqlCommand cmd_data = new SqlCommand("update maquinas set data_inventario='" + DateTime.Now + "' where hostname='" + tabela[0] + "'", con_data.cone());
                    retorno = cmd_data.ExecuteNonQuery();
                    con_data.cone().Close();

                    //Console.WriteLine(query);
                    File.Move(file,Directory.GetCurrentDirectory() + @"\processados\" + nom);

                }
                catch (Exception ex)
                {
                    continue;
                }
                
            }
            
            /*
            foreach (var path in Directory.GetFiles(@"C:\Program_Manager\AgenteTivit\Hardware\Inv_Padrao"))
            {
                Console.WriteLine(path); // full path
                Console.WriteLine(System.IO.Path.GetFileName(path)); // file name
            }
            //Console.WriteLine(query);
            Console.ReadKey();
            /*
            Connection cones = new Connection();
            String query = "bulk insert GER_AMB_TIVIT." + tabela[1]+" from '"+caminho+@"' with (firstrow 3, fieldterminator =',', rowterminator='\n' );";
            SqlCommand cmd = new SqlCommand(query, cones.cone());
            cones.cone().Open();
            cmd.ExecuteNonQuery();
            cones.cone().Close();*/

            
           
        }
    }
}
