using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace HardHashTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TotalTime.Text = $"Starting...";

        }

        SHA256 hasher = SHA256.Create();

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                var startTime = DateTime.Now;

                Dispatcher.Invoke(() => { TotalTime.Text = $"Creating buffer.."; });

                var buffer = new byte[512 * 1024 * 1024];
                var rnd = new Random();

                var ts = new TimeSpan[100];

                var prevTime = DateTime.Now;
                for (int i = 0; i < 100; i++)
                {
                    Dispatcher.Invoke(() => { TotalTime.Text = $"Hash {i:##0}"; });
                    rnd.NextBytes(buffer);
                    hasher.ComputeHash(buffer);
                    ts[i] = DateTime.Now - prevTime;
                    prevTime = DateTime.Now;
                }

                Dispatcher.Invoke(() =>
                {
                    TotalTime.Text = $"Total time = {DateTime.Now - startTime}";
                    for (var i = 0; i < ts.Length; i++)
                    {
                        TimeList.Items.Add($"{i:0000} {ts[i]}");
                    }
                });
            });

        }
    }
}