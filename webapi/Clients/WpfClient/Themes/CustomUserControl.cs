﻿using System.Windows;
using System.Windows.Controls;
namespace WpfClient;
public class CustomUserControl : UserControl
{
    static CustomUserControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomUserControl), new FrameworkPropertyMetadata(typeof(CustomUserControl)));
    }

    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(CustomUserControl), new PropertyMetadata(""));
}
