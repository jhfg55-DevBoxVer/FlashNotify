using Microsoft.UI.Xaml;

namespace FlashNotify;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
        // 如果不想显示窗口，可以设置窗口的 Visibility 或其他属性
        this.Visibility = Visibility.Collapsed;
    }
}
