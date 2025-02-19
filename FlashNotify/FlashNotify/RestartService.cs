using Microsoft.Windows.AppLifecycle;

namespace FlashNotifyService;

public static class RestartService
{
    public static void RestartApplication()
    {
        AppInstance.Restart("");
    }
}
