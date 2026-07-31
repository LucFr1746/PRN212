using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace PRN212.ExamKit.Common
{
    /// <summary>
    /// Reusable WPF ComboBox extension methods to eliminate boilerplate selection binding.
    /// </summary>
    public static class ComboBoxExtensions
    {
        /// <summary>
        /// Binds a lookup list to a WPF ComboBox and prepends a default selection item (e.g., "All" or "-- Select --").
        /// </summary>
        /// <typeparam name="T">The collection item type.</typeparam>
        /// <param name="comboBox">Target ComboBox control.</param>
        /// <param name="items">List of database-retrieved lookup entities.</param>
        /// <param name="displayPath">The property to display in the UI (e.g., "CategoryName").</param>
        /// <param name="valuePath">The backing identifier property (e.g., "CategoryId").</param>
        /// <param name="createDefaultItem">Delegate returning the custom default entity (e.g., () => new Category { CategoryId = 0, CategoryName = "All" }).</param>
        public static void LoadWithDefault<T>(
            this ComboBox comboBox,
            List<T> items,
            string displayPath,
            string valuePath,
            Func<T> createDefaultItem = null) where T : class
        {
            if (comboBox == null) throw new ArgumentNullException(nameof(comboBox));
            if (items == null) throw new ArgumentNullException(nameof(items));

            var bindList = new List<T>();
            
            // Insert custom default option at index 0 (e.g., CategoryId = 0, CategoryName = "All")
            if (createDefaultItem != null)
            {
                var defaultItem = createDefaultItem();
                if (defaultItem != null)
                {
                    bindList.Add(defaultItem);
                }
            }

            bindList.AddRange(items);

            comboBox.ItemsSource = bindList;
            comboBox.DisplayMemberPath = displayPath;
            comboBox.SelectedValuePath = valuePath;
            
            // Default selection set to first index
            if (bindList.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }
    }
}
