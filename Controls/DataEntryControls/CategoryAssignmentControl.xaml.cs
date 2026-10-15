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
using GmRdsDocsGenerator.Constants;
using GmRdsDocsGenerator.Models;
using Tecnomatix.Engineering;
using static GmRdsDocsGenerator.Models.CategoryRegistry;

namespace GmRdsDocsGenerator.Controls.DataEntryControls
{
    /// <summary>
    /// Interaction logic for ColouringToolsControl.xaml
    /// </summary>
    public partial class CategoryAssignmentControl : UserControl
    {
        public CategoryAssignmentControl()
        {
            InitializeComponent();

            foreach(var pair in CategoryNames)
            {
                AddNewColourButton(pair);
            }
        }

        internal DocumentJobModel JobModel
        {
            get => this.DataContext as DocumentJobModel;
            set => this.DataContext = value;
        }

        void AddNewColourButton(KeyValuePair<string, ObjCategories?> pair)
        {
            Button colourButton = GenerateNewColourButton(pair);
            this.pnlAssignColours.Children.Add(colourButton);
        }
        Button GenerateNewColourButton(KeyValuePair<string, ObjCategories?> pair)
        {
            var button = new Button();
            button.Content = pair.Key;
            if (pair.Value != null)
            {
                button.Background = new SolidColorBrush(ColorFromTxColor(CategoryColors[pair.Value.Value] ?? GMColours.White));
                button.Foreground = IsDarkColor(CategoryColors[pair.Value.Value]) ? new SolidColorBrush(Colors.White) : SystemColors.ControlTextBrush;
            }
            button.Click += (s,e) => AssignSelectedObjectCategory(pair.Value);

            return button;
        }
        void AssignSelectedObjectCategory(ObjCategories? objCategory)
        {
            foreach (var item in TxApplication.ActiveSelection.GetFilteredItems(new TxTypeFilter(typeof(ITxDisplayableObject))).OfType<ITxDisplayableObject>())
            {
                JobModel?.ViewStates?.AssignObjectCategory(item, objCategory);
            }
        }

        void ColourSelectedObjects(TxColor color)
        {
            foreach(var item in TxApplication.ActiveSelection.GetFilteredItems(new TxTypeFilter(typeof(ITxDisplayableObject))).OfType<ITxDisplayableObject>())
            {
                if (color != null)
                {
                    item.Color = color;
                }
                else
                {
                    item.RestoreColor();
                }
            }
        }

        Color ColorFromTxColor(TxColor txColor)
        {
            return new Color() { B = txColor.Blue, R = txColor.Red, G = txColor.Green, A = 255 };
        }

        bool IsDarkColor(TxColor color)
        {
            if (color == null) return false;
            return color.Red + color.Green + color.Blue < 255 * 3 / 2;
        }

        private Dictionary<string, CategoryRegistry.ObjCategories?> CategoryNames => new Dictionary<string, CategoryRegistry.ObjCategories?>()
        {
            {"Main Robot", CategoryRegistry.ObjCategories.MainRobotEquipments },
            {"Main DCS", CategoryRegistry.ObjCategories.MainDcs },
            {"Robots", CategoryRegistry.ObjCategories.OtherRobotsEquipments },
            {"EOAT", CategoryRegistry.ObjCategories.EOAT },
            {"Fencing", CategoryRegistry.ObjCategories.Fencing },
            {"Tools Surrounding Robot", CategoryRegistry.ObjCategories.ToolsSurroundingRobot },
            {"CPC Shared Zones",  CategoryRegistry.ObjCategories.CPCSharedZones},
            {"CPC Shared Zones (CSC)", CategoryRegistry.ObjCategories.CPCSharedZonesCSC },
            {"CPC Restricted Zones", CategoryRegistry.ObjCategories.CPCRestrictedZones },
            {"CPC Restricted Zones (CSC)", CategoryRegistry.ObjCategories.CPCRestrictedZonesCSC },
            {"Remove Colour", null },
        };
    }
}
