using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Tecnomatix.Engineering;
using static GmRdsDocsGenerator.Models.CategoryRegistry;

namespace GmRdsDocsGenerator.Models
{
    /// <remarks>
    /// TxObjectList raises no change notifications, so bound controls only update when a list property is assigned.
    /// Always assign a new, already filled list; items added to or removed from the current list are not displayed.
    /// </remarks>
    public class SimObjectsModel : ObservableObject
    {
        private ITxRobot _robot;
        public ITxRobot Robot { get => _robot; set => SetProperty(ref _robot, value); }

        private TxObjectList _eoat = new TxObjectList();
        public TxObjectList EOAT { get => _eoat; set => SetProperty(ref _eoat, value); }

        private ITxComponent _riser;
        public ITxComponent Riser { get => _riser; set => SetProperty(ref _riser, value); }

        private ITxComponent _toolChanger;
        public ITxComponent ToolChanger { get => _toolChanger; set => SetProperty(ref _toolChanger, value); }

        private TxObjectList _product = new TxObjectList();
        public TxObjectList Product { get => _product; set => SetProperty(ref _product, value); }

        private TxObjectList _dcsObjects = new TxObjectList();

        public TxObjectList DcsObjects
        {
            get => _dcsObjects;
            set => SetProperty(ref _dcsObjects, value);
        }

        private TxObjectList<TxGroup> _cpcZones = new TxObjectList<TxGroup>();
        public TxObjectList<TxGroup> CpcZones
        {
            get => _cpcZones;
            set => SetProperty(ref _cpcZones, value);
        }

        private TxObjectList<ITxComponent> _eoatToolChangers = new TxObjectList<ITxComponent>();
        public TxObjectList<ITxComponent> EoatToolChangers
        {
            get => _eoatToolChangers;
            set => SetProperty(ref _eoatToolChangers, value);
        }

        public ITxObject RestrictedVolume { get; set; }

        private TxObjectList<TxGroup> _cpcSharedZones = new TxObjectList<TxGroup>();

        public TxObjectList<TxGroup> CPCSharedZones
        {
            get { return _cpcSharedZones; }
            set { _cpcSharedZones = value; }
        }
        private TxObjectList<TxGroup> _cpcSharedZonesCSC = new TxObjectList<TxGroup>();

        public TxObjectList<TxGroup> CPCSharedZonesCSC
        {
            get { return _cpcSharedZonesCSC; }
            set { _cpcSharedZonesCSC = value; }
        }

        private TxObjectList<TxGroup> _cpcRestrictedZones = new TxObjectList<TxGroup>();
        public TxObjectList<TxGroup> CPCRestrictedZones
        {
            get { return _cpcRestrictedZones; }
            set { _cpcRestrictedZones = value; }
        }
        private TxObjectList<TxGroup> _cpcRestrictedZonesCSC = new TxObjectList<TxGroup>();

        public TxObjectList<TxGroup> CPCRestrictedZonesCSC
        {
            get { return _cpcRestrictedZonesCSC; }
            set { _cpcRestrictedZonesCSC = value; }
        }
    }
}
