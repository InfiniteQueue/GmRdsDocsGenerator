using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Ui;
using Tecnomatix.Engineering.Ui.WPF;

namespace GmRdsDocsGenerator.Controls.DataEntryControls
{
    /// <summary>
    /// Wraps a TxObjectGridControl so its object list can be bound from XAML, e.g.
    /// <c>&lt;local:TxObjectListPicker Objects="{Binding Panels}" ColumnHeader="Panel"/&gt;</c>
    /// </summary>
    public partial class TxObjectListPicker : UserControl
    {
        private bool _gridInitialized;
        private bool _syncingToGrid;
        private bool _syncingToSource;

        public TxObjectListPicker()
        {
            InitializeComponent();
            Loaded += TxObjectListPicker_Loaded;

            ctrlGrid.Picked += (s, a) => SyncToSource();
            ctrlGrid.DataChanged += (s, a) => SyncToSource();
            ctrlGrid.RowDeleted += (s, a) => SyncToSource();
            ctrlGrid.DataCleared += (s, a) => SyncToSource();
        }

        /// <summary>The objects shown in the grid. Binds two-way by default; the source always receives a new list instance on change.</summary>
        public TxObjectList Objects
        {
            get { return (TxObjectList)GetValue(ObjectsProperty); }
            set { SetValue(ObjectsProperty, value); }
        }

        public static readonly DependencyProperty ObjectsProperty =
            DependencyProperty.Register(nameof(Objects), typeof(TxObjectList), typeof(TxObjectListPicker),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    (d, e) => ((TxObjectListPicker)d).PushToGrid()));

        /// <summary>Header text of the pick column. Only read when the control first loads.</summary>
        public string ColumnHeader
        {
            get { return (string)GetValue(ColumnHeaderProperty); }
            set { SetValue(ColumnHeaderProperty, value); }
        }

        public static readonly DependencyProperty ColumnHeaderProperty =
            DependencyProperty.Register(nameof(ColumnHeader), typeof(string), typeof(TxObjectListPicker), new PropertyMetadata("Objects"));

        /// <summary>The wrapped grid, for settings not exposed by this control</summary>
        public TxObjectGridControl InnerGrid => ctrlGrid;

        private void TxObjectListPicker_Loaded(object sender, RoutedEventArgs e)
        {
            if (_gridInitialized) return; // Loaded fires each time a parent tab is reselected
            if (DesignerProperties.GetIsInDesignMode(this)) return; // Tecnomatix native libraries can't load in the XAML designer

            var column = new TxGridColumnData();
            column.Init(ColumnHeader, TxGridColumnType.StaticText);
            var columns = new TxGridColumnDataCollection();
            columns.AddItem(column);
            var desc = new TxGridDescription();
            desc.SetData(columns, 0); // column 0 receives picks
            ctrlGrid.Init(desc);

            _gridInitialized = true;
            PushToGrid();
        }

        /// <summary>Copies the bound list into the grid</summary>
        private void PushToGrid()
        {
            if (!_gridInitialized || _syncingToSource) return;

            _syncingToGrid = true;
            try
            {
                var source = Objects;
                // Setting Objects to an empty list adds a second blank pick row, so clear instead
                if (source == null || source.Count == 0)
                {
                    if (ctrlGrid.Count > 0) ctrlGrid.Clear();
                }
                else
                {
                    //Clone, TxObjectList is a reference type
                    ctrlGrid.Objects = new TxObjectList(source);
                }
            }
            finally { _syncingToGrid = false; }
        }

        /// <summary>Copies the grid's objects back to the bound list after user edits</summary>
        private void SyncToSource()
        {
            if (!_gridInitialized || _syncingToGrid) return;

            var gridObjects = ctrlGrid.Objects ?? new TxObjectList();
            if (SameObjects(Objects, gridObjects)) return; // several grid events can fire for one edit

            _syncingToSource = true;
            try { SetCurrentValue(ObjectsProperty, new TxObjectList(gridObjects)); } // SetCurrentValue keeps the binding intact
            finally { _syncingToSource = false; }
        }

        private static bool SameObjects(TxObjectList a, TxObjectList b)
        {
            int countA = a?.Count ?? 0, countB = b?.Count ?? 0;
            if (countA != countB) return false;
            for (int i = 0; i < countA; i++)
            {
                if (!Equals(a[i], b[i])) return false;
            }
            return true;
        }

        private void ctrlGrid_KeyDown(object sender, KeyEventArgs e)
        {
            ctrlGrid.LoseFocus();
        }
    }
}
