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
            viewStateRegistry.DocumentViewState.LoadDisplayData();
            HideAll();
            DisplayAll(viewStateRegistry.Categories[ObjCategories.MainDcs]);
            TxApplication.ViewersManager.GraphicViewer.ZoomToFit();
            DisplayAll(viewStateRegistry.Categories[ObjCategories.MainRobot]);
            DisplayAll(viewStateRegistry.Categories[ObjCategories.EOAT]);
        }
    }
}
