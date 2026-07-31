using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Q2.Helpers
{
    public static class ComboBoxExtensions
    {
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
            
            if (bindList.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }
    }
}
