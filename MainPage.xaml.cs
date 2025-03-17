using d64Wrapper;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace D64MauiApp
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        private D64 _d64;
        private Bam _bam = new();
        private BamTrackEntry _bamTrack = new();
        int count = 0;

        private ObservableCollection<string> _fileMenuItems = new ObservableCollection<string>();
        private ObservableCollection<ICommand> _fileMenuCommands = new ObservableCollection<ICommand>();
        private string _fileMenuName = "";

        /// <summary>
        /// The menu item string names
        /// </summary>
        public ObservableCollection<string> FileMenuItems
        {
            get => _fileMenuItems;
            set
            {
                _fileMenuItems = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Commands for the File menu items
        /// </summary>
        public ObservableCollection<ICommand> FileMenuCommands
        {
            get => _fileMenuCommands;
            set
            {
                _fileMenuCommands = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// File menu title
        /// </summary>
        public string FileMenuName
        {
            get => _fileMenuName;
            set
            {
                _fileMenuName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            BuildMenus();
        }

        private void BuildMenus()
        {
            FileMenuName = "File";
            FileMenuItems = new ObservableCollection<string> { "Open", "Save", "Exit" };
            FileMenuCommands = new ObservableCollection<ICommand>
            {
                new Command(CommandOpenExecuted),
                new Command(CommandSaveExecuted),
                new Command(CommandExitExecuted),
            };
        }

        /// <summary>
        /// User used exit from the menu
        /// </summary>
        public void CommandExitExecuted()
        {
            Application.Current.Quit();
        }

        /// <summary>
        /// User used open from the menu
        /// </summary>
        public void CommandOpenExecuted()
        {
            // Implement open functionality
        }

        /// <summary>
        /// User used save from the menu
        /// </summary>
        public void CommandSaveExecuted()
        {
            // Implement save functionality
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

