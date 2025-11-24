using System.Windows;
using DemoWpf.ViewModels;

namespace DemoWpf;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}