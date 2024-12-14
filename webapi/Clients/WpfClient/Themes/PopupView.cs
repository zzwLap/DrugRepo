using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WpfClient;

[TemplatePart(Name = ThumbName, Type = typeof(Thumb))]
[TemplatePart(Name = PopupName, Type = typeof(Popup))]
public class PopupView : ContentControl
{
    public const string ThumbName = "PART_Thumb";
    public const string PopupName = "PART_Popup";
    static PopupView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupView), new FrameworkPropertyMetadata(typeof(PopupView)));
        IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(PopupView), new PropertyMetadata(true));
    }

    public static readonly DependencyProperty IsOpenProperty;

    private Popup popup { get; set; }

    public override void OnApplyTemplate()
    {
        var thumb = this.GetTemplateChild(ThumbName) as Thumb;
        if (thumb != null)
        {
            thumb.DragDelta += Thumb_DragDelta;
        }

        var popup = this.GetTemplateChild(PopupName) as Popup;
        this.popup = popup;

        base.OnApplyTemplate();
    }

    private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
    {
        var width = this.popup.Width + e.HorizontalChange;
        var height = this.popup.Height + e.VerticalChange;

        this.popup.Width = width > 100 ? width : 100;
        this.popup.Height = height > 200 ? height : 200;
    }

    public bool IsOpen
    {
        get { return (bool)GetValue(IsOpenProperty); }
        set { SetValue(IsOpenProperty, value); }
    }
}
