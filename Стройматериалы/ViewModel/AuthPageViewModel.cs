using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Стройматериалы.DB;
using Стройматериалы.Models;
using Стройматериалы.Tools;
using Стройматериалы.View;

namespace Стройматериалы.ViewModel
{
    internal class AuthPageViewModel : BaseViewModel
    {
        private MainViewModel mainViewModel;
        private Visibility capchaVisible = Visibility.Collapsed;
        private readonly Canvas capchaCanvas;

        public string Login { get; set; }
        public string CapchaText { get; set; }

        public ViewCommand Enter { get; set; }
        public ViewCommand EnterGuest { get; set; }

        public bool CanEnter { 
            get => canEnter;
            set
            {
                canEnter = value;
                SignalChanged();
            }
        }

        public Visibility CapchaVisible
        {
            get => capchaVisible;
            set
            {
                capchaVisible = value;
                SignalChanged();
            }
        }

        public AuthPageViewModel(MainViewModel mainViewModel, Canvas capchaCanvas, System.Windows.Controls.PasswordBox textPassword)
        {
            this.mainViewModel = mainViewModel;
            this.capchaCanvas = capchaCanvas;

            Enter = new ViewCommand(() =>
            {
                string pass = textPassword.Password;
                if (CapchaVisible == Visibility.Collapsed)
                {
                    CheckLoginPassword(pass);
                }
                else
                {
                    if (capchaValue == CapchaText)
                    {
                        CheckLoginPassword(pass);
                    }
                    else
                    {
                        MessageBox.Show("Введенные символы не совпадают с капчей");
                        DispatcherTimer timer = new DispatcherTimer();
                        timer.Interval = TimeSpan.FromSeconds(10);
                        timer.Tick += Timer_Tick;
                        timer.Start();
                        
                        CanEnter = false;
                        GenerateCapcha();
                    }
                }
            });

            EnterGuest = new ViewCommand(() =>
            {
                mainViewModel.User = new User { UserRoleNavigation = new Role { RoleName = "Гость" } };
                mainViewModel.CurrentPage = new ProductListPage(mainViewModel);
            });
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            ((DispatcherTimer)sender).Stop();
            CanEnter = true;
        }

        private void CheckLoginPassword(string pass)
        {
            User user = null;
            try
            {
                user = DEContext.GetInstance().Users.Include("UserRoleNavigation").FirstOrDefault(s => s.UserPassword == pass & s.UserLogin == Login);
            }
            catch 
            {
                MessageBox.Show("Ошибка связи с бд");
                return;
            }
            if (user == null)
            {
                MessageBox.Show("Такого пользователя не существует!");
                CapchaVisible = Visibility.Visible;
                GenerateCapcha();
            }
            else
                EnterUser(user);
        }

        string capchaValue;
        private bool canEnter = true;

        private void GenerateCapcha()
        {
            capchaCanvas.Children.Clear();
            Random rnd = new Random();
            string value = "";
            string temp;
            for (int i = 0; i < 4; i++)
            {
                if (rnd.NextDouble() > 0.3)
                {
                    temp = ((char)rnd.Next(65, 91)).ToString();
                    if (rnd.NextDouble() > 0.5)
                        temp = temp.ToLower();
                }
                else
                    temp = ((char)rnd.Next(48, 58)).ToString();
                TextBlock text = new TextBlock();
                text.Text = temp;
                text.FontSize = 35;
                capchaCanvas.Children.Add(text);
                Canvas.SetLeft(text, i * 10 + 5);
                Canvas.SetTop(text, 5 + rnd.Next(-5, 5));
                value += temp;
            }
            capchaValue = value;

        }

        private void EnterUser(User user)
        {
            Auth.Auth.CurrentUser = user;
            mainViewModel.User = user;
            mainViewModel.CurrentPage = new ProductListPage(mainViewModel);
        }
    }
}
