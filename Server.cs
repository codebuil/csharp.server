using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleHTTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            String curdirs = Directory.GetCurrentDirectory();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Clear();
            TcpListener listener = new TcpListener(IPAddress.Any, 8080);
            listener.Start();
            Console.WriteLine(Directory.GetCurrentDirectory());
            Console.WriteLine("Servidor HTTP simples iniciado em http://localhost:8080/");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Conexão estabelecida");

                StreamReader reader = new StreamReader(client.GetStream());
                string request = reader.ReadLine();
                

                string[] tokens = request.Split(' ');
                string path = tokens[1];

                if (path == "/")
                {
                    path = "\\index.html";
                }
                string ppath = path.Replace("/", "\\");
                string filename = curdirs + ppath;
                Console.WriteLine(filename);
                if  (File.Exists(filename))
                {
                    FileStream fileStream = new FileStream(filename, FileMode.Open);
                    byte[] buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, (int)fileStream.Length);
                    fileStream.Close();
                    string contentType = GetContentType(filename);
                    SendResponse(client, "200 OK", contentType, buffer);
                }
                else
                {
                    string response = "404 Not Found";
                    byte[] buffer = Encoding.UTF8.GetBytes(response);
                    SendResponse(client, response, "text/plain", buffer);
                }

                client.Close();
                Console.WriteLine("Conexão terminada");
            }
        }

        static void SendResponse(TcpClient client, string status, string contentType, byte[] data)
        {
            StreamWriter writer = new StreamWriter(client.GetStream());
            writer.WriteLine("HTTP/1.1 " + status);
            writer.WriteLine("Content-Type: " + contentType);
            writer.WriteLine("Content-Length: " + data.Length);
            writer.WriteLine();
            writer.Flush();

            client.GetStream().Write(data, 0, data.Length);
        }

        static string GetContentType(string filename)
        {
            if (filename.EndsWith(".html") || filename.EndsWith(".htm"))
            {
                return "text/html";
            }
            else if (filename.EndsWith(".jpg") || filename.EndsWith(".jpeg"))
            {
                return "image/jpeg";
            }
            else if (filename.EndsWith(".png"))
            {
                return "image/png";
            }
            else if (filename.EndsWith(".gif"))
            {
                return "image/gif";
            }
            else if (filename.EndsWith(".css"))
            {
                return "text/css";
            }
            else if (filename.EndsWith(".js"))
            {
                return "text/javascript";
            }
            else
            {
                return "application/octet-stream";
            }
        }
    }
}
