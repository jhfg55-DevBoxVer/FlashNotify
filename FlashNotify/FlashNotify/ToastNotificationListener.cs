using Windows.UI.Notifications;
using Windows.UI.Notifications.Management;
using System;
using Windows.Data.Xml.Dom;

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
        // 获取应用设置时不再考虑通知优先级
        var settings = RegistryManager.GetSettingsForApp(appId);

        if (settings.EnableScreenFlash)
        {
            ScreenFlasher.FlashScreen(settings);
        }

        if (settings.EnableFlashlight)
        {
            FlashlightFlasher.FlashFlashlight(settings);
        }
    }
}
