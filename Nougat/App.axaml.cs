using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Nougat.Infrastructure;
using Nougat.Services;
using Nougat.ViewModels;
using Nougat.Views;

namespace Nougat;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;
    private TrayController? _tray;

    public override void Initialize()
    {
        MaskingLayoutRenderer.RegisterRenderer();
        GlobalExceptionHandler.Install();
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Services = new ServiceCollection().AddNougat().BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var vm = Services.GetRequiredService<MainWindowViewModel>();
            var window = new MainWindow { DataContext = vm };
            desktop.MainWindow = window;

            _tray = new TrayController(this, window);
            _tray.Install();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
