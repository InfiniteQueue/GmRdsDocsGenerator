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

namespace GmRdsDocsGenerator.Controls.ActionControls
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

        internal ViewStateManager StateManager
        {
            get => this.DataContext as ViewStateManager;
            set => this.DataContext = value;
        }

        private void DCSSetup(object sender, RoutedEventArgs e)
        {
            StateManager.ActiveView = Services.ViewSetup.ViewSetupDCS.NewSetup(StateManager);
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            StateManager.ActiveView = null;
        }
    }
}
