using System.Windows;
using StudyPlannerDataAccess.Models;
using StudyPlannerServices;

namespace StudyPlannerSystem;

public partial class StudyTaskManagementWindow : Window
{
    private readonly IStudyTaskService _studyTaskService;
    private readonly Student _loggedInStudent;

    public StudyTaskManagementWindow(Student student)
    {
        InitializeComponent();
        _loggedInStudent = student;
        _studyTaskService = new StudyTaskService();
        txtHeader.Text = $"My Study Tasks - {_loggedInStudent.FullName}";
        LoadTasks();
    }

    private void LoadTasks()
    {
        dgStudyTasks.ItemsSource = null;
        dgStudyTasks.ItemsSource = _studyTaskService.GetByStudentId(_loggedInStudent.StudentId);
    }
}
