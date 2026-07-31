using System.Windows;
using PRN212.ExamKit.Common;

namespace PRN212.ExamKit.Templates
{
    /// <summary>
    /// Interaction logic for LoginWindowTemplate.xaml.
    /// Copy this class body directly into your generated LoginWindow code-behind.
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Password.Trim();

            // 1. Mandatory input checks
            if (!ValidationHelpers.IsRequired(username) || !ValidationHelpers.IsRequired(password))
            {
                WpfHelpers.ShowError("Please enter both username and password.");
                return;
            }

            // 2. Perform database lookup via Service layer
            // TODO: Replace mock login validation with your database query
            // e.g.: var user = _accountService.Login(username, password);
            bool isValid = (username == "admin" && password == "admin");

            if (isValid)
            {
                WpfHelpers.ShowInfo("Login successful!");
                
                // 3. Open MainWindow and pass user session
                // var main = new MainWindow(user);
                // WpfHelpers.NavigateTo(this, main);
            }
            else
            {
                WpfHelpers.ShowError("Invalid username or password. Please try again.");
            }
        }
    }
}
