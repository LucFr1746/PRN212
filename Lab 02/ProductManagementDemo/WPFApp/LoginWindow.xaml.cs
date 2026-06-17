using System.Windows;
using System.Windows.Input;
using BusinessObjects;
using Services;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly IAccountService iAccountService;

        public LoginWindow()
        {
            InitializeComponent();
            iAccountService = new AccountService();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccountMember? account = iAccountService.GetAccountById(txtUser.Text);
                if (account != null && account.MemberPassword.Equals(txtPass.Password)
                    && account.MemberRole == 1)
                {
                    this.Hide();
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                }
                else
                {
                    MessageBox.Show("You are not permission !");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Database/Connection Error: " + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace, "Database Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
