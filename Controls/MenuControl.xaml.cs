// Written by Paul Baxter
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace D64MauiApp.Controls
{
    /// <summary>
    /// Menu user control
    /// </summary>
    public partial class MenuControl : ContentView
    {
        /// <summary>
        /// Bindable C64MenuName
        /// </summary>
        public static readonly BindableProperty C64MenuNameProperty =
            BindableProperty.Create(nameof(C64MenuName), typeof(string), typeof(MenuControl), string.Empty, propertyChanged: OnMenuPropertyChanged);

        /// <summary>
        /// Binadable C64MenuItemNames
        /// </summary>
        public static readonly BindableProperty C64MenuItemNamesProperty =
            BindableProperty.Create(nameof(C64MenuItemNames), typeof(ObservableCollection<string>), typeof(MenuControl), new ObservableCollection<string>(), propertyChanged: OnMenuPropertyChanged);

        /// <summary>
        /// Binadable C64Commands
        /// </summary>
        public static readonly BindableProperty C64CommandsProperty =
            BindableProperty.Create(nameof(C64Commands), typeof(ObservableCollection<ICommand>), typeof(MenuControl), new ObservableCollection<ICommand>(), propertyChanged: OnMenuPropertyChanged);

        /// <summary>
        /// Name of Menu
        /// </summary>
        public string C64MenuName
        {
            get => (string)GetValue(C64MenuNameProperty);
            set => SetValue(C64MenuNameProperty, value);
        }

        /// <summary>
        /// Menu names to build
        /// </summary>
        public ObservableCollection<string> C64MenuItemNames
        {
            get => (ObservableCollection<string>)GetValue(C64MenuItemNamesProperty);
            set => SetValue(C64MenuItemNamesProperty, value);
        }

        /// <summary>
        /// Commands for menus
        /// </summary>
        public ObservableCollection<ICommand> C64Commands
        {
            get => (ObservableCollection<ICommand>)GetValue(C64CommandsProperty);
            set => SetValue(C64CommandsProperty, value);
        }

        /// <summary>
        /// Dictionary to tie the command to menus
        /// </summary>
        private readonly Dictionary<ContentView, ICommand> C64MenuCommands = new();


        /// <summary>
        /// Constructor
        /// </summary>
        public MenuControl()
        {
            InitializeComponent();
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
                Debug.WriteLine($"Property changed: {control.C64MenuName}, {control.C64MenuItemNames.Count}, {control.C64Commands.Count}");
                control.BuildMenu();
            }
        }

        /// <summary>
        /// Build the menu
        /// </summary>
        public void BuildMenu()
        {
            Debug.WriteLine("Building menu...");
            MenuStackLayout.Children.Clear();
            PopupMenuStack.Children.Clear();

            var menuCount = C64MenuItemNames.Count;
            var commandCount = C64Commands.Count;

            if (menuCount != commandCount)
            {
                Debug.WriteLine("menuCount != commandCount... exit");
                return;
                // Don't throw exception since only one thing can be added at a time.
                // throw new Exception($"MenuCount {menuCount} does not equal command count {commandCount}");
            }

            // build the top menu
            var menuLabel = new Label
            {
                Style = (Style)Application.Current.Resources["MenuLabelStyle"],
                Text = C64MenuName
            };
            var gestureRecognizer = new PointerGestureRecognizer();
            gestureRecognizer.PointerPressed += OnPointerPressed;
            menuLabel.GestureRecognizers.Add(gestureRecognizer);
            MenuStackLayout.Children.Add(menuLabel);

            for (int i = 0; i < menuCount; i++)
            {
                var menuitemLabel = new Label
                {
                    Style = (Style)Application.Current.Resources["PopupMenuStyle"],
                    Text = C64MenuItemNames[i]
                };

                var menuitemContainer = new ContentView
                {
                    Style = (Style)Application.Current.Resources["PopupMenuContainerStyle"],
                    Content = menuitemLabel
                };

                var menugestureRecognizer = new PointerGestureRecognizer();
                menugestureRecognizer.PointerEntered += (s, e) =>
                {
                    menuitemContainer.Style = (Style)Application.Current.Resources["PopupMenuContainerEnteredStyle"];
                    menuitemLabel.Style = (Style)Application.Current.Resources["PopupMenuEnteredStyle"];
                };
                menugestureRecognizer.PointerExited += (s, e) =>
                {
                    menuitemContainer.Style = (Style)Application.Current.Resources["PopupMenuContainerStyle"];
                    menuitemLabel.Style = (Style)Application.Current.Resources["PopupMenuStyle"];
                };
                menugestureRecognizer.PointerReleased += MenuItemlOnExitTapped;
                menuitemContainer.GestureRecognizers.Add(menugestureRecognizer);

                var menugestureTapRecognizer = new TapGestureRecognizer();
                menugestureTapRecognizer.Tapped += MenuItemlOnExitTapped;
                menuitemContainer.GestureRecognizers.Add(menugestureTapRecognizer);

                C64MenuCommands.Add(menuitemContainer, C64Commands[i]);

                PopupMenuStack.Children.Add(menuitemContainer);
            }
        }

        /// <summary>
        /// Pointer pressed on menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPointerPressed(object sender, PointerEventArgs e)
        {
            ShowPopupMenu();
        }

        /// <summary>
        /// Pointere exited the menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPointerExited(object sender, PointerEventArgs e)
        {
            HidePopupMenu();
        }

        /// <summary>
        /// Show the menuitems
        /// </summary>
        private void ShowPopupMenu()
        {
            MenuStackLayout.Style = Application.Current.Resources["MenuStackEnteredLayoutStyle"] as Style;
            PopupMenu.IsVisible = true;
        }

        /// <summary>
        /// Hide the menuitems
        /// </summary>
        private void HidePopupMenu()
        {
            MenuStackLayout.Style = Application.Current.Resources["MenuStackLayoutStyle"] as Style;
            PopupMenu.IsVisible = false;
        }

        /// <summary>
        /// The user selected the menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemlOnExitTapped(object sender, EventArgs e)
        {
            if (sender is ContentView content)
            {
                HidePopupMenu();
                if (C64MenuCommands.TryGetValue(content, out var cmd))
                {
                    cmd.Execute(e);
                }
            }
        }
    }
}
