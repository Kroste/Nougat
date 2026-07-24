using Nougat.Chrome;
using Nougat.ViewModels;

namespace Nougat.Views;

public partial class SdkInstallWindow : ChromeWindow
{
    public SdkInstallWindow()
    {
        InitializeComponent();
    }

    public SdkInstallWindow(SdkInstallViewModel vm) : this()
    {
        DataContext = vm;
        vm.Completed += (_, _) => Close();
    }
}
