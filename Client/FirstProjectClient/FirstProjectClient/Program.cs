using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FirstProjectClient
{
    class Program
    {
        static TcpClient client = new TcpClient("127.0.0.1", 5001);
        static NetworkStream stream;

        static void Main(string[] args)
        {
            StartClient();
            Thread recieve = new Thread(() => recieveMessages());
            Thread send = new Thread(() => sendMessages());
            recieve.Start();
            send.Start();
        }

        static void StartClient()
        {
            try
            {
                stream = client.GetStream();
                Console.WriteLine("Connected");
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Connection was terminated");
            }
        }

        static void recieveMessages()
        {
            try
            {
                Byte[] bytes = new byte[256];
                string responseData = string.Empty;

                while (true)
                {
                    try
                    {
                        Int32 bytes32 = stream.Read(bytes, 0, bytes.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(bytes, 0, bytes32);
                        Console.WriteLine($"\nThem: {responseData}");
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine("Lost connection with server...");
                        StartClient();
                        Thread.Sleep(200);
                    }
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"ArgumentError {e}");
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketError {e}");
            }
        }

        static void sendMessages()
        {
            try
            {
                Byte[] bytes = new byte[256];
                string responseData = string.Empty;
                Console.WriteLine("Please enter the message you want to send:");

                while (true)
                {
                    try
                    {
                        Console.Write("You: ");
                        responseData = Console.ReadLine();
                        bytes = System.Text.Encoding.ASCII.GetBytes(responseData);
                        string ForUser = responseData.Replace("You: ", "");
                        stream.Write(bytes, 0, bytes.Length);
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine("Lost connection with server...");
                        StartClient();
                        Thread.Sleep(200);
                    }
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"ArgumentError {e}");
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketError {e}");
            }
        }
    }
}
