using System.Windows;
using System.Windows.Controls;
using StudyPlannerDataAccess.Models;
using StudyPlannerServices;

namespace StudyPlannerSystem;

public partial class SubjectManagementWindow : Window
{
    private readonly ISubjectService _subjectService;

    public SubjectManagementWindow()
    {
        InitializeComponent();
        _subjectService = new SubjectService();
        LoadSubjects();
    }

    private void LoadSubjects()
    {
        dgSubjects.ItemsSource = null;
        dgSubjects.ItemsSource = _subjectService.GetAll();
    }

    private void DgSubjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (dgSubjects.SelectedItem is not Subject selected)
            return;

        txtSubjectCode.Text = selected.SubjectCode;
        txtSubjectName.Text = selected.SubjectName;
    }

    private void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var subject = new Subject
            {
                SubjectCode = txtSubjectCode.Text.Trim(),
                SubjectName = txtSubjectName.Text.Trim()
            };

            _subjectService.Add(subject);
            MessageBox.Show("Subject added successfully.", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
            ClearInputs();
            LoadSubjects();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BtnEdit_Click(object sender, RoutedEventArgs e)
    {
        if (dgSubjects.SelectedItem is not Subject selected)
        {
            MessageBox.Show("Please select a subject to edit.", "Warning",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            selected.SubjectCode = txtSubjectCode.Text.Trim();
            selected.SubjectName = txtSubjectName.Text.Trim();

            _subjectService.Update(selected);
            MessageBox.Show("Subject updated successfully.", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
            ClearInputs();
            LoadSubjects();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (dgSubjects.SelectedItem is not Subject selected)
        {
            MessageBox.Show("Please select a subject to delete.", "Warning",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var result = MessageBox.Show($"Are you sure you want to delete '{selected.SubjectName}'?",
            "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes)
            return;

        try
        {
            _subjectService.Delete(selected.SubjectId);
            MessageBox.Show("Subject deleted successfully.", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
            ClearInputs();
            LoadSubjects();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BtnClear_Click(object sender, RoutedEventArgs e)
    {
        ClearInputs();
    }

    private void ClearInputs()
    {
        txtSubjectCode.Text = string.Empty;
        txtSubjectName.Text = string.Empty;
        dgSubjects.SelectedItem = null;
    }
}
