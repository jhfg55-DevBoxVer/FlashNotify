using Microsoft.UI.Xaml;

namespace FlashNotify;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
        // ���������ʾ���ڣ��������ô��ڵ� Visibility ����������
        this.Visibility = Visibility.Collapsed;
    }
}
