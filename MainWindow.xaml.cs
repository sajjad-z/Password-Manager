using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MaterialDesignThemes.Wpf;
using Models;
using Password_Manager.MyControls;
using PeanutButter.Toast;

namespace Password_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataBase<tbl_Password> dbPassword;
        DataBase<tbl_Setting> dbSetting;
        Toaster _Toaster;
        int _PassId = 0;

        public MainWindow()
        {
            InitializeComponent();

            // load default personal settings 
            this.Width = Properties.Settings.Default.lastWidth;
            this.Height = Properties.Settings.Default.lastHeight;

            // first Init
            dbPassword = new DataBase<tbl_Password>();
            dbSetting = new DataBase<tbl_Setting>();
            _Toaster = new Toaster();

            isProtectionEnable();
            loadData();
            loadSetting();

            // display cardShow in default
            menuShow_Click(null, null);

            // fill colors comboBox
            comboColor.ItemsSource = Enum.GetValues(typeof(card.myColors)).Cast<card.myColors>();
            // set default Value
            comboColor.SelectedIndex = 0;
        }

        // display login page or not
        private void isProtectionEnable()
        {
            try
            {
                if (dbSetting.Select(1).isLock)
                {
                    loginControl.Visibility = Visibility.Visible;
                }
                else
                {
                    loginControl.Visibility = Visibility.Collapsed;
                }
            }
            catch { }
        }

        // load passwords data from db in wrapPnael
        void loadData()
        {
            passwordPanel.Children.Clear();
            foreach (var item in dbPassword.Select())
            {
                card myCard = new card();
                myCard.Text = item.name;
                myCard.Color = (card.myColors)Enum.Parse(typeof(card.myColors), item.color);
                myCard.MouseLeftButtonUp += MyCard_MouseLeftButtonUp;
                myCard.Tag = item.id;
                passwordPanel.Children.Add(myCard);
            }
        }

        // load settings from db in wrapPnael
        void loadSetting()
        {
            tbl_Setting setting = dbSetting.Select(1);
            if (setting == null)
            {
                setting = new tbl_Setting()
                {
                    id = 1,
                    isDark = false,
                    isLock = false,
                    password = "",
                    username = "admin"
                };
                dbSetting.Insert(setting);
            }
            toggleDark.IsChecked = setting.isDark;
            toggleProtection.IsChecked = setting.isLock;

            // set Dark ui 
            if (setting.isDark)
            {
                titleBar.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3A4047"));
                mainGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#282E33"));
                minimizeIcon.Foreground = maximizeIcon.Foreground = closeIcon.Foreground = new SolidColorBrush(Colors.White);

                cardSetting.Style = cardAdd.Style = cardShow.Style = (Style)Application.Current.Resources["customCardDark"];
            }
            // set Light ui 
            else
            {
                titleBar.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F1F1F1"));
                mainGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                minimizeIcon.Foreground = maximizeIcon.Foreground = closeIcon.Foreground = new SolidColorBrush(Colors.DimGray);

                cardSetting.Style = cardAdd.Style = cardShow.Style = (Style)Application.Current.Resources["customCardLight"];
            }
        }

        private void MyCard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _PassId = (int)(((card)sender).Tag);
            menuShow_Click(null, null);
        }

        // reset all filed
        void clear()
        {
            txtDesc.Text = txtPassword.Text = txtTitle.Text = txtUsername.Text = "";
        }

        //show alert dialog
        void alertMessage(string title, string message, ToastTypes type)
        {
            bool isPersistent = false;
            bool showInWindow = false;

            if (showInWindow)
            {
                Rect bounds = new Rect(
                    this.Left,
                    this.Top,
                    this.Width,
                    this.Height);

                _Toaster.Show(title, message, type, bounds, isPersistent);
            }
            else
                _Toaster.Show(title, message, type, isPersistent: isPersistent);
        }

        // add new password to db
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txtTitle.Text == "")
            {
                alertMessage("", "", ToastTypes.Error);
                return;
            }

            tbl_Password pass = new tbl_Password()
            {
                color = comboColor.Text,
                description = txtDesc.Text,
                name = txtTitle.Text,
                password = txtPassword.Text,
                username = txtUsername.Text
            };

            if (dbPassword.Insert(pass))
            {
                loadData();
                alertMessage("ok", "ok", ToastTypes.Success);
                clear();
            }
            else
            {
                alertMessage("nnnnnnnnn", "nnnnnnnnnnnnn", ToastTypes.Error);
            }
        }

        #region menu Animations
        Card cardFrom = new Card();
        Card cardTo = new Card();

        void runAnimation(Card c)
        {
            cardTo = c;
            if (cardShow.Visibility == Visibility.Visible)
            {
                cardFrom = cardShow;
            }
            else if (cardSetting.Visibility == Visibility.Visible)
            {
                cardFrom = cardSetting;
            }
            else if (cardAdd.Visibility == Visibility.Visible)
            {
                cardFrom = cardAdd;
            }

            DoubleAnimation animation0 = new DoubleAnimation();
            animation0.From = cardFrom.Opacity;
            animation0.To = 0;
            animation0.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            animation0.Completed += Animation0_Completed;
            cardFrom.BeginAnimation(OpacityProperty, animation0);

            //DoubleAnimation animation1 = new DoubleAnimation();
            //animation1.From = cardFrom.ActualHeight;
            //animation1.To = cardFrom.ActualHeight - 100;
            //animation1.Duration = new Duration(TimeSpan.FromMilliseconds(150));
            //cardFrom.BeginAnimation(HeightProperty, animation1);

            //DoubleAnimation animation2 = new DoubleAnimation();
            //animation2.From = cardFrom.ActualWidth;
            //animation2.To = cardFrom.ActualWidth - 100;
            //animation2.Duration = new Duration(TimeSpan.FromMilliseconds(150));
            //cardFrom.BeginAnimation(WidthProperty, animation2);
        }

        private void Animation0_Completed(object sender, EventArgs e)
        {
            // opacity animation
            DoubleAnimation animation0 = new DoubleAnimation();
            animation0.From = cardFrom.Opacity;
            animation0.To = 1;
            animation0.Duration = new Duration(TimeSpan.FromMilliseconds(1));
            cardFrom.BeginAnimation(OpacityProperty, animation0);

            cardFrom.Visibility = Visibility.Collapsed;

            // width animation
            cardTo.Visibility = Visibility.Visible;
            DoubleAnimation animation1 = new DoubleAnimation();
            animation1.From = 0;
            animation1.To = 270;
            animation1.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            cardTo.BeginAnimation(WidthProperty, animation1);

            if (_PassId != 0)
            {
                lblDesc.Visibility = lblUsername.Visibility = lblPassword.Visibility = lblTitle.Visibility = btnDelete.Visibility = btnEdit.Visibility = Visibility.Visible;

                tbl_Password password = dbPassword.Select(_PassId);
                lblTitle.Text = password.name;
                lblPassword.Text = password.password;
                lblUsername.Text = password.username;
                lblDesc.Text = password.description;
            }
            else
            {
                lblDesc.Visibility = lblUsername.Visibility = lblPassword.Visibility = lblTitle.Visibility = btnDelete.Visibility = btnEdit.Visibility = Visibility.Hidden;
            }
        }

        private void menuSetting_Click(object sender, RoutedEventArgs e)
        {
            runAnimation(cardSetting);
            loadSetting();
        }

        private void menuAdd_Click(object sender, RoutedEventArgs e)
        {
            runAnimation(cardAdd);
        }

        private void menuShow_Click(object sender, RoutedEventArgs e)
        {
            runAnimation(cardShow);
        }
        #endregion

        #region titleBar
        // close program
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // minimize program
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // maximize program
        private void Maximized_Click(object sender, RoutedEventArgs e)
        {
            AdjustWindowSize();
        }

        // move program by titleBar
        private void titleBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.ClickCount == 2)
                {
                    AdjustWindowSize();
                }
                else
                {
                    Application.Current.MainWindow.DragMove();
                }
            }
        }

        // Adjusts the WindowSize to correct parameters when Maximize button is clicked
        private void AdjustWindowSize()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }
        #endregion

        // save new setting in db
        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            tbl_Setting setting = dbSetting.Select(1);
            if (setting == null)
            {
                setting = new tbl_Setting()
                {
                    id = 1,
                    isDark = toggleDark.IsChecked ?? false,
                    isLock = toggleProtection.IsChecked ?? false,
                    username = "admin"
                };
                if (passwordBox.Password == passwordBoxRepeat.Password) setting.password = passwordBox.Password; else { alertMessage("err", "err", ToastTypes.Error); return; };

                dbSetting.Insert(setting);
            }
            else
            {
                if (passwordBox.Password == passwordBoxRepeat.Password) setting.password = passwordBox.Password; else { alertMessage("err", "err", ToastTypes.Error); return; };
                setting.isLock = toggleProtection.IsChecked ?? false;
                setting.isDark = toggleDark.IsChecked ?? false;

                dbSetting.Update(setting);
            }
            loadSetting();
        }

        // delete a password from db
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dbPassword.Delete(_PassId))
            {
                _PassId = 0;
                menuShow_Click(null, null);
                alertMessage("", "", ToastTypes.Success);
                loadData();
            }
            else
            {
                alertMessage("", "", ToastTypes.Success);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        // change display type (row/tiles)
        void changeDisplay()
        {

        }

        private void btnOrderTile_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.showType = "tile";
            Properties.Settings.Default.Save();
        }

        private void btnOrderRow_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.showType = "row";
            Properties.Settings.Default.Save();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Properties.Settings.Default.lastHeight = Convert.ToInt32(this.ActualHeight);
            Properties.Settings.Default.lastWidth = Convert.ToInt32(this.ActualWidth);
            Properties.Settings.Default.Save();
        }

        private void loginControl_LoginClick(object sender, RoutedEventArgs e)
        {
            if ((dbSetting.Select(1).password ?? loginControl.txtPass.Text) == loginControl.txtPass.Text)
            {
                // End of Animation for login user control
                DoubleAnimation animation = new DoubleAnimation();
                animation.From = loginControl.ActualHeight;
                animation.To = 0;
                animation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
                animation.Completed += Animation_Completed; ;
                loginControl.BeginAnimation(HeightProperty, animation);
            }
            else
            {
                alertMessage("Wrong", "Password Is Not Valid", ToastTypes.Error);
            }
        }

        private void Animation_Completed(object sender, EventArgs e)
        {
            loginControl.Visibility = Visibility.Collapsed;
        }
    }
}
