﻿using System;
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
using Стройматериалы.Models;
using Стройматериалы.ViewModel;

namespace Стройматериалы.View
{
    /// <summary>
    /// Логика взаимодействия для EditProductPage.xaml
    /// </summary>
    public partial class EditProductPage : Page
    {
        public EditProductPage(Product edit, MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = new EditProductViewModel(edit, mainViewModel);
        }
    }
}
