﻿using PrintService.ViewModels;
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
using System.Windows.Shapes;
using Wpf.Ui.Appearance;

namespace PrintService.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView 
    {
        public MainWindowViewModel ViewModel { get; }
        public MainView()
        {
            ViewModel = new MainWindowViewModel();
            SystemThemeWatcher.Watch(this);
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
