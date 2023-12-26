using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace PZ_16
{
    public partial class MainWindow : Window
    {
        private decimal acountBalance = 5000.0m;
        private TcpClient tcpClient;
        private NetworkStream clientStream;
        private StreamWriter writer;

        public MainWindow()
        {
            InitializeComponent();
            ConnectToServer();
        }

        private void ConnectToServer()
        {
            tcpClient = new TcpClient("127.0.0.1", 8080);
            clientStream = tcpClient.GetStream();
            writer = new StreamWriter(clientStream);
        }

        private void SendTransaction(string clientName, decimal amount, string transactionType)
        {
            // Создание объекта FinancialTransaction
            FinancialTransaction transaction = new FinancialTransaction
            {
                ClientName = clientName,
                Amount = amount,
                TransactionType = transactionType
            };
            if (transaction.TransactionType == "Доход")
            {
                acountBalance += transaction.Amount;
                UpdateAccoBalance();
            }
            else if (transaction.TransactionType == "Расход")
            {
                if (acountBalance < transaction.Amount)
                {
                    MessageBox.Show("Недостаточно средст");
                    return;
                }
                acountBalance -= transaction.Amount;
                UpdateAccoBalance();
            }

            // Отправка транзакции серверу
            string jsonTransaction = JsonConvert.SerializeObject(transaction);
            writer.WriteLine(jsonTransaction);
            writer.Flush();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем данные из пользовательского интерфейса (например, текстовые поля)
            string clientName = ClientNameTextBox.Text;
            decimal amount = decimal.Parse(AmountTextBox.Text);
            string transactionType = TransactionTypeComboBox.SelectedItem.ToString();


            // Создаем экземпляр FinancialTransaction и отправляем на сервер
            SendTransaction(clientName, amount, transactionType);
        }
        private void UpdateAccoBalance()
        {
            AccountBalanceTextBlock.Text = $"Баланс счета: {acountBalance:C}";
        }
    }
}
