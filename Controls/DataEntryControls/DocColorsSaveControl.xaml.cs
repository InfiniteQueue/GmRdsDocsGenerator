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
using static GmRdsDocsGenerator.Statics.SnapshotStatics;

namespace GmRdsDocsGenerator.Controls.DataEntryControls
{
    /// <summary>
    /// Interaction logic for ViewSetupControl.xaml
    /// </summary>
    public partial class ViewSetupControl : UserControl
    {
        public ViewSetupControl()
        {
            InitializeComponent();
        }

        DocumentJobModel documentJobObject => this.DataContext as DocumentJobModel;

        private void btnAutoViewSetup_Click(object sender, RoutedEventArgs e)
        {
            documentJobObject.ViewStates.GenerateDefaultDocumentViewState(documentJobObject.SimObjects);
        }

        private void btnOverwrite_Click(object sender, RoutedEventArgs e)
        {
            documentJobObject.ViewStates.OverwriteDocumentViewStateWithCurrentView();
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            documentJobObject.ViewStates.OriginalViewState?.LoadDisplayData();
        }
    }
}
