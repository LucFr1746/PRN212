using Q2.Helpers;
using Q2.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

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
            LoadInitData();
        }

        private void LoadInitData()
        {
            try
            {
                using (var context = new Prn21226sprB11Context())
                {
                    var departments = context.Departments.ToList();
                    var skills = context.Skills.ToList();

                    cboFilterDepartment.LoadWithDefault(departments, "DepartmentName", "DepartmentId",
                        () => new Departments { DepartmentId = 0, DepartmentName = "All" });
                    cboFilterSkill.LoadWithDefault(skills, "SkillName", "SkillId",
                        () => new Skills { SkillId = 0, SkillName = "All" });

                    cboDepartment.ItemsSource = departments;
                    cboDepartment.DisplayMemberPath = "DepartmentName";
                    cboDepartment.SelectedValuePath = "DepartmentId";
                    cboDepartment.SelectedIndex = 0;

                    lstSkills.ItemsSource = skills;
                }
                RefreshGrid();
            }
            catch (Exception ex)
            {
                WpfHelpers.ShowError($"Failed to load initial data: {ex.Message}");
            }
        }

        private void RefreshGrid()
        {
            using (var context = new Prn21226sprB11Context())
            {
                dgEmployees.ItemsSource = context.Employees
                    .Include(x => x.Department)
                    .ToList();
            }
        }

        // ==========================================
        // DYNAMIC FILTER IMPLEMENTATION
        // ==========================================
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int departmentId = Convert.ToInt32(cboFilterDepartment.SelectedValue);
                int skillId = Convert.ToInt32(cboFilterSkill.SelectedValue);

                using (var context = new Prn21226sprB11Context())
                {
                    IQueryable<Employees> query = context.Employees
                        .Include(x => x.Department)
                        .Include(x => x.Skill);

                    if (departmentId > 0)
                        query = query.Where(x => x.DepartmentId == departmentId);

                    if (skillId > 0)
                        query = query.Where(x => x.Skill.Any(b => b.SkillId == skillId));

                    dgEmployees.ItemsSource = query.ToList();
                }
            }
            catch (Exception ex)
            {
                WpfHelpers.ShowError($"Filtering error: {ex.Message}");
            }
        }

        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset dropdowns back to first index ("All")
            cboFilterDepartment.SelectedIndex = 0;
            cboFilterSkill.SelectedIndex = 0;
            RefreshGrid();
        }

        // ==========================================
        // VALIDATION & TRANSACTION INSERT
        // ==========================================
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Đọc input
            var fullName = txtFullName.Text.Trim();
            var email = txtEmail.Text.Trim();
            var salary = txtSalary.Text.Trim();
            var departmentId = cboDepartment.SelectedValue;

            // 2. Validate
            if (!ValidationHelpers.IsRequired(fullName)) { WpfHelpers.ShowError("Fullname is required."); return; }
            if (!ValidationHelpers.IsRequired(fullName) || !ValidationHelpers.IsValidEmail(email)) { WpfHelpers.ShowError("Please enter a valid email."); return; }
            if (!ValidationHelpers.TryParseDecimal(salary, out decimal salaryValue) || salaryValue <= 0)
            { WpfHelpers.ShowError("Invalid number."); return; }
            if (departmentId == null) { WpfHelpers.ShowError("Select a category."); return; }

            // 3. Validate CheckedListBox
            var allPhu = lstSkills.ItemsSource as List<Skills>;
            var selectedPhu = allPhu?.Where(x => x.IsChecked).ToList() ?? new List<Skills>();
            if (selectedPhu.Count == 0) { WpfHelpers.ShowError("Select at least one item."); return; }

            try
            {
                using (var context = new Prn21226sprB11Context())
                {
                    // 4. Tạo entity chính
                    var employee = new Employees
                    {
                        FullName = fullName,
                        Email = email,
                        Salary = salaryValue,
                        HireDate = DateOnly.FromDateTime(DateTime.Today),
                        DepartmentId = Convert.ToInt32(departmentId)
                    };
                    context.Employees.Add(employee);
                    context.SaveChanges();

                    // 5. Tạo bản ghi bridge (many-to-many)
                    foreach (var phu in selectedPhu)
                    {
                        var dbSkill = context.Skills.Find(phu.SkillId);
                        if (dbSkill != null)
                        {
                            employee.Skill.Add(dbSkill);
                        }
                    }
                    context.SaveChanges();
                }

                WpfHelpers.ShowInfo("Added successfully!");
                RefreshGrid();
                ResetForm();
            }
            catch (Exception ex) { WpfHelpers.ShowError($"Error: {ex.Message}"); }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            txtFullName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtSalary.Text = string.Empty;
            cboDepartment.SelectedIndex = 0;

            var allPhu = lstSkills.ItemsSource as List<Skills>;
            if (allPhu != null) foreach (var x in allPhu) x.IsChecked = false;
            lstSkills.Items.Refresh();
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