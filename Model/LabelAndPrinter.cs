using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintService.Model
{
    public class LabelAndPrinter : ObservableObject
    {
        private string labelFileName;
        public string LabeFilelName
        {
            get => labelFileName;
            set => SetProperty(ref labelFileName, value);
        }
        private string labelName;
        public string LabelName
        { 
            get => labelName;
            set => SetProperty(ref labelName, value);
        }
        /// <summary>
        /// 描述
        /// </summary>
        private string labelDescription;
        public string LabelDescription
        {
            get => labelDescription;
            set => SetProperty(ref labelDescription, value);
        }
        private string path;
        public string Path
        {
            get => path;
            set => SetProperty(ref path, value);
        }
        private string printerName;
        public string PrinterName
        {
            get => printerName;
            set => SetProperty(ref printerName, value);
        }
        private string printerDescription;
        public string PrinterDescription
        {
            get => printerDescription;
            set => SetProperty(ref printerDescription, value);
        }
        private string printerAddress;
        public string PrinterAddress
        {
            get => printerAddress;
            set => SetProperty(ref printerAddress, value);
        }


    }
}
