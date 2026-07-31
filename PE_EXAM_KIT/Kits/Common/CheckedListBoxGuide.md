# WPF CheckedListBox Binding Recipe

Since WPF does not have a native `CheckedListBox` control, use this recipe to bind a checklist of database entities (e.g. `Skills` or `Suppliers`) using a standard `ListBox`.

---

## Step 1: Create a Partial Class for the Entity
Create a file named `PartialEntities.cs` in your **DataAccess** or **Models** folder. Declaring an `IsChecked` property in a partial class makes it bindable in WPF without saving it to SQL Server.

```csharp
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models // Use the exact namespace of your scaffolded models
{
    public partial class Skill // Replace with your entity class name (e.g., Supplier, Skill)
    {
        [NotMapped] // Prevents EF Core from trying to map this to the DB
        public bool IsChecked { get; set; }
    }
}
```

---

## Step 2: Define the ListBox in XAML
Add a `ListBox` inside your view grid. Use an `ItemTemplate` containing a `CheckBox` bound to the partial class properties:

```xml
<ListBox x:Name="lstSkills" Height="150" Margin="5" SelectionMode="Multiple">
    <ListBox.ItemTemplate>
        <DataTemplate>
            <CheckBox Content="{Binding SkillName}" 
                      IsChecked="{Binding IsChecked, Mode=TwoWay}"
                      Margin="2"/>
        </DataTemplate>
    </ListBox.ItemTemplate>
</ListBox>
```
*Note: Make sure to change `Content="{Binding SkillName}"` to whatever text column you want to display (e.g. `SupplierName`).*

---

## Step 3: Load the Data in Code-Behind
Bind the list of entities from your service/database directly to the `ItemsSource` of the ListBox:

```csharp
private void LoadSkills()
{
    // Retrieve skills list from DB
    List<Skill> skills = _skillService.GetAll();
    
    // Bind to listbox (each item now has IsChecked defaulted to false)
    lstSkills.ItemsSource = skills;
}
```

---

## Step 4: Extract Checked Items on Insert
When the user clicks "Add", retrieve the selected items by filtering the bound collection:

```csharp
private void AddButton_Click(object sender, RoutedEventArgs e)
{
    // 1. Get the list of all items from ItemsSource
    var allSkills = lstSkills.ItemsSource as List<Skill>;
    if (allSkills == null) return;

    // 2. Filter for checked items
    var selectedSkills = allSkills.Where(s => s.IsChecked).ToList();

    if (selectedSkills.Count == 0)
    {
        MessageBox.Show("Please select at least one skill.");
        return;
    }

    // 3. Insert main entity and save
    var employee = new Employee { FullName = txtName.Text };
    _employeeService.Add(employee); // Saves to DB and populates employee.EmployeeId

    // 4. Insert junction records using generated ID
    foreach (var skill in selectedSkills)
    {
        var junction = new EmployeeSkill 
        { 
            EmployeeId = employee.EmployeeId, 
            SkillId = skill.SkillId 
        };
        _junctionService.Add(junction);
    }
    
    MessageBox.Show("Employee and skills added successfully!");
}
```
