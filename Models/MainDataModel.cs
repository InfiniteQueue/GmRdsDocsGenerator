using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GmRdsDocsGenerator.Models
{
    public class MainDataModel : ObservableObject
    {
        private string _robotName;
            [ObservableProperty]
        public string RobotName { get => _robotName; set => SetProperty(ref _robotName, value); }
        private string _robotModel;
            [ObservableProperty]
        public string RobotModel { get => _robotModel; set => SetProperty(ref _robotModel, value); }
        private string _dCSVersion;
            [ObservableProperty]
        public string DCSVersion { get => _dCSVersion; set => SetProperty(ref _dCSVersion, value); }
        private string _plantName;
            [ObservableProperty]
        public string PlantName { get => _plantName; set => SetProperty(ref _plantName, value); }
        private string _supplierName;
            [ObservableProperty]
        public string SupplierName { get => _supplierName; set => SetProperty(ref _supplierName, value); }
        private string _supplierContact;
            [ObservableProperty]
        public string SupplierContact { get => _supplierContact; set => SetProperty(ref _supplierContact, value); }
        private string _supplierPhone;
            [ObservableProperty]
        public string SupplierPhone { get => _supplierPhone; set => SetProperty(ref _supplierPhone, value); }
        private DateTime _revisionDate;
            [ObservableProperty]
        public DateTime RevisionDate { get => _revisionDate; set => SetProperty(ref _revisionDate, value); }
        private int _revisionNumber;
            [ObservableProperty]
        public int RevisionNumber { get => _revisionNumber; set => SetProperty(ref _revisionNumber, value); }
    }
}
