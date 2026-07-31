using System.Windows;
using StudyPlannerDataAccess.Models;
using StudyPlannerServices;

namespace StudyPlannerSystem;

public partial class MyProgressWindow : Window
{
    public MyProgressWindow(Student student)
    {
        InitializeComponent();
        LoadProgress(student);
    }

    private void LoadProgress(Student student)
    {
        var studyTaskService = new StudyTaskService();

        int completed = studyTaskService.GetCompletedCount(student.StudentId);
        int total = studyTaskService.GetTotalCount(student.StudentId);
        double rate = studyTaskService.GetCompletionRate(student.StudentId);

        txtStudentName.Text = student.FullName;
        txtCompletedCount.Text = completed.ToString();
        txtTotalCount.Text = total.ToString();
        txtCompletionRate.Text = $"{rate:F2}%";
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
