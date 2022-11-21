using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Стройматериалы.Tools;

namespace Стройматериалы.ViewModel
{
    internal class AuthPageViewModel : BaseViewModel
    {
        private MainViewModel mainViewModel;
        private Visibility capchaVisible;
        private readonly Canvas capchaCanvas;
        private readonly PasswordBox textPassword;

        public ViewCommand Enter { get; set; }
        public ViewCommand EnterGuest { get; set; }

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
            this.textPassword = textPassword;

            Enter = new ViewCommand(() => 
            {
            
            });

            EnterGuest = new ViewCommand(() =>
            {

            });
        }
    }
}
