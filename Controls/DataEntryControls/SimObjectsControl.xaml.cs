using GmRdsDocsGenerator.Models;
using GmRdsDocsGenerator.DebugClasses;
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
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Ui;
using Tecnomatix.Engineering.Ui.WPF;

namespace GmRdsDocsGenerator.Controls.DataEntryControls
{
    /// <summary>
    /// Interaction logic for SimObjectsControl.xaml
    /// </summary>
    public partial class SimObjectsControl : UserControl
    {
        public SimObjectsControl()
        {
            InitializeComponent();
            DataContextChanged += this.AttachViewModel;
            AttachViewModel(this, new DependencyPropertyChangedEventArgs(DataContextProperty, null, ViewModel));
        }

        private void AttachViewModel(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null) (e.OldValue as INotifyPropertyChanged).PropertyChanged -= this.SimObjectsControl_PropertyChanged;
            if (e.NewValue != null) (e.NewValue as INotifyPropertyChanged).PropertyChanged += this.SimObjectsControl_PropertyChanged;
        }

        public SimObjectsModel ViewModel
        {
            get => this.DataContext as SimObjectsModel;
            set => this.DataContext = value;
        }

        private void SimObjectsControl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ViewModelPropertyChanged?.Invoke(sender, e);    
           
        }

        private void btnDebug_Click(object sender, RoutedEventArgs e)
        {
            DebugMethods.DebugMain();
        }

        private void TxObjEditBoxControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TxObjEditBoxControl editBox) editBox.LoseFocus();
            if (sender is TxObjectGridControl grdControl) grdControl.LoseFocus();
        }

        private void ctrlRobotBox_TypeValid(object sender, EventArgs e)
        {
            if (sender is TxObjEditBoxControl editBox) editBox.LoseFocus();
        }

        public event PropertyChangedEventHandler ViewModelPropertyChanged;
    }
}
