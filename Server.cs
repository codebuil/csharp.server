using System;
using System.Linq;
using System.Net;
using System.Text;

class SimpleServer
{
    static void Main(string[] args)
    {
        string responseString = "<html><body><h1>Olá mundo!</h1></body></html>";
        string s = "http://localhost:8080/";
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.Clear();
        if (args.Count() > 0) s = args[0].ToString();
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add(s); // define o endereço e a porta do servidor
        listener.Start(); // inicia o servidor

        Console.WriteLine("Servidor iniciado na porta 8080");

        while (true)
        {
            HttpListenerContext context = listener.GetContext(); // aguarda uma requisição

            HttpListenerResponse response = context.Response;
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            response.ContentType = "text/html"; // define o tipo de conteúdo como HTML
            Console.WriteLine("**");
            // escreve a resposta
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
    }
}
