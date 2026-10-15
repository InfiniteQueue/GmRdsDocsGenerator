using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GmRdsDocsGenerator.Statics;
using Tecnomatix.Engineering;
using ObjCategories = GmRdsDocsGenerator.Models.CategoryRegistry.ObjCategories;

namespace GmRdsDocsGenerator.Services.ViewSetup
{
    internal class ViewSetupDCS : ViewSetupBase<ViewSetupDCS>
    {
        protected override void PerformSetup()
        {
            var graphicViewer = TxApplication.ViewersManager.GraphicViewer;
            viewStateRegistry.DocumentViewState.LoadDisplayData();
            HideAll();

            //Reframe on DCS objects
            DisplayAll(viewStateRegistry.Categories[ObjCategories.MainDcs]);
            graphicViewer.ZoomToFit();

            graphicViewer.ShowContours = true;

            DisplayAll(viewStateRegistry.Categories[ObjCategories.MainRobot]);
            DisplayAll(viewStateRegistry.Categories[ObjCategories.EOAT]);
        }

        protected override void Cleanup()
        {
            var graphicViewer = TxApplication.ViewersManager.GraphicViewer;
            graphicViewer.ShowContours = false;

        }

        private void SetupDCSDisplay()
        {
            var dcsObjects = viewStateRegistry.Categories[ObjCategories.MainDcs];

            foreach(var item in dcsObjects.OfType<ITxDisplayableObject>())
            {
                item.DisplayMode = TxDisplayMode.Wireframe;
                item.DisplayMode = TxDisplayMode.Shaded;
            }
        }
    }
}
