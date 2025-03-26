using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using d64Wrapper;

namespace D64MauiApp
{
    public class DirectoryEntryViewModel : INotifyPropertyChanged
    {
        public DirectoryEntryViewModel()
        { }

        public DirectoryEntryViewModel(DirectoryEntry dirEntry)
        {
            FileType = dirEntry.FileType;
            Start = dirEntry.Start;
            FileName = dirEntry.FileName;
            Side = dirEntry.Side;
            RecordLength = dirEntry.RecordLength;
            Replace = dirEntry.Replace;
            FileSize = dirEntry.FileSize;
        }

        private C64FileType _fileType;
        public C64FileType FileType
        {
            get => _fileType;
            set
            {
                _fileType = value;
                OnPropertyChanged();
            }
        }


        private TrackSector? _start;
        public TrackSector? Start
        {
            get => _start;
            set
            {
                _start = value;
                OnPropertyChanged();
            }
        }

        private String? _fileName;
        public String? FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                OnPropertyChanged();
            }
        }

        private TrackSector? _side;
        public TrackSector? Side
        {
            get => _side;
            set
            {
                _side = value;
                OnPropertyChanged();
            }
        }

        private int _recordLength;
        public int RecordLength
        {
            get => _recordLength;
            set
            {
                _recordLength = value;  
                OnPropertyChanged();
            }
        }

        private TrackSector? _replace;
        public TrackSector? Replace
        {
            get => _replace;
            set
            {
                _replace = value;
                OnPropertyChanged();
            }
        }

        private int _fileSize;
        public int FileSize
        {
            get => _fileSize;
            set
            {
                _fileSize = value;
                OnPropertyChanged();
            }
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
