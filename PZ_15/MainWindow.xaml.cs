using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
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

namespace PZ_15
{
    public partial class MainWindow : Window
    {
        private List<NetworkInterface> interfaces;
        private Timer timer;

        public MainWindow()
        {
            InitializeComponent();

            InitializeComponent();
            interfaces = new List<NetworkInterface>();

            // Получения список сетевых интерфейсов
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                interfaces.Add(nic);
                listViewInterfaces.Items.Add(nic.Description);
            }

            //таймер для обновления информации каждую секунду
            timer = new Timer(UpdateNetworkInfo, null, 0, 500);
        }

        private void UpdateNetworkInfo(object state)
        {
            Dispatcher.Invoke(() =>
            {
                // Обнова инфы при выборе интерфейса
                if (listViewInterfaces.SelectedIndex >= 0)
                {
                    int selectedIndex = listViewInterfaces.SelectedIndex;
                    NetworkInterface selectedInterface = interfaces[selectedIndex];
                    UpdateInterfaceInfo(selectedInterface);
                }
            });
        }
        private void UpdateInterfaceInfo(NetworkInterface nic)
        {
            // Получение инфы о сетевом интерфейсе и обновления TextBlock ов
            textBlockInterfaceType.Text = $"Тип интерфейса: {nic.NetworkInterfaceType}";
            textBlockMaxSpeed.Text = $"Максимальная скорость: {nic.Speed / 1000000} Мбит/с";
            textBlockTotalBytesSent.Text = $"Передано: {nic.GetIPv4Statistics().BytesSent} байт";
            textBlockTotalBytesReceived.Text = $"Получено: {nic.GetIPv4Statistics().BytesReceived} байт";
            textBlockDownloadSpeed.Text = $"Скорость загрузки: {nic.GetIPv4Statistics().BytesReceived / 1024} Кб/с";
            textBlockUploadSpeed.Text = $"Скорость отдачи: {nic.GetIPv4Statistics().BytesSent / 1024} Кб/с";
        }
        private void listViewInterfaces_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            // Обновления информации при изменении выбранного интерфейса
            int selectedIndex = listViewInterfaces.SelectedIndex;
            NetworkInterface selectedInterface = interfaces[selectedIndex];
            UpdateInterfaceInfo(selectedInterface);
        }
    }
}
