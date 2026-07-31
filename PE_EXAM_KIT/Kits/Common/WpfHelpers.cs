using System.Windows;

namespace PRN212.ExamKit.Common
{
    /// <summary>
    /// Helper utilities to reduce lengthy WPF boilerplate code for alerts and window transitions.
    /// </summary>
    public static class WpfHelpers
    {
        /// <summary>
        /// Displays an error dialog with an OK button and error icon.
        /// </summary>
        public static void ShowError(string message, string title = "Error")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Displays an information dialog with an OK button and info icon.
        /// </summary>
        public static void ShowInfo(string message, string title = "Success")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Displays a Yes/No confirmation dialog and returns true if the user clicks Yes.
        /// </summary>
        public static bool ShowConfirm(string message, string title = "Confirmation")
        {
            var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        /// <summary>
        /// Transition helper that displays a target window and closes the current window.
        /// </summary>
        /// <param name="current">The window to close (can be null).</param>
        /// <param name="target">The window to show.</param>
        public static void NavigateTo(Window current, Window target)
        {
            if (target == null) return;
            
            target.Show();
            current?.Close();
        }
    }
}
