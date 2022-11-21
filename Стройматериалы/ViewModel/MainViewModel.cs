using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Стройматериалы.Models;
using Стройматериалы.Tools;
using Стройматериалы.View;

namespace Стройматериалы.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private Page currentPage;
        private User user = new User();
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

        public User User
        {
            get => user;
            set
            {
                LoggedIn = Visibility.Visible;
                user = value;
                SignalChanged();
                SignalChanged("UserName");
                SignalChanged("Role");
            }
        }

        public string Role
        {
            get => user.UserRoleNavigation?.RoleName;
        }

        public string UserName
        {
            get => $"{user.UserSurname} {user.UserName} {user.UserPatronymic}";
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

        public ViewCommand Logout { get; set; }

        public MainViewModel()
        {
            CurrentPage = new AuthPage(this);

            Logout = new ViewCommand(() => 
            {
                User = new User();
                LoggedIn = Visibility.Collapsed;
            });
        }
    }
}
