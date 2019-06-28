using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using ReactiveUI;

namespace Host.Generic.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Hello World!";

        private string _username;

        public event EventHandler CredentialsCollected;

        public string Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }
        private string _secret;

        public string Secret
        {
            get => _secret;
            set => this.RaiseAndSetIfChanged(ref _secret, value);
        }

        public bool Success { get; private set;}

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }

        public MainWindowViewModel()
        {
            LoginCommand = ReactiveCommand.Create(Login);
            CancelCommand = ReactiveCommand.Create(Cancel);
        }

        void Login()
        {
            Success = true;
            OnCredentialsCollected(new EventArgs());
        }

        void Cancel()
        {
            Success = false;
            OnCredentialsCollected(new EventArgs());
        }

        protected virtual void OnCredentialsCollected(EventArgs e)
        {
            EventHandler handler = CredentialsCollected;
            handler?.Invoke(this, e);
        }
    }
}
