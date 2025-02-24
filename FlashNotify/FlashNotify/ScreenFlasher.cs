using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Threading.Tasks;

namespace FlashNotify;

public static class ScreenFlasher
{
    public static async void FlashScreen(Settings settings)
    {
        // 创建新的窗口
        var window = new Window();

        // 设置窗口属性（全屏，无边框）
        window.ExtendsContentIntoTitleBar = true;
        window.SetTitleBar(null);
        window.AppWindow.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.FullScreen);

        // 创建覆盖层
        var grid = new Grid();
        var overlay = new Border
        {
            Background = new SolidColorBrush(GetColorFromHex(settings.FlashColor)),
            Opacity = settings.FlashOpacity,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };
        grid.Children.Add(overlay);
        window.Content = grid;

        // 显示窗口
        window.Activate();

        // 闪烁效果
        for (int i = 0; i < settings.FlashCount; i++)
        {
            overlay.Visibility = Visibility.Visible;
            await Task.Delay(settings.FlashInterval);
            overlay.Visibility = Visibility.Collapsed;
            if (i < settings.FlashCount - 1)
            {
                await Task.Delay(settings.FlashInterval);
            }
        }

        // 关闭窗口，释放资源
        window.Close();
    }

    private static Windows.UI.Color GetColorFromHex(string hex)
    {
        hex = hex.Replace("#", "");
        byte a = 255;
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            hex = hex.Substring(2);
        }
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        return Windows.UI.Color.FromArgb(a, r, g, b);
    }
}
