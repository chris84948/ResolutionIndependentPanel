using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;

namespace ResolutionIndependentPanel
{
    public class ResolutionIndependentPanel : ContentControl
    {
        public static readonly DependencyProperty DesignWidthProperty =
            DependencyProperty.Register("DesignWidth",
                                        typeof(double?),
                                        typeof(ResolutionIndependentPanel),
                                        new FrameworkPropertyMetadata(null));
        public double? DesignWidth
        {
            get { return (double?)GetValue(DesignWidthProperty); }
            set { SetValue(DesignWidthProperty, value); }
        }

        public static readonly DependencyProperty DesignHeightProperty =
            DependencyProperty.Register("DesignHeight",
                                        typeof(double?),
                                        typeof(ResolutionIndependentPanel),
                                        new FrameworkPropertyMetadata(null));
        public double? DesignHeight
        {
            get { return (double?)GetValue(DesignHeightProperty); }
            set { SetValue(DesignHeightProperty, value); }
        }

        private ContentControl _content;
        private Grid _gridMain;
        private ScaleTransform _resolutionScaleTransform;
        private Window _window;
        private int _screenWidth, _screenHeight, _taskbarHeight;

        public ResolutionIndependentPanel()
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                return;

            Loaded += ResolutionIndependentPanel_Loaded;
        }

        static ResolutionIndependentPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ResolutionIndependentPanel), new FrameworkPropertyMetadata(typeof(ResolutionIndependentPanel)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _content = GetTemplateChild("PART_Content") as ContentControl;
            _gridMain = GetTemplateChild("PART_MainGrid") as Grid;
            _resolutionScaleTransform = GetTemplateChild("PART_ResolutionScaleTransform") as ScaleTransform;
        }

        private void ResolutionIndependentPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (DesignWidth == null || DesignHeight == null)
                return;

            _window = Window.GetWindow(this);
            _window.SizeChanged += window_SizeChanged;

            var helper = new WindowInteropHelper(_window); //this being the wpf form 
            var screen = System.Windows.Forms.Screen.FromHandle(helper.Handle);

            _screenWidth = screen.WorkingArea.Width;
            _screenHeight = screen.WorkingArea.Height;
            _taskbarHeight = screen.Bounds.Height - screen.WorkingArea.Height;

            double widthRatio = _screenWidth / (double)DesignWidth;
            double heightRatio = _screenHeight / ((double)DesignHeight - _taskbarHeight);

            double smallestRatio = widthRatio > heightRatio ? widthRatio : heightRatio;

            _resolutionScaleTransform.ScaleX = smallestRatio;
            _resolutionScaleTransform.ScaleY = smallestRatio;
        }

        private void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < MinWidth || (e.NewSize.Height + _taskbarHeight) < MinHeight)
                return;

            double actualWidth = _window.WindowState == WindowState.Maximized && e.NewSize.Width > _screenWidth ? _screenWidth : e.NewSize.Width;
            double actualHeight = _window.WindowState == WindowState.Maximized && e.NewSize.Height > _screenHeight ? _screenHeight : e.NewSize.Height;

            if (double.IsInfinity(actualWidth) || double.IsInfinity(actualHeight))
                return;

            _gridMain.Width = actualWidth / _resolutionScaleTransform.ScaleX;
            _gridMain.Height = ((actualHeight - _taskbarHeight) / _resolutionScaleTransform.ScaleY);
        }
    }
}