using Windows.UI.Notifications;
using Windows.UI.Notifications.Management;
using System;

namespace FlashNotify;

public class ToastNotificationListener
{
    public async void StartListening()
    {
        var listener = UserNotificationListener.Current;
        var accessStatus = await listener.RequestAccessAsync();

        if (accessStatus == UserNotificationListenerAccessStatus.Allowed)
        {
            listener.NotificationChanged += OnNotificationChanged;
        }
    }

    private async void OnNotificationChanged(UserNotificationListener sender, UserNotificationChangedEventArgs args)
    {
        if (args.ChangeKind == UserNotificationChangedKind.Added)
        {
            var notifications = await sender.GetNotificationsAsync(NotificationKinds.Toast);
            foreach (var notification in notifications)
            {
                HandleNotification(notification);
            }
        }
    }

    private void HandleNotification(Windows.UI.Notifications.UserNotification notification)
    {
        var appId = notification.AppInfo.AppUserModelId;
        var priority = GetNotificationPriority(notification);

        var settings = RegistryManager.GetSettingsForApp(appId, priority);

        if (settings.EnableScreenFlash)
        {
            ScreenFlasher.FlashScreen(settings);
        }

        if (settings.EnableFlashlight)
        {
            FlashlightFlasher.FlashFlashlight(settings);
        }
    }

    private string GetNotificationPriority(Windows.UI.Notifications.UserNotification notification)
    {
        // 这里可以根据通知内容或通知类型来确定优先级
        // 由于示例中未指定具体的判定方法，这里默认返回 "Medium"
        return "Medium";
    }
}
