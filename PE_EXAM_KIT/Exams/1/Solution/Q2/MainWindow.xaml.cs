using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Q2.Models;
using Q2.Helpers;

namespace Q2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                using (var context = new Prn21226sprB11Context())
                {
                    // 1. Load departments and skills collections
                    var departments = context.Departments.ToList();
                    var skills = context.Skills.ToList();

                    // 2. Load filter ComboBoxes using ComboBoxExtensions
                    cboFilterDept.LoadWithDefault(
                        departments, 
                        "DepartmentName", 
                        "DepartmentId", 
                        () => new Departments { DepartmentId = 0, DepartmentName = "All" }
                    );

                    cboFilterSkill.LoadWithDefault(
                        skills, 
                        "SkillName", 
                        "SkillId", 
                        () => new Skills { SkillId = 0, SkillName = "All" }
                    );

                    // 3. Load insert form ComboBox
                    cboDept.ItemsSource = departments;
                    cboDept.DisplayMemberPath = "DepartmentName";
                    cboDept.SelectedValuePath = "DepartmentId";
                    cboDept.SelectedIndex = 0;

                    // 4. Load CheckedListBox ListBox
                    lstSkills.ItemsSource = skills;
                }

                // 5. Display employees in Grid
                RefreshGrid();
            }
            catch (Exception ex)
            {
                WpfHelpers.ShowError($"Failed to initialize data: {ex.Message}");
            }
        }

        private void RefreshGrid()
        {
            try
            {
                using (var context = new Prn21226sprB11Context())
                {
                    // Include Department lookup reference to display DepartmentName
                    dgEmployees.ItemsSource = context.Employees
                        .Include(e => e.Department)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                WpfHelpers.ShowError($"Failed to load employees: {ex.Message}");
            }
        }

        // ==========================================
        // DYNAMIC FILTER IMPLEMENTATION
        // ==========================================
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int selectedDeptId = Convert.ToInt32(cboFilterDept.SelectedValue);
                int selectedSkillId = Convert.ToInt32(cboFilterSkill.SelectedValue);

                using (var context = new Prn21226sprB11Context())
                {
                    IQueryable<Employees> query = context.Employees
                        .Include(e => e.Department)
                        .Include(e => e.Skill);

                    // A. Filter by Department
                    if (selectedDeptId > 0)
                    {
                        query = query.Where(e => e.DepartmentId == selectedDeptId);
                    }

                    // B. Filter by Skill
                    if (selectedSkillId > 0)
                    {
                        query = query.Where(e => e.Skill.Any(s => s.SkillId == selectedSkillId));
                    }

                    dgEmployees.ItemsSource = query.ToList();
                }
            }
            catch (Exception ex)
            {
                WpfHelpers.ShowError($"Failed to filter: {ex.Message}");
            }
        }

        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            cboFilterDept.SelectedIndex = 0;
            cboFilterSkill.SelectedIndex = 0;
            RefreshGrid();
        }

        // ==========================================
        // VALIDATION & TRANSACTION INSERT
        // ==========================================
        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string salaryText = txtSalary.Text.Trim();
            object deptIdVal = cboDept.SelectedValue;

            // 1. Run validations
            if (!ValidationHelpers.IsRequired(fullName))
            {
                WpfHelpers.ShowError("Full name is required.");
                return;
            }

            if (!ValidationHelpers.IsRequired(email) || !ValidationHelpers.IsValidEmail(email))
            {
                WpfHelpers.ShowError("A valid email address is required.");
                return;
            }

            if (!ValidationHelpers.TryParseDecimal(salaryText, out decimal salary) || salary <= 0)
            {
                WpfHelpers.ShowError("Salary must be a positive numeric value.");
                return;
            }

            if (deptIdVal == null)
            {
                WpfHelpers.ShowError("Please select a department.");
                return;
            }

            // 2. Extract Checked items
            var allSkills = lstSkills.ItemsSource as List<Skills>;
            var selectedSkills = allSkills?.Where(s => s.IsChecked).ToList() ?? new List<Skills>();

            if (selectedSkills.Count == 0)
            {
                WpfHelpers.ShowError("Please select at least 1 skill.");
                return;
            }

            try
            {
                using (var context = new Prn21226sprB11Context())
                {
                    var employee = new Employees
                    {
                        FullName = fullName,
                        Email = email,
                        Salary = salary,
                        DepartmentId = Convert.ToInt32(deptIdVal),
                        HireDate = DateOnly.FromDateTime(DateTime.Today)
                    };

                    // Attach skills to the context and link them to the employee
                    foreach (var s in selectedSkills)
                    {
                        context.Entry(s).State = EntityState.Unchanged; // Ensure EF tracks the skill as existing
                        employee.Skill.Add(s);
                    }

                    context.Employees.Add(employee);
                    context.SaveChanges();
                }

                WpfHelpers.ShowInfo("Employee and skills added successfully!");
                RefreshGrid();
                ResetForm();
            }
            catch (Exception ex)
            {
                WpfHelpers.ShowError($"Database insert failed: {ex.Message}");
            }
        }

        private void ClearFormButton_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            txtFullName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtSalary.Text = string.Empty;
            
            if (cboDept.Items.Count > 0)
            {
                cboDept.SelectedIndex = 0;
            }

            var allSkills = lstSkills.ItemsSource as List<Skills>;
            if (allSkills != null)
            {
                foreach (var s in allSkills) s.IsChecked = false;
            }
            lstSkills.Items.Refresh();
        }
    }
}