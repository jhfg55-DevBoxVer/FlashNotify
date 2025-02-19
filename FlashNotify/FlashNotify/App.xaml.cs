using Microsoft.UI.Xaml;

namespace FlashNotify;

public partial class App : Application
{
    public App()
    {
        this.InitializeComponent();
        this.UnhandledException += App_UnhandledException;
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        RegistryManager.CheckAndSetDefaultSettings();
        RegistryManager.SetAutoStart();

        MainWindow = new MainWindow();
        MainWindow.Activate();

        // 启动通知监听器
        var listener = new ToastNotificationListener();
        listener.StartListening();
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        e.Handled = true;
        // 使用重启服务
        RestartService.RestartApplication();
    }

    public static Window MainWindow { get; private set; }
}
