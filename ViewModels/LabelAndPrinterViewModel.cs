using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using PrintService.Domain;
using PrintService.Log;
using PrintService.Model;
using PrintService.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PrintService.ViewModels
{
    public class LabelAndPrinterViewModel : ObservableObject
    {

        private LabelAndPrinter labelAndPrinter = new LabelAndPrinter();
        public LabelAndPrinter LabelAndPrinterModel
        {
            get => labelAndPrinter;
            set => SetProperty(ref labelAndPrinter, value);
        }

        public ICommand AddPrinter {  get; }
        public ICommand AddLabel {  get; }
        public ICommand CloseWindow {  get; }
        public ICommand UploadFile {  get; }

        private Window currentWindow { get; }
        public LabelAndPrinterViewModel(System.Windows.Window window) 
        {
            currentWindow = window;
            AddPrinter = new RelayCommand(savePrinter);
            CloseWindow = new RelayCommand(closeWindow);
            AddLabel = new RelayCommand(saveLabel);
            UploadFile = new RelayCommand(saveFile);

        }
        private void savePrinter()
        {
            Printer printer = new Printer();
            printer.address = LabelAndPrinterModel.PrinterAddress;
            printer.name = LabelAndPrinterModel.PrinterName ;
            printer.description = LabelAndPrinterModel.PrinterDescription ;
            App.printBll.InsertPrinter(printer);
        }
        private void saveLabel()
        {
            try
            {
                LabelFileInfo printer = new LabelFileInfo();
                printer.name = LabelAndPrinterModel.LabelName;
                printer.description = LabelAndPrinterModel.LabelDescription;
                printer.path = LabelAndPrinterModel.Path;
                printer.url = Path.Combine(App.configuration["labelDir"], LabelAndPrinterModel.LabeFilelName);
                App.printBll.InsertLabelFile(printer);
                LogHelper.Info(LogHelper.WPF_SHOW_START + "添加标签成功，名称：" + printer.name);
            }
            catch (Exception ex)
            {
                LogHelper.Error(LogHelper.WPF_SHOW_START + "添加标签失败", ex);
            }
         
            closeWindow();
        }

        private void closeWindow()
        {
            currentWindow.Close();
        }
        private void saveFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\desktop";    
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowDialog();    //显示对话框
            try
            {
                string fileFullPath = openFileDialog.FileName; //获取选择的文件的全路径名
                string fileName = Path.GetFileName(fileFullPath);
                LabelAndPrinterModel.Path = Path.Combine(Environment.CurrentDirectory, App.configuration["labelDir"], fileName);
                LabelAndPrinterModel.LabeFilelName = fileName;
                // 存在文件先删除在保存
                if(File.Exists(fileFullPath))
                {
                    File.Delete(fileFullPath);
                }
                using FileStream readFile = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read);
                using FileStream writeFile = new FileStream(LabelAndPrinterModel.Path, FileMode.Create, FileAccess.Write);
                readFile.CopyTo(writeFile);

            }catch (Exception ex)
            {
                LogHelper.Error("上传标签文件失败", ex);
            }

        }




    }
}
