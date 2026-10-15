using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Tecnomatix.Engineering;

namespace GmRdsDocsGenerator.Models
{
    public class SimObjectsModel : ObservableObject
    {
        private ITxRobot _robot;
        public ITxRobot Robot { get => _robot; set => SetProperty(ref _robot, value); }

        private ITxDevice _gripper;
        public ITxDevice Gripper { get => _gripper; set => SetProperty(ref _gripper, value); }

        private TxObjectList _panels = new TxObjectList();
        public TxObjectList Panels { get => _panels; set => SetProperty(ref _panels, value); }

        private TxObjectList _dcsObjects;

        public TxObjectList DcsObjects
        {
            get => _dcsObjects;
            set => SetProperty(ref _dcsObjects, value);
        }


    }
}
