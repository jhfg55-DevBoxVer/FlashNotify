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
        string rawText = string.Empty;

        // 尝试从通知 XML 中提取文本内容
        try
        {
            var doc = notification.Notification.Content;
            var textNodes = doc.GetElementsByTagName("text");
            foreach (var node in textNodes)
            {
                rawText += node.InnerText + " ";
            }
        }
        catch (Exception ex)
        {
            // 此处可记录异常或根据需要处理
            Console.WriteLine($"解析通知内容时发生异常: {ex.Message}");
        }

        // 根据通知内容判断优先级
        if (rawText.IndexOf("紧急", StringComparison.OrdinalIgnoreCase) >= 0 ||
            rawText.IndexOf("立即", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            return "High";
        }
        else if (rawText.IndexOf("提示", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            return "Low";
        }
        else
        {
            return "Medium";
        }
    }
}
