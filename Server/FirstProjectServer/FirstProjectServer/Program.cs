using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FirstProjectServer
{
    class Program
    {
        static TcpListener server = new TcpListener(IPAddress.Any, 5001);
        static TcpClient client;
        static NetworkStream stream;


        static void Main(string[] args)
        {
            StartServer();
            Thread recieve = new Thread(() => recieveMessages());
            Thread send = new Thread(() => sendMessages());
            recieve.Start();
            send.Start();
        }

        static void StartServer()
        {
            server.Start();
            client = server.AcceptTcpClient();
            stream = client.GetStream();
            Console.WriteLine("Connected");
        }
        
        static void recieveMessages()
        {
            try
            {
                Byte[] bytes = new byte[256];
                string data = null;

                while (true)
                {
                    try
                    {
                        data = null;
                        int i;
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i = stream.Read(bytes, 0, bytes.Length));//gets message sent from client
                        Console.WriteLine($"\nThem: {data}");//displays the clients message
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine("Lost connection with client...");
                        StartServer();
                        Thread.Sleep(200);
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine($"Socket Exeption: {e}");
            }
            finally
            {
                server.Stop();
            }
        }

        static void sendMessages()
        {
            try
            {
                string Response = string.Empty;
                Byte[] bytes = new byte[256];
                Console.WriteLine("Please enter the message you want to send:");
                while (true)
                {
                    try
                    {
                        Console.Write("You: ");
                        Response = Console.ReadLine();
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(Response);
                        string ForUser = Response.Replace("You: ", "");
                        // Send back a response to the client
                        stream.Write(msg, 0, msg.Length);
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine("Lost connection with client...");
                        StartServer();
                        Thread.Sleep(200);
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine($"Socket Exeption: {e}");
            }
            finally
            {
                server.Stop();
            }
        }
    }
}
