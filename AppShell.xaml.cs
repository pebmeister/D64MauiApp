using System.Windows.Input;

namespace D64MauiApp
{
    public partial class AppShell : Shell
    {
        public ICommand ExitCommand { get; }

        public AppShell()
        {
            InitializeComponent();

            ExitCommand = new Command(OnExit);
            BindingContext = this;
        }

        private void OnExit()
        {
            // Handle the exit functionality
            Application.Current.Quit();
        }
    }
}

