using Microsoft.Windows.AppLifecycle;

namespace FlashNotify;

public static class RestartService
{
    public static void RestartApplication()
    {
        AppInstance.Restart("");
    }
}
