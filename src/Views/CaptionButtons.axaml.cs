using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace SourceGit.Views
{
    public partial class CaptionButtons : UserControl
    {
        public static readonly StyledProperty<bool> IsCloseButtonOnlyProperty =
            AvaloniaProperty.Register<CaptionButtons, bool>(nameof(IsCloseButtonOnly));

        public bool IsCloseButtonOnly
        {
            get => GetValue(IsCloseButtonOnlyProperty);
            set => SetValue(IsCloseButtonOnlyProperty, value);
        }

        public CaptionButtons()
        {
            InitializeComponent();

            var routes = RoutingStrategies.Bubble;
            BtnMinimize.AddHandler(InputElement.PointerPressedEvent, MinimizeWindow, routes, true);
            BtnMaximizeOrRestore.AddHandler(InputElement.PointerPressedEvent, MaximizeOrRestoreWindow, routes, true);
            BtnClose.AddHandler(InputElement.PointerPressedEvent, CloseWindow, routes, true);
        }

        private void MinimizeWindow(object _, PointerPressedEventArgs e)
        {
            if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                return;

            var window = TopLevel.GetTopLevel(this) as Window;
            if (window != null)
                window.WindowState = WindowState.Minimized;

            e.Handled = true;
        }

        private void MaximizeOrRestoreWindow(object _, PointerPressedEventArgs e)
        {
            if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                return;

            var window = TopLevel.GetTopLevel(this) as Window;
            if (window != null)
                window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

            e.Handled = true;
        }

        private void CloseWindow(object _, PointerPressedEventArgs e)
        {
            if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                return;

            var window = TopLevel.GetTopLevel(this) as Window;
            window?.Close();

            e.Handled = true;
        }
    }
}
