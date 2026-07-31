using System.Windows;
using StudyPlannerDataAccess.Models;

namespace StudyPlannerSystem;

public partial class MainWindow : Window
{
    private readonly Student _loggedInStudent;

    public MainWindow(Student student)
    {
        InitializeComponent();
        _loggedInStudent = student;
        Title = $"Study Planner - Welcome, {_loggedInStudent.FullName}";
    }

    private void MenuStudyTasks_Click(object sender, RoutedEventArgs e)
    {
        var window = new StudyTaskManagementWindow(_loggedInStudent);
        window.ShowDialog();
    }

    private void MenuSubjects_Click(object sender, RoutedEventArgs e)
    {
        var window = new SubjectManagementWindow();
        window.ShowDialog();
    }

    private void MenuMyProgress_Click(object sender, RoutedEventArgs e)
    {
        var window = new MyProgressWindow(_loggedInStudent);
        window.ShowDialog();
    }

    private void MenuLogout_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show("Are you sure you want to logout?",
            "Logout Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes)
            return;

        var loginWindow = new LoginWindow();
        loginWindow.Show();
        Close();
    }
}