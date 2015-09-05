using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Enums.Filters;

namespace VkLongPollApi.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var vk = new VkApi();

            Console.Write("Application ID:");
            int applicationId = Convert.ToInt32(Console.ReadLine());

            Console.Write("Login:");
            string login = Console.ReadLine();

            Console.Write("Password:");
            string password = Console.ReadLine();

            Console.WriteLine("Авторизация...");
            vk.Authorize(applicationId, login, password, Settings.Messages);
            Console.WriteLine("Авторизовались. Идем дальше...");

            var longPoll = new VkNet.VkLongPollApi(vk);
            longPoll.Messages
                .Subscribe(
                    msg => Console.WriteLine("{0:10} {1:dd.MM.yyyy} {2}: {3}", msg.Id, msg.Date, msg.UserId, msg.Body),
                    exception =>
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(exception.Message);
                        Console.ResetColor();
                    },
                    () =>
                    {
                        Console.WriteLine("Это конец");
                    }
                );

            Console.ReadKey();
        }
    }
}
