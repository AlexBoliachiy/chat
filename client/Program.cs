using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using client.ServiceReference1;
namespace GettingStartedClient
{

    // Прототип-
    // Прокси+
    // Посредник+
    // MVC+
    // Динамическая стратегия+
    // Стратегия +

    //Flyweight создаине табов


    //Бан. Если забанен, то ничего не отобразить. Юзать стратегию
    // Абстрактная фабрика интерфейсов.
    // Фабричный метод комманд
    // Команды, которые скачивает клиент когда заходит в сеть. Например сообщение, добавить нового друга 
    // Посредник формально уже реализован. Клиент это объект номер 1, сервер это объект номер два и класс хранилище это объект номер 3.
    // Вывод разных шрифтов. Мост
    class Program
    {
        static void Main(string[] args)
        {
            //Step 1: Create an instance of the WCF proxy.
            MessangerClient client = new MessangerClient();
            int id = client.Authorization("FirstUser", "Secret");
            if (id == -1)
                throw new ArgumentException(" Server don't dedicate id");
            Message msg = new Message();
            msg.to = msg.from = "FirstUser";
            msg.message = "Hello, firstUser";
            Console.WriteLine(client.SendMessage(id, msg));
            Console.WriteLine(client.GetNewMessage("FirstUser", id)[0].message);
            Console.ReadKey();
            client.Close();
        }
    }
}