using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

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
            BtnMinimize.AddHandler(InputElement.PointerPressedEvent, (_, e) => BeginCaptionAction(BtnMinimize, CaptionAction.Minimize, e), routes, true);
            BtnMinimize.AddHandler(InputElement.PointerReleasedEvent, (_, e) => CompleteCaptionAction(BtnMinimize, CaptionAction.Minimize, e), routes, true);
            BtnMinimize.AddHandler(InputElement.PointerCaptureLostEvent, OnPointerCaptureLost, routes, true);

            BtnMaximizeOrRestore.AddHandler(InputElement.PointerPressedEvent, (_, e) => BeginCaptionAction(BtnMaximizeOrRestore, CaptionAction.MaximizeOrRestore, e), routes, true);
            BtnMaximizeOrRestore.AddHandler(InputElement.PointerReleasedEvent, (_, e) => CompleteCaptionAction(BtnMaximizeOrRestore, CaptionAction.MaximizeOrRestore, e), routes, true);
            BtnMaximizeOrRestore.AddHandler(InputElement.PointerCaptureLostEvent, OnPointerCaptureLost, routes, true);

            BtnClose.AddHandler(InputElement.PointerPressedEvent, (_, e) => BeginCaptionAction(BtnClose, CaptionAction.Close, e), routes, true);
            BtnClose.AddHandler(InputElement.PointerReleasedEvent, (_, e) => CompleteCaptionAction(BtnClose, CaptionAction.Close, e), routes, true);
            BtnClose.AddHandler(InputElement.PointerCaptureLostEvent, OnPointerCaptureLost, routes, true);
        }

        private void BeginCaptionAction(Button button, CaptionAction action, PointerPressedEventArgs e)
        {
            if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                return;

            _pressedButton = button;
            _pressedAction = action;
            e.Pointer.Capture(button);

            e.Handled = true;
        }

        private void CompleteCaptionAction(Button button, CaptionAction action, PointerReleasedEventArgs e)
        {
            if (_pressedButton != button || _pressedAction != action)
            {
                e.Pointer.Capture(null);
                ResetPressedAction();
                return;
            }

            var position = e.GetPosition(button);
            var isInside = position.X >= 0 &&
                position.Y >= 0 &&
                position.X < button.Bounds.Width &&
                position.Y < button.Bounds.Height;

            e.Pointer.Capture(null);
            ResetPressedAction();

            if (isInside)
                ExecuteCaptionAction(action);

            e.Handled = true;
        }

        private void ExecuteCaptionAction(CaptionAction action)
        {
            var window = TopLevel.GetTopLevel(this) as Window;
            if (window == null)
                return;

            if (OperatingSystem.IsWindows() && TrySendWindowsSystemCommand(window, action))
                return;

            switch (action)
            {
                case CaptionAction.Minimize:
                    window.WindowState = WindowState.Minimized;
                    break;
                case CaptionAction.MaximizeOrRestore:
                    window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                    break;
                case CaptionAction.Close:
                    window.Close();
                    break;
            }
        }

        private void OnPointerCaptureLost(object _, PointerCaptureLostEventArgs e)
        {
            ResetPressedAction();
            e.Handled = true;
        }

        private void ResetPressedAction()
        {
            _pressedButton = null;
            _pressedAction = CaptionAction.None;
        }

        [SupportedOSPlatform("windows")]
        private static bool TrySendWindowsSystemCommand(Window window, CaptionAction action)
        {
            var handle = window.TryGetPlatformHandle()?.Handle ?? IntPtr.Zero;
            if (handle == IntPtr.Zero)
                return false;

            var command = action switch
            {
                CaptionAction.Minimize => SC_MINIMIZE,
                CaptionAction.MaximizeOrRestore => window.WindowState == WindowState.Maximized ? SC_RESTORE : SC_MAXIMIZE,
                CaptionAction.Close => SC_CLOSE,
                _ => 0,
            };

            if (command == 0)
                return false;

            SendMessage(handle, WM_SYSCOMMAND, new IntPtr(command), IntPtr.Zero);
            return true;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        private enum CaptionAction
        {
            None,
            Minimize,
            MaximizeOrRestore,
            Close,
        }

        private const uint WM_SYSCOMMAND = 0x0112;
        private const int SC_CLOSE = 0xF060;
        private const int SC_MAXIMIZE = 0xF030;
        private const int SC_MINIMIZE = 0xF020;
        private const int SC_RESTORE = 0xF120;

        private Button _pressedButton = null;
        private CaptionAction _pressedAction = CaptionAction.None;
    }
}
