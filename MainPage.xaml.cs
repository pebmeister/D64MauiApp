// written by Paul Baxter
using D64MauiApp.Controls;
using d64Wrapper;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Threading.Tasks;

namespace D64MauiApp
{
    /// <summary>
    /// MainPage class
    /// </summary>
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        private D64? _d64File;
        private Bam? _bam;

        private ObservableCollection<DirectoryEntryViewModel> _c64DiskFiles = new();
        public ObservableCollection<DirectoryEntryViewModel> C64DiskFiles
        {
            get => _c64DiskFiles;
            set
            {
                _c64DiskFiles = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<C64MenuItem> _fileMenuItems = new();
        private string? _fileMenuName;

        private string _startDirectory = "";
        /// <summary>
        /// Start directory for file open
        /// </summary>
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

        private string? _d64DiskName;
        /// <summary>
        /// Name of the .D64File disk
        /// </summary>
        public string D64DiskName
        {
            get => string.IsNullOrEmpty(_d64DiskName) ? "" : _d64DiskName;
            set
            {
                _d64DiskName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// D64 disk that is open
        /// </summary>
        public D64? D64File
        {
            get => _d64File;
            set
            {
                _d64File = value;
                OnPropertyChanged();
                if (D64File != null)
                {
                    D64DiskName = "0 " + InvertC64String("\"" + D64File.DiskName().PadRight(16) + "\"");
                    C64DiskFiles.Clear();
                    var files = D64File.GetFiles();
                    foreach (var file in files)
                    {
                        C64DiskFiles.Add( new DirectoryEntryViewModel(file));
                    }
                }
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
        public string? FileMenuName
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
        }

        private static string InvertC64String(string input)
        {
            return new string(input.Select(c => InvertC64Char(c)).ToArray());
        }

        private static char InvertC64Char(char c)
        {
            if (c >= ' ' && c <= '?') // Space to ?     0x20 - 0x3F
                return (char)('\uE220' + (c - ' '));

            if (c >= '@' && c <= 'Z') // @ to Z         0x40 - 0x5A
                return (char)('\uE240' + (c - '@'));

            if (c >= 'a' && c <= 'z') // a to z         0x61 - 0x7A
                return (char)('\uEF81' + (c - 'a'));


            // Add mappings for common C64 graphic characters
            return c switch
            {
                '\u2500' => '\uE280', // ─
                '\u2502' => '\uE281', // │
                '\u250C' => '\uE282', // ┌
                '\u2510' => '\uE283', // ┐
                '\u2514' => '\uE284', // └
                '\u2518' => '\uE285', // ┘
                '\u251C' => '\uE286', // ├
                '\u2524' => '\uE287', // ┤
                '\u252C' => '\uE288', // ┬
                '\u2534' => '\uE289', // ┴
                '\u253C' => '\uE28A', // ┼
                '\u2580' => '\uE28B', // ▀
                '\u2584' => '\uE28C', // ▄
                '\u2588' => '\uE28D', // █
                '\u2591' => '\uE28E', // ░
                '\u2592' => '\uE28F', // ▒
                '\u2593' => '\uE290', // ▓
                _ => c // Default to the original character if no mapping is found
            };
        }

        /// <summary>
        /// Build the file menu
        /// </summary>
        private void BuildMenuCommands()
        {
            FileMenuName = "File";
            FileMenuItems =
            [
                new()
                {
                    Name = "New",
                    Command = new AsyncRelayCommand(CommandNewExecutedAsync),
                    IsEnabled = true,
                },
                new()
                {
                    Name = "Open ...",
                    Command = new AsyncRelayCommand(CommandOpenExecutedAsync),
                    IsEnabled = true,
                },
                new()
                {
                    Name = "Save ...",
                    Command = new AsyncRelayCommand(CommandSaveExecutedAsync),
                    IsEnabled = true,
                },
                new()
                {
                    Name = "Exit",
                    Command = new AsyncRelayCommand(CommandExitExecutedAsync),
                    IsEnabled = true,
                },
            ];
        }

        /// <summary>
        /// New Executed
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private async Task CommandNewExecutedAsync()
        {
            await Task.Run(() => throw new NotImplementedException());
        }

        /// <summary>
        /// User used exit from the menu
        /// </summary>
        public async Task CommandExitExecutedAsync()
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
                Application.Current?.Quit());
        }

        /// <summary>
        /// User used open from the menu
        /// </summary>
        public async Task CommandOpenExecutedAsync()
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                FileMenuContainer.IsVisible = false;
                DiskLabelGrid.IsVisible = false;

                C64FileDialogControl.IsVisible = true;
                C64FileDialogControl.Initialize(StartDirectory, ".d64");
            });
        }

        /// <summary>
        /// User used save from the menu
        /// </summary>
        public async Task CommandSaveExecutedAsync()
        {
            await Task.Run(() =>
            {
                // Implement save functionality
            });
        }

        /// <summary>
        /// File is selected from the open file dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="filePath"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public void OnFileSelected(
            object sender,
            string filePath)
        {
            C64FileDialogControl.IsVisible = false;
            FileMenuContainer.IsVisible = true;
            DiskLabelGrid.IsVisible = true;
            D64File = new D64(filePath);
            StartDirectory = C64FileDialogControl.CurrentDirectory;
        }

        /// <summary>
        /// Set the startdirectory based on Platform
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Page laoded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContentPage_Loaded(object sender, EventArgs e)
        {
            BuildMenuCommands();
            FileMenuControl.C64MenuName = FileMenuName;
            FileMenuControl.C64MenuItems = FileMenuItems;
        }
    }
}

