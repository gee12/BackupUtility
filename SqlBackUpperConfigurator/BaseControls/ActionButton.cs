using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SBUConfigurator
{
    public class ActionButton : Button
    {
        public static Brush AppGradientBrushGreen = Application.Current.Resources["AppGradientBrushGreen"] as Brush;

        public ActionButton()
        {
            this.Background = AppGradientBrushGreen;
        }

        public ActionButton(string toolTip, object content, int w, int h, Thickness margin, ICommand command)
        {
            Background = AppGradientBrushGreen;
            ToolTip = toolTip;
            Content = content;
            Width = w;
            Height = h;
            Margin = margin;
            Command = command;
        }
    }
}
