﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DayZLootEdit
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LootTable LootTable;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            if (dlg.ShowDialog().Value)
            {
                LootTable = new LootTable(dlg.FileName);
                LootTable.LoadFile();

                LootList.ItemsSource = LootTable.Loot;

                SaveBtn.IsEnabled = true;
            }
        }

        private void PercBtn_Click(object sender, RoutedEventArgs e)
        {
            int percentage = 0;
            bool ok = int.TryParse(PercBox.Text.Replace("%", ""), out percentage);
            if (!ok) return;

            foreach (LootType loot in LootList.SelectedItems)
            {
                loot.SetNominal(percentage);
            }

            LootList.Items.Refresh();
        }

        private void PercSilder_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PercBox.Text = Math.Round(PercSilder.Value).ToString();
            UpdatePercValue();
        }
        
        private void PercBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PercBox.SelectAll();
        }

        private void PercBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdatePercValue();
        }

        private void PercBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdatePercValue();
            }
        }
        
        private void UpdatePercValue()
        {
            int newval = 0;
            bool ok = int.TryParse(PercBox.Text.Replace("%", ""), out newval);

            if (ok)
            {
                PercSilder.Value = newval;
                PercBox.Text = String.Format("{0}%", newval);
                PercBtn?.Focus();
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            LootTable.SaveFile();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = TbFilter.Text;

            if(!string.IsNullOrEmpty(filterText) && filterText.Length > 2)
            {
                LootTable.Filter(filterText);
                LootList.ItemsSource = LootTable.FilteredLoot;
            }
            else
            {
                LootList.ItemsSource = LootTable.Loot;
            }
        }

        private void MinBtn_Click(object sender, RoutedEventArgs e)
        {
            int percentage = 0;
            bool ok = int.TryParse(PercBox.Text.Replace("%", ""), out percentage);
            if (!ok) return;

            foreach (LootType loot in LootList.SelectedItems)
            {
                loot.SetMin(percentage);
            }

            LootList.Items.Refresh();
        }

        private void BothBtn_Click(object sender, RoutedEventArgs e)
        {
            int percentage = 0;
            bool ok = int.TryParse(PercBox.Text.Replace("%", ""), out percentage);
            if (!ok) return;

            foreach (LootType loot in LootList.SelectedItems)
            {
                loot.SetMin(percentage);
                loot.SetNominal(percentage);
            }

            LootList.Items.Refresh();
        }

        private void BtnDisable_Click(object sender, RoutedEventArgs e)
        {
            foreach (LootType loot in LootList.SelectedItems)
            {
                loot.SetMin(0);
                loot.SetNominal(0);
            }

            LootList.Items.Refresh();
        }
    }
}
