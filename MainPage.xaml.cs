using D64MauiApp.Controls;
using d64Wrapper;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Controls;

namespace D64MauiApp
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        private D64 _d64;
        private Bam _bam;

        private ObservableCollection<C64MenuItem> _fileMenuItems = new ObservableCollection<C64MenuItem>();
        private string _fileMenuName = "";

        private string _startDirectory = "";
        public string StartDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_startDirectory))
                    _startDirectory = SetStartDirectory();
                return _startDirectory;
            }
            set
            {
                _startDirectory = value;
                OnPropertyChanged();
            }
        }

        private string _d64DiskName;
        public string D64DiskName
        {
            get => string.IsNullOrEmpty(_d64DiskName)  ? "" : _d64DiskName;
            set
            {
                _d64DiskName = value;
                OnPropertyChanged();
            }
        }

        public D64 d64 
        { 
            get => _d64;
            set
            {
                _d64 = value;
                OnPropertyChanged();

                D64DiskName = d64.DiskName();
            }
        }

        /// <summary>
        /// The menu items for the file menu
        /// </summary>
        public ObservableCollection<C64MenuItem> FileMenuItems
        {
            get => _fileMenuItems;
            set
            {
                _fileMenuItems = value;
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
            // C64FileDialogControl.FileSelected += FileSelected;
        }

        private void BuildMenus()
        {
            FileMenuName = "File";
            FileMenuItems = new ObservableCollection<C64MenuItem>
            {
                new C64MenuItem()
                {
                    Name = "New...",
                    Command = new Command (CommandNewExecuted),
                    IsEnabled = true,
                    HotKey = "Ctrl+N"
                },
                new C64MenuItem()
                {
                    Name = "Open...",
                    Command = new Command (CommandOpenExecuted),
                    IsEnabled = true,
                    HotKey = "Ctrl+O"
                },
                new C64MenuItem()
                {
                    Name = "Save ...",
                    Command = new Command (CommandSaveExecuted),
                    IsEnabled = true,
                    HotKey = "Ctrl+S"
                },
                new C64MenuItem()
                {
                    Name = "Exit",
                    Command = new Command (CommandExitExecuted),
                    IsEnabled = true,
                    HotKey = "Ctrl+Q"
                },
            };
        }

        private void CommandNewExecuted(object obj)
        {
            throw new NotImplementedException();
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
            FileMenuContainer.IsVisible = false;
            DiskLabelGrid.IsVisible = false;

            C64FileDialogControl.IsVisible = true;
            C64FileDialogControl.Initialize(StartDirectory, ".d64");
        }

        
        /// <summary>
        /// User used save from the menu
        /// </summary>
        public void CommandSaveExecuted()
        {
            // Implement save functionality
        }

        private void OnFileSelected(object sender, string filePath)
        {
            FileMenuContainer.IsVisible = true;
            DiskLabelGrid.IsVisible = true;
            d64 = new D64(filePath);
            StartDirectory = C64FileDialogControl.CurrentDirectory;
        }

        private string SetStartDirectory()
        {
            string startDirectory = "";
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                startDirectory = "/storage/emulated/0/Download";
            }
            else if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                startDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                startDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            else if (DeviceInfo.Platform == DevicePlatform.MacCatalyst)
            {
                startDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            return startDirectory;
        }

        /// <summary>
        /// Property value change
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Property changed event handler
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ContentPage_Loaded(object sender, EventArgs e)
        {
            BuildMenus();

            FileMenuControl.C64MenuName = FileMenuName;
            FileMenuControl.C64MenuItems = FileMenuItems;
        }
    }
}

