using Microsoft.Win32;
using System;

namespace FlashNotify;

public class RegistryManager
{
    private const string RegistryPath = @"SOFTWARE\FlashNotifyService";

    public static void CheckAndSetDefaultSettings()
    {
        using var key = Registry.LocalMachine.OpenSubKey(RegistryPath, true) ?? Registry.LocalMachine.CreateSubKey(RegistryPath);
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

        

        // TODO: 读取应用特定设置，并覆盖上述设置

        return settings;
    }

    public static void SetAutoStart()
    {
        const string policyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Scripts\Logon";
        using var key = Registry.LocalMachine.OpenSubKey(policyPath, true);
        if (key != null)
        {
            // 添加启动脚本或程序
            // 由于组策略的复杂性，这里仅作示例，实际可能需要使用更高级的方法来修改组策略
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
}
