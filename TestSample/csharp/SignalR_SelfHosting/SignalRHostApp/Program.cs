using System;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Owin;

namespace SignalRHostApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost/";

            using (WebApp.Start<StartUp>(url))
            {
                Console.WriteLine("The Server URL is: {0}", url);
                Console.ReadLine();
            }
        }
    }
}
