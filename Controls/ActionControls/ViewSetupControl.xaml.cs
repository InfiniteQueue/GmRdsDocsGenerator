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
using GmRdsDocsGenerator.Services.ViewSetup;
using static GmRdsDocsGenerator.Services.ViewSetup.ViewSetupBase;


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
            StateManager.ActiveView = NewSetup<ViewSetupDCS>(StateManager);
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            StateManager.ActiveView = null; //Disposes the previous view
        }

        private void DcsTop(object sender, RoutedEventArgs e)
        {
            StateManager.ActiveView = NewSetup<ViewSetupDcsTop>(StateManager);
        }

        private void DcsSide(object sender, RoutedEventArgs e)
        {
            StateManager.ActiveView = NewSetup<ViewSetupDcsSide>(StateManager);
        }

        private void DcsIso(object sender, RoutedEventArgs e)
        {
            StateManager.ActiveView = NewSetup<ViewSetupDcsIso>(StateManager);
        }

        private void EOATIso(object sender, RoutedEventArgs e)
        {
            StateManager.ActiveView = NewSetup<ViewSetupEOATIso>(StateManager);
        }

        private void EOATRearIso(object sender, RoutedEventArgs e)
        {
            StateManager.ActiveView = NewSetup<ViewSetupEOATRearIso>(StateManager);

        }

        private void ToolChanger(object sender, RoutedEventArgs e)
        {
            StateManager.ActiveView = NewSetup<ViewSetupToolchangerIso>(StateManager);
        }

        private void EOATIndex(object sender, RoutedEventArgs e)
        {
            var newSetup = NewSetup<ViewSetupEOATIndividual>(StateManager);
            newSetup.EOATIndex = 0;
            StateManager.ActiveView = newSetup;

        }

        private void RestrictedSpaceEnvelope(object sender, RoutedEventArgs e)
        {
            StateManager.ActiveView = NewSetup<ViewSetupRestrictedSpaceEnvelope>(StateManager);
        }

        private void CpcTop(object sender, RoutedEventArgs e)
        {
            StateManager.ActiveView = NewSetup<ViewSetupCPCPlan>(StateManager);
        }

        private void CpcIso(object sender, RoutedEventArgs e)
        {

            var newSetup = NewSetup<ViewSetupCPCAngledIso>(StateManager);
            newSetup.ClockwiseTurns = 1;
            StateManager.ActiveView = newSetup;
        }
    }
}
