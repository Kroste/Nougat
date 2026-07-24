using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using Nougat.Chrome;
using Nougat.ViewModels;

namespace Nougat.Views;

public partial class MainWindow : ChromeWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnSettingsClick(object? sender, RoutedEventArgs e)
    {
        var vm = App.Services.GetRequiredService<SettingsWindowViewModel>();
        var window = new SettingsWindow(vm);
        window.ShowDialog(this);
    }

    private void OnAboutClick(object? sender, RoutedEventArgs e)
    {
        var vm = App.Services.GetRequiredService<InfoWindowViewModel>();
        var window = new InfoWindow { DataContext = vm };
        window.ShowDialog(this);
    }

    private void OnInstallSdkClick(object? sender, RoutedEventArgs e)
    {
        var vm = App.Services.GetRequiredService<SdkInstallViewModel>();
        var window = new SdkInstallWindow(vm);
        window.ShowDialog(this);
    }
}
