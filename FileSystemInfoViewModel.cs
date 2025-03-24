using System.ComponentModel;
using System.Runtime.CompilerServices;

public class FileSystemInfoViewModel : INotifyPropertyChanged
{
    private string _name = "";
    public string Name { get; set; }
    private string _fullName = "";
    public string FullName { get; set; }
    private FileSystemInfo _fileSystemInfo;
    public FileSystemInfo FileSystemInfo { get; set; }
    public bool IsDirectory { get; set; }

    public FileSystemInfoViewModel(FileSystemInfo fileSystemInfo)
    {
        FileSystemInfo = fileSystemInfo;
        Name = fileSystemInfo.Name;
        FullName = fileSystemInfo.FullName;
        IsDirectory = fileSystemInfo is DirectoryInfo;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
