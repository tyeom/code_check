using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TextBoxHint;

public class HintTextBoxControl : TextBox
{
    private const string HintTextBlockPartName = "PART_HintTextBlock";
    
    private TextBlock? _hintTextBlockPart;

    public HintTextBoxControl()
    {
        this.DefaultStyleKey = typeof(HintTextBoxControl);
    }

    /// <summary>
    /// Hint Text DependencyProperty
    /// </summary>
    public static readonly DependencyProperty? HintProperty =
        DependencyProperty.Register(
            "Hint",
            typeof(String),
            typeof(TextBox),
            new PropertyMetadata(null));
    /// <summary>
    /// Hint Text
    /// </summary>
    public String? Hint
    {
        get { return (String?)this.GetValue(HintProperty); }
        set { this.SetValue(HintProperty, value); }
    }

    /// <summary>
    /// Hint Size DependencyProperty
    /// </summary>
    public static readonly DependencyProperty? HintSizeProperty =
        DependencyProperty.Register(
            "HintSize",
            typeof(double),
            typeof(TextBox),
            new PropertyMetadata(12d));
    /// <summary>
    /// Hint Size
    /// </summary>
    public double HintSize
    {
        get { return (double)this.GetValue(HintSizeProperty); }
        set { this.SetValue(HintSizeProperty, value); }
    }

    /// <summary>
    /// Hint Foreground DependencyProperty
    /// </summary>
    public static readonly DependencyProperty? HintForegroundProperty =
        DependencyProperty.Register(
            "HintForeground",
            typeof(Brush),
            typeof(TextBox),
            new PropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("#FF000000")!));  // Default Color #FF000000(Black)
    /// <summary>
    /// Hint Foreground
    /// </summary>
    public Brush HintForeground
    {
        get { return (Brush)this.GetValue(HintForegroundProperty); }
        set { this.SetValue(HintForegroundProperty, value); }
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _hintTextBlockPart = GetTemplateChild(HintTextBlockPartName) as TextBlock;

        this.TextChanged += this.HintTextBoxControl_TextChanged;
    }

    private void HintTextBoxControl_TextChanged(object sender, TextChangedEventArgs e)
    {
        if(string.IsNullOrWhiteSpace(this.Text.Trim()))
        {
            _hintTextBlockPart!.Visibility = Visibility.Visible;
        }
        else
        {
            _hintTextBlockPart!.Visibility = Visibility.Collapsed;
        }
    }
}