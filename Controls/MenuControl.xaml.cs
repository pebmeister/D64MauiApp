// Written by Paul Baxter
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace D64MauiApp.Controls
{
    /// <summary>
    /// Menu user control
    /// </summary>
    public partial class MenuControl : ContentView, INotifyPropertyChanged
    {
        /// <summary>
        /// Bindable C64MenuName
        /// </summary>
        public static BindableProperty C64MenuNameProperty =
            BindableProperty.Create(nameof(C64MenuName), typeof(string), typeof(MenuControl), string.Empty, propertyChanged: OnMenuPropertyChanged);

        /// <summary>
        /// Bindable C64Commands
        /// </summary>
        public static readonly BindableProperty C64MenuItemsProperty =
            BindableProperty.Create(nameof(C64MenuItems), typeof(ObservableCollection<C64MenuItem>), typeof(MenuControl), new ObservableCollection<C64MenuItem>(), propertyChanged: OnMenuItemsPropertyChanged);

        /// <summary>
        /// Name of Menu
        /// </summary>
        public string C64MenuName
        {
            get => (string)GetValue(C64MenuNameProperty);
            set => SetValue(C64MenuNameProperty, value);
        }

        public ObservableCollection<C64MenuItem> C64MenuItems
        {
            get => (ObservableCollection<C64MenuItem>)GetValue(C64MenuItemsProperty);
            set => SetValue(C64MenuItemsProperty, value);
        }

        private bool _isPopupMenuVisible = false;
        public bool IsPopupMenuVisible
        {
            get => _isPopupMenuVisible;
            set
            {
                if (_isPopupMenuVisible != value)
                {
                    _isPopupMenuVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Dictionary to tie the command to menus
        /// </summary>
        private readonly Dictionary<string, ICommand> C64MenuCommands = new();

        /// <summary>
        /// Constructor
        /// </summary>
        public MenuControl()
        {
            InitializeComponent();
            BindingContext = this;
        }

        Style? LoadStyle(string styleName)
        {
            if (Application.Current.Resources.TryGetValue(styleName, out var style))
                return style as Style;

            return null;
        }

        /// <summary>
        /// Notify when a property is changed
        /// </summary>
        /// <param name="bindable">bound object</param>
        /// <param name="oldValue">old value</param>
        /// <param name="newValue">new value</param>
        private static void OnMenuPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is MenuControl control)
            {
                Debug.WriteLine($"Property changed: {control.C64MenuName}, {control.C64MenuItems.Count}");
                control.OnPropertyChanged(nameof(C64MenuName));
                control.BuildMenu();
            }
        }

        private static void OnMenuItemsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is MenuControl control)
            {
                Debug.WriteLine($"Menu items changed: {control.C64MenuItems.Count}");
                control.OnPropertyChanged(nameof(C64MenuItems));
                control.BuildMenu();
            }
        }

        /// <summary>
        /// Build the menu
        /// </summary>
        public void BuildMenu()
        {
            C64MenuCommands.Clear();
            foreach (var menuItem in C64MenuItems)
            {
                if (menuItem.Command  == null) continue;
                C64MenuCommands.Add(menuItem.Name, menuItem.Command);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void PopupMenuList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            IsPopupMenuVisible = false;
            MenuLabel.Style = LoadStyle("MenuStyle");
            var name = C64MenuItems[e.ItemIndex].Name;
            C64MenuCommands.TryGetValue(name, out ICommand menucommand);
            if (menucommand != null)
            {
                if (menucommand.CanExecute(name))
                {
                    menucommand.Execute(name);
                }
            }
        }


        private void MenuItemEntered(object sender, PointerEventArgs e)
        {
            if (sender is Label label)
            {
                label.Style = LoadStyle("PopupMenuEnteredStyle");
            }
        }

        private void MenuItemExited(object sender, PointerEventArgs e)
        {
            if (sender is Label label)
            {
                label.Style = LoadStyle("PopupMenuStyle");
            }
        }

        private void MenuTapped(object sender, TappedEventArgs e)
        {
            if (IsPopupMenuVisible)
            {
                MenuLabel.Style = LoadStyle("MenuStyle");
            }
            else
            {
                MenuLabel.Style = LoadStyle("MenuSelectedStyle");
            }
            IsPopupMenuVisible = !IsPopupMenuVisible;
        }

        private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {

        }
    }
}
