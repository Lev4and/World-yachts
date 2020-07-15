﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace World_yachts.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ExpanderWithCheckBoxes.xaml
    /// </summary>
    public partial class ExpanderWithCheckBoxes : UserControl
    {
        public ObservableCollection<string> Data
        {
            get { return (ObservableCollection<string>)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(ObservableCollection<string>), typeof(ExpanderWithCheckBoxes), new PropertyMetadata(null, Data_Changed));

        private static void Data_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as ExpanderWithCheckBoxes;

            if (current != null)
            {
                current.RemoveContorls();
                current.GenerateItems();
                current.RenderContorls();
            }
        }

        public List<CheckBox> Items
        {
            get { return (List<CheckBox>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(List<CheckBox>), typeof(ExpanderWithCheckBoxes), new PropertyMetadata(null));

        public ObservableCollection<string> ListSelectedValues
        {
            get { return (ObservableCollection<string>)GetValue(ListSelectedValuesProperty); }
            set { SetValue(ListSelectedValuesProperty, value); }
        }

        public static readonly DependencyProperty ListSelectedValuesProperty =
            DependencyProperty.Register("ListSelectedValues", typeof(ObservableCollection<string>), typeof(ExpanderWithCheckBoxes), new PropertyMetadata(null));

        public string ElementHeader
        {
            get { return (string)GetValue(ElementHeaderProperty); }
            set { SetValue(ElementHeaderProperty, value); }
        }

        public static readonly DependencyProperty ElementHeaderProperty =
            DependencyProperty.Register("ElementHeader", typeof(string), typeof(ExpanderWithCheckBoxes), new PropertyMetadata(""));

        public ExpanderWithCheckBoxes()
        {
            InitializeComponent();

            Items = new List<CheckBox>();
            ListSelectedValues = new ObservableCollection<string>();
        }

        private void GenerateItems()
        {
            Items.Clear();

            for (int i = 0; i < Data.Count; i++)
            {
                var checkBox = new CheckBox();
                checkBox.VerticalAlignment = VerticalAlignment.Top;
                checkBox.HorizontalAlignment = HorizontalAlignment.Left;
                checkBox.Margin = new Thickness(10, 10 + (i * 30), 0, 0);
                checkBox.IsChecked = false;
                checkBox.Content = Data[i];
                checkBox.Checked += Item_Checked;
                checkBox.Unchecked += Item_Unchecked;
                Items.Add(checkBox);
            }
        }

        private void Item_Checked(object sender, RoutedEventArgs e)
        {
            if (!ListSelectedValues.Contains((sender as CheckBox).Content.ToString()))
                ListSelectedValues.Add((sender as CheckBox).Content.ToString());
        }

        private void Item_Unchecked(object sender, RoutedEventArgs e)
        {
            ListSelectedValues.Remove((sender as CheckBox).Content.ToString());
        }

        private void RemoveContorls()
        {
            while (grid.Children.Count > 0)
                grid.Children.Clear();
        }

        private void RenderContorls()
        {
            foreach (var item in Items)
                grid.Children.Add(item);
        }
    }
}