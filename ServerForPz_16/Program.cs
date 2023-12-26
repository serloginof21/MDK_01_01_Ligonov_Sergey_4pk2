using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;

namespace ServerForPz_16
{
    internal class Program
    {
        static TcpListener tcpListener;
        static List<TcpClient> clients = new List<TcpClient>();
        private static List<FinancialTransaction> transactions = new List<FinancialTransaction>(); // Сделал transactions статическим

        private static object lockObject = new object();

        static void Main(string[] args)
        {
            tcpListener = new TcpListener(IPAddress.Any, 8080);
            tcpListener.Start();

            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            LoadTransaction();

            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                clients.Add(client);

                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                clientThread.Start(client);
            }
        }

        private static void LoadTransaction()
        {
            try
            {
                string json = File.ReadAllText("transactions.json");
                transactions = JsonConvert.DeserializeObject<List<FinancialTransaction>>(json);
                Console.WriteLine("Данные были загружены...");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл данных не найден. Создан новый");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибок при загрузке данных: {ex.Message}");
            }
        }

        private static void SaveTransaction()
        {
            lock (lockObject)
            {
                string json = JsonConvert.SerializeObject(transactions, Formatting.Indented);
                File.WriteAllText("transactions.json", json); // Поправил имя файла
                Console.WriteLine("Данные успешно сохранены");
            }
        }

        static void HandleClient(object obj)
        {
            TcpClient tcpClient = (TcpClient)obj;
            NetworkStream clientStream = tcpClient.GetStream();
            StreamReader reader = new StreamReader(clientStream);
            StreamWriter writer = new StreamWriter(clientStream);

            while (true)
            {
                try
                {
                    string message = reader.ReadLine();
                    if (message != null)
                    {
                        Console.WriteLine("Клиент произвел транзакцию: " + message);
                        FinancialTransaction transaction = JsonConvert.DeserializeObject<FinancialTransaction>(message);

                        SaveTransaction(transaction);
                        SendResponse(tcpClient, "Транзакция успешно сохранена");
                    }
                }
                catch (IOException)
                {
                    // Обработка отключения клиента
                    clients.Remove(tcpClient);
                    break;
                }
            }
        }

        private static void SaveTransaction(FinancialTransaction transaction)
        {
            lock (lockObject)
            {
                transactions.Add(transaction);
                SaveTransaction(); // Сохранение после каждой транзакции
            }
        }

        private static void SendResponse(TcpClient tcpClient, string response)
        {

            NetworkStream clientStream = tcpClient.GetStream();
            StreamWriter writer = new StreamWriter(clientStream);
            writer.WriteLine(response);
            writer.Flush();
        }
    }
}
