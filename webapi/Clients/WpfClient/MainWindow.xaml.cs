using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfClient;

/// <summary>
/// MainWindow.xaml 的交互逻辑
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.DataContext = new MainViewModel();
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        tab.Items.Add(new TabItem()
        {
            Header="1232",
            Content = new TextBlock() { Text = "Hello world" }
        });
    }
}

[ObservableObject]
public partial class MainViewModel
{
    [ObservableProperty] private string displayTime;

    public MainViewModel()
    {
        Task.Run(async () =>
        {
            while (true)
            {
                DisplayTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                await Task.Delay(1000);
            }
        }
        );
    }
}

