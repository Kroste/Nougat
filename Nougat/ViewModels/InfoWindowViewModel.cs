using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Nougat.ViewModels;

public partial class InfoWindowViewModel : ViewModelBase
{
    public string Version { get; }
    public string GithubUrl => "https://github.com/Kroste/Nougat";
    public string CoffeeUrl => "https://www.buymeacoffee.com/kroste";
    public string Description =>
        "Offline-NuGet-Bundle-Builder fuer Kroste-Repos.\n" +
        "Waehle Repos aus, Nougat sammelt PackageReferences, fuehrt dotnet restore aus\n" +
        "und schreibt einen NuGet-Local-Ordner (analog zu nuget-offline-bundle.sh).";

    public InfoWindowViewModel()
    {
        var v = Assembly.GetExecutingAssembly().GetName().Version;
        Version = "Version " + (v?.ToString(3) ?? "0.1.0");
    }
}
