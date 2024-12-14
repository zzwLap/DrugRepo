using System.Windows;

namespace WpfClient;
/// <summary>
/// WindowTemplate.xaml 的交互逻辑
/// </summary>
public partial class WindowTemplate : Window
{
    public WindowTemplate(CustomUserControl element)
    {
        InitializeComponent();
        content.Content = element;
    }
    public WindowTemplate(FrameworkElement element)
    {
        InitializeComponent();
        content.Content = element;
    }
}

public static class WindowExtendsion
{
    public static Dictionary<FrameworkElement, bool> dialogWindows = new Dictionary<FrameworkElement, bool>();

    public static void Show(this CustomUserControl @this, ResizeMode resieMode = ResizeMode.NoResize)
    {
        var view = new WindowTemplate(@this);
        view.Title = @this.Title;
        view.SetOwner();
        view.Height = @this.Height;
        view.Width = @this.Width;
        view.ResizeMode = resieMode;
        @this.Height = double.NaN;
        @this.Width = double.NaN;
        view.Show();
    }

    public static void Show(this FrameworkElement @this, ResizeMode resieMode = ResizeMode.NoResize)
    {
        var view = new WindowTemplate(@this);
        view.SetOwner();
        view.Height = @this.Height;
        view.Width = @this.Width;
        view.ResizeMode = resieMode;
        @this.Height = double.NaN;
        @this.Width = double.NaN;
        view.Show();
    }

    public static bool? ShowDialog(this CustomUserControl @this, ResizeMode resieMode = ResizeMode.NoResize)
    {
        var view = new WindowTemplate(@this);
        view.Title = @this.Title;
        view.SetOwner();
        view.Height = @this.Height;
        view.Width = @this.Width;
        view.ResizeMode = resieMode;
        @this.Height = double.NaN;
        @this.Width = double.NaN;
        dialogWindows[@this] = true;
        return view.ShowDialog();
    }

    public static bool? ShowDialog(this CustomUserControl @this, string title)
    {
        var view = new WindowTemplate(@this);
        view.Title = title;
        view.SetOwner();
        view.Height = @this.Height;
        view.Width = @this.Width;
        @this.Height = double.NaN;
        @this.Width = double.NaN;
        return view.ShowDialog();
    }

    public static void Close(this CustomUserControl @this, bool? dialogResult = null)
    {
        var win = Window.GetWindow(@this);

        if (dialogWindows.TryGetValue(@this, out var isDialog) && isDialog == true)
        {
            win.DialogResult = dialogResult;
        }

        win.Close();
        dialogWindows.Remove(@this);
        return;
    }

    public static void SetOwner(this FrameworkElement @this, FrameworkElement parent = null)
    {
        var win = Window.GetWindow(@this);
        if (parent != null)
        {
            win.Owner = Window.GetWindow(parent);
        }
        else
        {
            win.Owner = Application.Current.MainWindow;
        }
    }
}
