using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace agente
{

    static class Program
    {
        private static readonly Socket ClientSocket = new Socket
            (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private const int PORT = 1112;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        
        public static void Main(string[] args)
        { 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            try
            {
                ConnectToServer(args[0]);
                RequestLoop(args[1]);
                Application.Exit();
            }
            catch (Exception ex)
            {
                Exit();
                Application.Exit();
            }

            
        }
        public static string GetIPAddress(string hostname)
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(hostname);

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    //System.Diagnostics.Debug.WriteLine("LocalIPadress: " + ip);
                    return ip.ToString();
                }
            }
            return string.Empty;
        }

        private static void ConnectToServer(string clientip)
        {
            while (!ClientSocket.Connected)
            {
                try
                {
                    //attempts++;
                    //Console.WriteLine("Connection attempt " + attempts);
                    // Change IPAddress.Loopback to a remote IP to connect to a remote host.

                    ClientSocket.Connect(IPAddress.Parse(GetIPAddress(clientip)), PORT);
                    //ClientSocket.Connect(hostEntry.AddressList[0], PORT);
                }
                catch (SocketException)
                {
                    //Console.Clear();
                }
            }
             
            //Console.Clear();
            //Console.WriteLine("Servidor Conectado!");
        }
        

        private static void RequestLoop(string comando)
        {
            //Console.WriteLine(@"<Type ""exit"" to properly disconnect client>");

                SendRequest(comando);
                ReceiveResponse();
                
        }

        /// <summary>
        /// Close socket and exit program.
        /// </summary>
        private static void Exit()
        {
            SendString("exit"); // Tell the server we are exiting
            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            Environment.Exit(0);
        }

        private static void SendRequest(string comando)
        {
                switch (comando)
                {
                    case "ajuda": Console.WriteLine("Comandos disponiveis:\n 'exit' = sair");

                        break;
                    default: SendString(comando);
                        ReceiveResponse();
                        break;
                }



                if (comando.ToLower() == "exit")
                {
                    Exit();
                }
            
        }

        /// <summary>
        /// Sends a string to the server with ASCII encoding.
        /// </summary>
        private static void SendString(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }

        private static void ReceiveResponse()
        {
            var buffer = new byte[2048];
            int received = ClientSocket.Receive(buffer, SocketFlags.None);
            if (received == 0) return;
            var data = new byte[received];
            Array.Copy(buffer, data, received);
            string text = Encoding.ASCII.GetString(data);
            Exit();
            Application.Exit();
            //Console.WriteLine(text);
        }
    }
}
