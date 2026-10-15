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
using GmRdsDocsGenerator.Services;
using Tecnomatix.Engineering;

namespace GmRdsDocsGenerator.DataEntryControls
{
    /// <summary>
    /// Interaction logic for DocumentJobControl.xaml
    /// </summary>
    public partial class DocumentJobControl : UserControl
    {
        public DocumentJobControl()
        {
            InitializeComponent();

            ctrlSimObjects.ViewModelPropertyChanged += this.CtrlSimObjects_ViewModelPropertyChanged;
        }

        private void CtrlSimObjects_ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        internal DocumentJobModel Model
        {
            get => (DocumentJobModel)this.DataContext;
            set => this.DataContext = value;
        }

        private void btnDebug_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
