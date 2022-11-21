using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Стройматериалы.Tools;
using Стройматериалы.View;

namespace Стройматериалы.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private Page currentPage;
        private string user;
        private Visibility loggedIn = Visibility.Collapsed;

        public Page CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                SignalChanged();
            }
        }
        public string User
        {
            get => user;
            set
            {
                user = value;
                SignalChanged();
            }
        }
        public Visibility LoggedIn
        {
            get => loggedIn;
            set
            {
                loggedIn = value;
                SignalChanged();
            }
        }
        public MainViewModel()
        {
            CurrentPage = new AuthPage(this);
        }
    }
}
