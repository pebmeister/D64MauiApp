using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace D64MauiApp.Controls
{
    public partial class C64FileDialog : ContentView, INotifyPropertyChanged
    {
        public ObservableCollection<FileSystemInfoViewModel> Files { get; set; } = new ObservableCollection<FileSystemInfoViewModel>();
        private string _currentDirectory;
        public string CurrentDirectory
        {
            get => _currentDirectory;
            set
            {
                _currentDirectory = value;
                OnPropertyChanged();
            }
        }

        private string _fileFilter;

        public string FileFilter
        {
            get => _fileFilter;
            set
            {
                _fileFilter = value;
                OnPropertyChanged();
            }
        }

        private string _startDirectory;
        public string StartDirectory
        {
            get => _startDirectory;
            set
            {
                _startDirectory = value;
                OnPropertyChanged();
            }
        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get => _cancelCommand;
            set
            {
                _cancelCommand = value;
                OnPropertyChanged();
            }
        }

        private ICommand _openCommand;
        public ICommand OpenCommand
        {
            get => _openCommand;
            set
            {
                _openCommand = value;
                OnPropertyChanged();
            }
        }

        public event EventHandler<string> FileSelected = delegate { };

        public C64FileDialog()
        {
            InitializeComponent();
            BindingContext = this;

            CancelCommand = new Command(OnCancel);
            OpenCommand = new Command(OnOpen);
        }

        public void Initialize(string startDirectory, string fileFilter)
        {
            StartDirectory = startDirectory;
            FileFilter = fileFilter;
            LoadFiles(startDirectory);
        }

        private void LoadFiles(string path)
        {
            Files.Clear();
            CurrentDirectory = path;
            var directoryInfo = new DirectoryInfo(path);
            if (directoryInfo.Exists)
            {
                // Add ".." entry to navigate up a directory
                if (directoryInfo.Parent != null)
                {
                    var fsInfo = new FileSystemInfoViewModel(directoryInfo.Parent)
                    {
                        Name = "..",
                        IsDirectory = true
                    };
                    Files.Add(fsInfo);
                }
                List<FileSystemInfoViewModel> directories = new List<FileSystemInfoViewModel>();
                List<FileSystemInfoViewModel> files = new List<FileSystemInfoViewModel>();

                try
                {
                    directories = directoryInfo.GetDirectories().Select(dir => new FileSystemInfoViewModel(dir)).ToList();
                    foreach (var dir in directories)
                    {
                        Files.Add(dir);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception iterating directories {ex.Message}");
                }
                try
                {
                    files = directoryInfo.GetFiles().Where(file => string.IsNullOrEmpty(FileFilter) || FileFilter == "*" || string.Compare(FileFilter, file.Extension, true) == 0)
                            .Select(file => new FileSystemInfoViewModel(file)).ToList();
                    foreach (var file in files)
                    {
                        Files.Add(file);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception iterating files {ex.Message}");
                }

            }
        }

        private async void OnFileListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is FileSystemInfoViewModel viewModel)
            {
                if (viewModel.IsDirectory)
                {
                    LoadFiles(viewModel.FullName);
                }
                else
                {
                    IsVisible = false;
                    ExecuteOpenCommand(viewModel.FullName);
                }
            }
        }

        private void ExecuteOpenCommand(string name)
        {
            if (OpenCommand.CanExecute(name))
            {
                OpenCommand.Execute(name);
            }
        }

        private void OnCancel()
        {
            IsVisible = false;
            FileSelected?.Invoke(this, "");
        }

        private void OnOpen(object parameter)
        {
            IsVisible = false;
            FileSelected?.Invoke(this, parameter.ToString());
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
    }
}

