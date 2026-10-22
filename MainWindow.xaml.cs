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
using System.Windows.Shapes;
using GmRdsDocsGenerator.Services;
using GmRdsDocsGenerator.Statics;
using Tecnomatix.Engineering;

namespace GmRdsDocsGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundSetup backgroundSetup = new BackgroundSetup();

        public MainWindow()
        {
            InitializeComponent();

            ctrlDocumentJobControl.Model = new Models.DocumentJobModel();

            this.Closing += OnClose;
            this.Closed += OnClose;
        }

        void OnClose(object sender, EventArgs e)
        {
            DisposeView();
            RestoreBackground();
        }
        void DisposeView()
        {
            if (ctrlDocumentJobControl?.Model?.ViewStates?.OriginalViewSnapshot is TxSnapshot originalViewSnapshot) originalViewSnapshot.LoadDisplayData();
        }

        private void RestoreBackground()
        {
            backgroundSetup?.Dispose();
            backgroundSetup = null;
        }
    }
}
