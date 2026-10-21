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
            StateManager.ActiveView = ViewSetupDCS.NewSetup<ViewSetupDCS>(StateManager);
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            StateManager.ActiveView = null; //Disposes the previous view
        }

        private void DcsTop(object sender, RoutedEventArgs e)
        {
            StateManager.ActiveView = ViewSetupDcsTop.NewSetup<ViewSetupDcsTop>(StateManager);
        }

        private void DcsSide(object sender, RoutedEventArgs e)
        {
            StateManager.ActiveView = ViewSetupDcsSide.NewSetup<ViewSetupDcsSide>(StateManager);
        }

        private void DcsIso(object sender, RoutedEventArgs e)
        {
            StateManager.ActiveView = ViewSetupDcsIso.NewSetup<ViewSetupDcsIso>(StateManager);
        }

        private void EOATIso(object sender, RoutedEventArgs e)
        {
            StateManager.ActiveView = ViewSetupEOATIso.NewSetup<ViewSetupEOATIso>(StateManager);
        }

        private void EOATRearIso(object sender, RoutedEventArgs e)
        {
            StateManager.ActiveView = ViewSetupEOATRearIso.NewSetup<ViewSetupEOATRearIso>(StateManager);

        }

        private void ToolChanger(object sender, RoutedEventArgs e)
        {
            StateManager.ActiveView = ViewSetupToolchanger.NewSetup<ViewSetupToolchanger>(StateManager);
        }

        private void EOATIndex(object sender, RoutedEventArgs e)
        {
            var newSetup = ViewSetupEOATIndividual.NewSetup<ViewSetupEOATIndividual>(StateManager);
            newSetup.EOATIndex = 0;
            StateManager.ActiveView = newSetup;

        }

        private void RestrictedSpaceEnvelope(object sender, RoutedEventArgs e)
        {
            StateManager.ActiveView = ViewSetupRestrictedSpaceEnvelope.NewSetup<ViewSetupRestrictedSpaceEnvelope>(StateManager);
        }
    }
}
