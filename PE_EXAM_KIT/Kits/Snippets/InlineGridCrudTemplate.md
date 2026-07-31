# WPF DataGrid Inline CRUD Recipe (Exam 5 Style)

In some exams (e.g. Exam 5), you are required to allow users to add new rows, edit cells, and delete items directly in the DataGrid, and then save all changes in bulk using a "Save Changes" button. 

Here is the fastest way to implement this using Entity Framework Core's **Local Tracking Engine**.

---

## Step 1: Configure the DataGrid in XAML
Set `CanUserAddRows="True"` and `CanUserDeleteRows="True"` to let users type new records in the grid and press the `Delete` key to remove them.

```xml
<Grid Margin="10">
    <Grid.RowDefinitions>
        <RowDefinition Height="*"/>
        <RowDefinition Height="Auto"/>
    </Grid.RowDefinitions>

    <!-- DataGrid with direct editing enabled -->
    <DataGrid x:Name="dgSubjects" AutoGenerateColumns="False" CanUserAddRows="True" CanUserDeleteRows="True">
        <DataGrid.Columns>
            <DataGridTextColumn Header="Subject Code" Binding="{Binding SubjectCode, UpdateSourceTrigger=LostFocus}" Width="150"/>
            <DataGridTextColumn Header="Subject Name" Binding="{Binding SubjectName, UpdateSourceTrigger=LostFocus}" Width="*"/>
        </DataGrid.Columns>
    </DataGrid>

    <!-- Controls -->
    <StackPanel Grid.Row="1" Orientation="Horizontal" HorizontalAlignment="Right" Margin="0,10,0,0">
        <Button Content="Refresh" Click="RefreshButton_Click" Width="80" Margin="0,0,5,0" Padding="5"/>
        <Button Content="Save Changes" Click="SaveButton_Click" Width="100" Margin="0,0,5,0" Padding="5" Background="#10B981" Foreground="White" FontWeight="Bold"/>
        <Button Content="Delete Row" Click="DeleteButton_Click" Width="90" Padding="5" Background="#EF4444" Foreground="White"/>
    </StackPanel>
</Grid>
```

---

## Step 2: Code-Behind Setup (Local Binding)
Use the `.Local` property of the EF DbContext `DbSet`. When you bind `ItemsSource` to the local collection, EF Core automatically updates the context state as the user types, edits, or deletes rows in the DataGrid.

```csharp
using System;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models; // Replace with your model namespace

namespace WpfApp.Views
{
    public partial class SubjectManagementView : UserControl
    {
        private MyDbContext _context;

        public SubjectManagementView()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Instantiate a new tracking context (resets tracker state)
                _context = new MyDbContext();
                
                // Load items into EF memory (Requires: using Microsoft.EntityFrameworkCore;)
                _context.Subjects.Load();
                
                // Bind DataGrid directly to EF Local tracker collection
                dgSubjects.ItemsSource = _context.Subjects.Local.ToObservableCollection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load subjects: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Persists all inline additions, edits, and deletions to DB in one transaction
                _context.SaveChanges();
                MessageBox.Show("All changes saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Save failed: {ex.Message}\nMake sure code and names are unique and not empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // If the user selected a row, delete it from the grid (which removes it from EF Local tracking)
            var selected = dgSubjects.SelectedItem as Subject;
            if (selected == null)
            {
                MessageBox.Show("Please select a subject to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete {selected.SubjectCode}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // Removes from EF collection immediately
                _context.Subjects.Remove(selected);
            }
        }
    }
}
```
