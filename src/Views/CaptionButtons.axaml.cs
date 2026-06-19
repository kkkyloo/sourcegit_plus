using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;

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

            if (OperatingSystem.IsWindows())
                return;

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
            _pressedPointer = e.Pointer;
            e.Pointer.Capture(button);

            if (OperatingSystem.IsWindows())
                StartWindowsReleaseFallback();

            e.Handled = true;
        }

        private void CompleteCaptionAction(Button button, CaptionAction action, PointerReleasedEventArgs e)
        {
            if (_pressedButton != button || _pressedAction != action)
            {
                ResetPressedAction(true);
                return;
            }

            var position = e.GetPosition(button);
            var isInside = IsPointInsideButton(button, position);

            ResetPressedAction(true);

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
            if (!OperatingSystem.IsWindows() || _releaseFallbackTimer == null)
                ResetPressedAction(true);

            e.Handled = true;
        }

        private static bool IsPointInsideButton(Button button, Point position)
        {
            return position.X >= 0 &&
                position.Y >= 0 &&
                position.X < button.Bounds.Width &&
                position.Y < button.Bounds.Height;
        }

        private void ResetPressedAction(bool stopFallbackTimer)
        {
            if (stopFallbackTimer)
                StopWindowsReleaseFallback();

            var pointer = _pressedPointer;
            _pressedPointer = null;
            pointer?.Capture(null);

            _pressedButton = null;
            _pressedAction = CaptionAction.None;
        }

        [SupportedOSPlatform("windows")]
        private void StartWindowsReleaseFallback()
        {
            StopWindowsReleaseFallback();

            _releaseFallbackTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
            _releaseFallbackTimer.Tick += (_, _) =>
            {
                if (IsLeftMouseButtonPressed())
                    return;

                var button = _pressedButton;
                var action = _pressedAction;
                var isInside = button != null && IsCursorInsideButton(button);

                ResetPressedAction(true);

                if (isInside)
                    ExecuteCaptionAction(action);
            };
            _releaseFallbackTimer.Start();
        }

        private void StopWindowsReleaseFallback()
        {
            if (_releaseFallbackTimer == null)
                return;

            _releaseFallbackTimer.Stop();
            _releaseFallbackTimer = null;
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

        [SupportedOSPlatform("windows")]
        private static bool IsLeftMouseButtonPressed()
        {
            return (GetAsyncKeyState(VK_LBUTTON) & 0x8000) != 0;
        }

        [SupportedOSPlatform("windows")]
        private static bool IsCursorInsideButton(Button button)
        {
            if (!GetCursorPos(out var cursor))
                return false;

            var topLeft = button.PointToScreen(new Point(0, 0));
            var bottomRight = button.PointToScreen(new Point(button.Bounds.Width, button.Bounds.Height));

            return cursor.X >= topLeft.X &&
                cursor.Y >= topLeft.Y &&
                cursor.X < bottomRight.X &&
                cursor.Y < bottomRight.Y;
        }

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Win32Point point);

        [StructLayout(LayoutKind.Sequential)]
        private struct Win32Point
        {
            public int X;
            public int Y;
        }

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
        private const int VK_LBUTTON = 0x01;

        private Button _pressedButton = null;
        private IPointer _pressedPointer = null;
        private CaptionAction _pressedAction = CaptionAction.None;
        private DispatcherTimer _releaseFallbackTimer = null;
    }
}
