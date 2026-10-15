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

            this.Closing += RestoreBackground;
            this.Closed += RestoreBackground;
        }

        void RestoreBackground(object sender, EventArgs e)
        {
            backgroundSetup?.Dispose();
            backgroundSetup = null;
        }
    }
}
