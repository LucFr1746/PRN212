using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PRN212.ExamKit.Common;

namespace PRN212.ExamKit.Templates
{
    /// <summary>
    /// Interaction logic for CrudWindowTemplate.xaml.
    /// Reusable blueprint for master-details grid listing, dynamic filtering, and multi-relation database insertions.
    /// </summary>
    public partial class CrudWindow : Window
    {
        // TODO: Instantiate your service layers
        // private readonly EmployeeService _employeeService = new EmployeeService();
        // private readonly DepartmentService _departmentService = new DepartmentService();
        // private readonly SkillService _skillService = new SkillService();

        public CrudWindow()
        {
            InitializeComponent();
            LoadInitData();
        }

        private void LoadInitData()
        {
            try
            {
                // 1. Fetch lookup collections
                // var departments = _departmentService.GetAll();
                // var skills = _skillService.GetAll();

                // 2. Bind filter dropdowns with "All" option (Category/Secondary)
                // cboFilterCategory.LoadWithDefault(departments, "DepartmentName", "DepartmentId", () => new Department { DepartmentId = 0, DepartmentName = "All" });
                // cboFilterSecondary.LoadWithDefault(skills, "SkillName", "SkillId", () => new Skill { SkillId = 0, SkillName = "All" });

                // 3. Bind insert form dropdowns (does NOT need "All")
                // cboCategory.ItemsSource = departments;
                // cboCategory.DisplayMemberPath = "DepartmentName";
                // cboCategory.SelectedValuePath = "DepartmentId";
                // cboCategory.SelectedIndex = 0;

                // 4. Bind association listbox (requires partial class IsChecked binding)
                // lstCheckedItems.ItemsSource = skills;

                // 5. Load main DataGrid list
                RefreshGrid();
            }
            catch (Exception ex)
            {
                WpfHelpers.ShowError($"Failed to load initial data: {ex.Message}");
            }
        }

        private void RefreshGrid()
        {
            // TODO: Fetch list using eager loading (.Include) and assign to DataGrid
            // dgRecords.ItemsSource = _employeeService.GetEmployeesWithDetails();
        }

        // ==========================================
        // DYNAMIC FILTER IMPLEMENTATION
        // ==========================================
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int selectedCategoryVal = Convert.ToInt32(cboFilterCategory.SelectedValue);
                int selectedSecondaryVal = Convert.ToInt32(cboFilterSecondary.SelectedValue);

                // Pass values to filter query
                // dgRecords.ItemsSource = _employeeService.Filter(selectedCategoryVal, selectedSecondaryVal);
            }
            catch (Exception ex)
            {
                WpfHelpers.ShowError($"Filtering error: {ex.Message}");
            }
        }

        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset dropdowns back to first index ("All")
            cboFilterCategory.SelectedIndex = 0;
            cboFilterSecondary.SelectedIndex = 0;
            RefreshGrid();
        }

        // ==========================================
        // VALIDATION & TRANSACTION INSERT
        // ==========================================
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Extract inputs
            var name = txtName.Text.Trim();
            var numericText = txtNumericValue.Text.Trim();
            var categoryId = cboCategory.SelectedValue;

            // 2. Perform validations
            if (!ValidationHelpers.IsRequired(name))
            {
                WpfHelpers.ShowError("Name field is required.");
                return;
            }

            if (!ValidationHelpers.TryParseDecimal(numericText, out decimal numericValue) || numericValue <= 0)
            {
                WpfHelpers.ShowError("Numeric value must be a valid number greater than 0.");
                return;
            }

            if (categoryId == null)
            {
                WpfHelpers.ShowError("Please select a valid category.");
                return;
            }

            // 3. Extract Checked Items from ListBox
            // var allSkills = lstCheckedItems.ItemsSource as List<Skill>;
            // var selectedSkills = allSkills?.Where(s => s.IsChecked).ToList() ?? new List<Skill>();

            // if (selectedSkills.Count == 0)
            // {
            //     WpfHelpers.ShowError("Please check at least one association item.");
            //     return;
            // }

            try
            {
                // 4. Save parent entity
                // var employee = new Employee 
                // { 
                //     FullName = name, 
                //     Salary = numericValue, 
                //     DepartmentId = Convert.ToInt32(categoryId) 
                // };
                // _employeeService.Add(employee); // Generates employee.EmployeeId

                // 5. Save bridge entities using generated Parent ID
                // foreach (var skill in selectedSkills)
                // {
                //     var bridge = new EmployeeSkill { EmployeeId = employee.EmployeeId, SkillId = skill.SkillId };
                //     _employeeSkillService.Add(bridge);
                // }

                WpfHelpers.ShowInfo("Record successfully added!");
                
                // 6. Refresh and reset form
                RefreshGrid();
                ResetForm();
            }
            catch (Exception ex)
            {
                WpfHelpers.ShowError($"Database operation failed: {ex.Message}");
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            txtName.Text = string.Empty;
            txtNumericValue.Text = string.Empty;
            cboCategory.SelectedIndex = 0;

            // Uncheck ListBox items
            // var allSkills = lstCheckedItems.ItemsSource as List<Skill>;
            // if (allSkills != null)
            // {
            //     foreach (var s in allSkills) s.IsChecked = false;
            // }
            // lstCheckedItems.Items.Refresh(); // Forces UI to refresh binding states
        }

        private void DgRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Optional: Map selected row to form TextBoxes for Updates
            // var selected = dgRecords.SelectedItem as Employee;
            // if (selected != null)
            // {
            //     txtName.Text = selected.FullName;
            //     txtNumericValue.Text = selected.Salary.ToString();
            //     cboCategory.SelectedValue = selected.DepartmentId;
            // }
        }
    }
}
