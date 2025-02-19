using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System.Threading.Tasks;

namespace FlashNotify;

public static class ScreenFlasher
{
    public static async void FlashScreen(Settings settings)
    {
        var overlay = new Rectangle
        {
            Fill = new SolidColorBrush(GetColorFromHex(settings.FlashColor)),
            Opacity = settings.FlashOpacity,
            Width = Window.Current.Bounds.Width,
            Height = Window.Current.Bounds.Height
        };

        var window = App.MainWindow;
        var grid = window.Content as Grid ?? new Grid();
        grid.Children.Add(overlay);
        window.Content = grid;

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

        grid.Children.Remove(overlay);
    }

    private static Windows.UI.Color GetColorFromHex(string hex)
    {
        hex = hex.Replace("#", "");
        byte a = 255;
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return Windows.UI.Color.FromArgb(a, r, g, b);
    }
}
