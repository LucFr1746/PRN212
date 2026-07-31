using System.Windows;
using StudyPlannerServices;

namespace StudyPlannerSystem;

public partial class LoginWindow : Window
{
    private readonly IStudentService _studentService;

    public LoginWindow()
    {
        InitializeComponent();
        _studentService = new StudentService();
    }

    private void BtnLogin_Click(object sender, RoutedEventArgs e)
    {
        string studentCode = txtStudentCode.Text.Trim();
        string password = txtPassword.Password;

        if (string.IsNullOrWhiteSpace(studentCode) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("Please enter Student Code and Password.",
                "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var student = _studentService.Authenticate(studentCode, password);
        if (student == null)
        {
            MessageBox.Show("Invalid StudentCode or Password.",
                "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var mainWindow = new MainWindow(student);
        mainWindow.Show();
        Close();
    }
}
