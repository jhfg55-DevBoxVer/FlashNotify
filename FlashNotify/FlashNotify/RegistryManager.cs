using Microsoft.Win32;
using System;

namespace FlashNotify;

public class RegistryManager
{
    private const string RegistryPath = @"SOFTWARE\FlashNotifyService";
    private const string LogFile = "FlashNotify.log";

    public static void CheckAndSetDefaultSettings()
    {
        using var key = Registry.LocalMachine.OpenSubKey(RegistryPath, true)
                          ?? Registry.LocalMachine.CreateSubKey(RegistryPath);
        if (key.GetSubKeyNames().Length == 0)
        {
            // 写入默认设置
            SetDefaultSettings(key);
        }
    }

    private static void SetDefaultSettings(RegistryKey key)
    {
        // Global设置
        using var globalKey = key.CreateSubKey("Global");
        globalKey.SetValue("FlashColor", "#FFFFE0");
        globalKey.SetValue("FlashCount", 2);
        globalKey.SetValue("FlashInterval", 300);
        globalKey.SetValue("FlashOpacity", 0.3);
        globalKey.SetValue("EnableScreenFlash", true);
        globalKey.SetValue("EnableFlashlight", false);
        globalKey.SetValue("FlashlightFlashCount", 2);
        globalKey.SetValue("FlashlightDuration", 300);
        globalKey.SetValue("FlashlightOpacity", 0.3);
    }

    // 已删除通知优先级相关参数
    public static Settings GetSettingsForApp(string appId, string priority)
    {
        var settings = new Settings();
        using var key = Registry.LocalMachine.OpenSubKey(RegistryPath);

        // 读取Global设置
        using var globalKey = key.OpenSubKey("Global");
        settings.EnableScreenFlash = Convert.ToBoolean(globalKey.GetValue("EnableScreenFlash", true));
        settings.FlashColor = globalKey.GetValue("FlashColor", "#FFFFE0").ToString();
        settings.FlashCount = Convert.ToInt32(globalKey.GetValue("FlashCount", 2));
        settings.FlashInterval = Convert.ToInt32(globalKey.GetValue("FlashInterval", 300));
        settings.FlashOpacity = Convert.ToDouble(globalKey.GetValue("FlashOpacity", 0.3));
        settings.EnableFlashlight = Convert.ToBoolean(globalKey.GetValue("EnableFlashlight", false));
        settings.FlashlightFlashCount = Convert.ToInt32(globalKey.GetValue("FlashlightFlashCount", 2));
        settings.FlashlightDuration = Convert.ToInt32(globalKey.GetValue("FlashlightDuration", 300));

        // 读取应用特定设置，并覆盖上述设置
        using var appKey = key.OpenSubKey(appId);
        if (appKey != null)
        {
            settings.EnableScreenFlash = Convert.ToBoolean(appKey.GetValue("EnableScreenFlash", settings.EnableScreenFlash));
            settings.FlashColor = appKey.GetValue("FlashColor", settings.FlashColor).ToString();
            settings.FlashCount = Convert.ToInt32(appKey.GetValue("FlashCount", settings.FlashCount));
            settings.FlashInterval = Convert.ToInt32(appKey.GetValue("FlashInterval", settings.FlashInterval));
            settings.FlashOpacity = Convert.ToDouble(appKey.GetValue("FlashOpacity", settings.FlashOpacity));
            settings.EnableFlashlight = Convert.ToBoolean(appKey.GetValue("EnableFlashlight", settings.EnableFlashlight));
            settings.FlashlightFlashCount = Convert.ToInt32(appKey.GetValue("FlashlightFlashCount", settings.FlashlightFlashCount));
            settings.FlashlightDuration = Convert.ToInt32(appKey.GetValue("FlashlightDuration", settings.FlashlightDuration));
        }

        return settings;
    }

    public static void SetAutoStart()
    {
        const string policyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Scripts\Logon";
        using var key = Registry.LocalMachine.OpenSubKey(policyPath, true);
        if (key != null)
        {
            // 获取当前应用的完整路径
            var appPath = System.Reflection.Assembly.GetEntryAssembly()?.Location;
            if (string.IsNullOrEmpty(appPath))
            {
                Logger.Log("无法获取当前应用路径，自动启动配置失败。");
                return;
            }

            // 使用子项 "0" 表示第一个登录脚本
            using var scriptKey = key.CreateSubKey("0");
            if (scriptKey != null)
            {
                // 设置启动脚本路径和参数（假设无额外参数，且 Order 指定执行顺序）
                scriptKey.SetValue("Script", appPath);
                scriptKey.SetValue("Parameters", "");
                scriptKey.SetValue("Order", 0);
            }

            // 设置同步运行登录脚本（添加或更新 RunLogonScriptSynchronous 值为 1）
            using var systemKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", true);
            if (systemKey != null)
            {
                systemKey.SetValue("RunLogonScriptSynchronous", 1, RegistryValueKind.DWord);
            }
            else
            {
                Logger.Log($"无法打开注册表路径: SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
            }
        }
        else
        {
            Logger.Log($"无法打开注册表路径: {policyPath}");
        }
    }

}

public class Settings
{
    public bool EnableScreenFlash { get; set; }
    public string FlashColor { get; set; }
    public int FlashCount { get; set; }
    public int FlashInterval { get; set; }
    public double FlashOpacity { get; set; }
    public bool EnableFlashlight { get; set; }
    public int FlashlightFlashCount { get; set; }
    public int FlashlightDuration { get; set; }
    public double FlashlightOpacity { get; set; }
}
