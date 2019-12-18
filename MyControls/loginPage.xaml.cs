using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Password_Manager.MyControls
{
    /// <summary>
    /// Interaction logic for loginPage.xaml
    /// </summary>
    public partial class loginPage : UserControl
    {
        public loginPage()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler LoginClick;

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (LoginClick != null)
            {
                LoginClick(this, new RoutedEventArgs());
            }
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginClick(this, new RoutedEventArgs());
            }
        }
    }
}
