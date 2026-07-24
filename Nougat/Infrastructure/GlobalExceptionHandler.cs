using System;
using System.Threading.Tasks;
using NLog;

namespace Nougat.Infrastructure;

public static class GlobalExceptionHandler
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public static void Install()
    {
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            _logger.Fatal(e.ExceptionObject as Exception, "Unbehandelte AppDomain-Exception");

        TaskScheduler.UnobservedTaskException += (_, e) =>
        {
            _logger.Fatal(e.Exception, "Unbeobachtete Task-Exception");
            e.SetObserved();
        };
    }
}
