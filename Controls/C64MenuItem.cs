// written by Paul Baxter
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace D64MauiApp.Controls
{
    public class C64MenuItem : INotifyPropertyChanged
    {
        private string _name = "";
        private ICommand? _command;
        private bool _isEnabled = true;
        private string _hotKey = "";

        public string Name 
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public ICommand? Command
        {
            get => _command;
            set
            {
                _command = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled 
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            } 
        }

        public string HotKey 
        {
            get => _hotKey; 
            set
            {
                _hotKey = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

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
