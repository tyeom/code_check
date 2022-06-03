namespace SimpleMVVMWpf.Behaviours
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    public class TextBoxValidationBehaviour : DependencyObject
    {
        public static readonly DependencyProperty ValidationActionProperty = DependencyProperty.Register(
           "ValidationFunc", typeof(Func<bool>), typeof(TextBoxValidationBehaviour), new PropertyMetadata(null));

        public static Func<bool> GetValidationFunc(DependencyObject obj)
        {
            return (Func<bool>)obj.GetValue(ValidationActionProperty);
        }

        public static void SetValidationFunc(DependencyObject obj, Func<bool> value)
        {
            obj.SetValue(ValidationActionProperty, value);
        }

        /// <summary>
        /// 유효성 검사 시작 - true인 경우 검사
        /// </summary>
        public static readonly DependencyProperty ValidationProperty = DependencyProperty.RegisterAttached(
           "Validation", typeof(bool), typeof(TextBoxValidationBehaviour), new UIPropertyMetadata(false, null, ValidationCallback));

        public static bool GetValidation(DependencyObject obj)
        {
            return (bool)obj.GetValue(ValidationProperty);
        }

        public static void SetValidation(DependencyObject obj, bool value)
        {
            obj.SetValue(ValidationProperty, value);
        }

        /// <summary>
        /// 유효성 검사 성공 여부
        /// </summary>
        public static readonly DependencyProperty IsValidationSuccessProperty = DependencyProperty.RegisterAttached(
           "IsValidationSuccess", typeof(bool), typeof(TextBoxValidationBehaviour), new UIPropertyMetadata(false));

        public static bool GetIsValidationSuccess(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsValidationSuccessProperty);
        }

        public static void SetIsValidationSuccess(DependencyObject obj, bool value)
        {
            obj.SetValue(IsValidationSuccessProperty, value);
        }

        private static object ValidationCallback(DependencyObject d, object baseValue)
        {
            if ((bool)baseValue)
            {
                TextBox tb = d as TextBox;
                if (tb == null)
                    return false;

                if(GetValidationFunc(d)())
                {
                    SetIsValidationSuccess(d, true);
                }
                else
                {
                    tb.Focus();
                    tb.SelectAll();
                    SetIsValidationSuccess(d, false);
                }
            }

            return false;
        }
    }
}
