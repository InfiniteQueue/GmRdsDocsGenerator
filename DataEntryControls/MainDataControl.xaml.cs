using GmRdsDocsGenerator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace GmRdsDocsGenerator.DataEntryControls
{
    /// <summary>
    /// Interaction logic for MainDataControl.xaml
    /// </summary>
    public partial class MainDataControl : UserControl
    {
        public MainDataControl()
        {
            InitializeComponent();
            this.DataContextChanged += this.AttachViewModel; ;
        }

        private void AttachViewModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is INotifyPropertyChanged oldVm) oldVm.PropertyChanged -= this.VMPropertyChanged;
            if (e.NewValue is INotifyPropertyChanged newVm) newVm.PropertyChanged += this.VMPropertyChanged;
        }

        private void VMPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ViewModelPropertyChanged?.Invoke(sender, e);
        }

        public MainDataModel ViewModel
        {
            get => this.DataContext as MainDataModel;
            set => this.DataContext = value;
        }

        public event PropertyChangedEventHandler ViewModelPropertyChanged;
    }
}
