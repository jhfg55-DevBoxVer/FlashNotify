using FlashNotify;
using Microsoft.UI.Xaml;

namespace FlashNotifyService;

public partial class App : Application
{
    public App()
    {
        // 不调用 InitializeComponent 或者确保其内容最小化
        this.UnhandledException += App_UnhandledException;
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        RegistryManager.CheckAndSetDefaultSettings();
        RegistryManager.SetAutoStart();

        // 不创建 MainWindow
        // MainWindow = new MainWindow();
        // MainWindow.Activate();

        // 启动通知监听器
        var listener = new ToastNotificationListener();
        listener.StartListening();
    }

    private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        e.Handled = true;
        RestartService.RestartApplication();
    }
}
