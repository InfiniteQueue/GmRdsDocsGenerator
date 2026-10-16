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
using GmRdsDocsGenerator.Models;
using Tecnomatix.Engineering;
using ObjCategories = GmRdsDocsGenerator.Models.CategoryRegistry.ObjCategories;

namespace GmRdsDocsGenerator.DebugClasses
{
    /// <summary>
    /// Lists every object assigned to each category of the DocumentJobModel's view state CategoryRegistry
    /// </summary>
    public partial class DebugCategoryViewControl : UserControl
    {
        public DebugCategoryViewControl()
        {
            InitializeComponent();
            Loaded += (s, e) => RefreshDisplay();
        }

        internal DocumentJobModel JobModel
        {
            get => this.DataContext as DocumentJobModel;
            set => this.DataContext = value;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshDisplay();
        }

        public void RefreshDisplay()
        {
            var registry = JobModel?.ViewStates?.Categories;
            if (registry == null)
            {
                treeCategories.ItemsSource = null;
                txtStatus.Text = JobModel == null ? "No DocumentJobModel in DataContext" : "No category registry yet, build the document view state first";
                return;
            }

            var groups = Enum.GetValues(typeof(ObjCategories)).Cast<ObjCategories>()
                .Select(category => new DebugCategoryGroup(category.ToString(), registry[category].Select(DescribeObject).ToList()))
                .ToList();

            treeCategories.ItemsSource = groups;
            txtStatus.Text = $"{groups.Sum(x => x.Items.Count)} assigned objects, refreshed {DateTime.Now:HH:mm:ss}";
        }

        static string DescribeObject(ITxObject obj)
        {
            if (obj == null) return "(null)";
            try
            {
                return $"{obj.Name}  [{obj.GetType().Name}]";
            }
            catch (Exception ex) // e.g. the object was deleted from the study
            {
                return $"(unreadable {obj.GetType().Name}: {ex.GetType().Name})";
            }
        }
    }

    public class DebugCategoryGroup
    {
        public DebugCategoryGroup(string name, List<string> items)
        {
            Name = name;
            Items = items;
        }

        public string Name { get; }
        public List<string> Items { get; }
        public string CountText => $"({Items.Count})";
    }
}
