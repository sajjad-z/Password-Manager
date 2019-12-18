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
    /// Interaction logic for card.xaml
    /// </summary>
    public partial class card : UserControl
    {
        public card()
        {
            InitializeComponent();
        }

        // add Yellow,Red 
        public enum myColors
        {
            Blue, Green, Purple, Orange, Pink
        }

        //------------------------------------------------------------------Color Dependency Property------------------------------------------------------------------
        public myColors Color
        {
            get { return (myColors)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(myColors), typeof(card), new PropertyMetadata(myColors.Blue, new PropertyChangedCallback(onColorChanged)));

        private static void onColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            card myCard = d as card;
            myCard.onColorChanged(e);
        }

        private void onColorChanged(DependencyPropertyChangedEventArgs e)
        {
            LinearGradientBrush gradientBrush = linearGradientBrush;

            switch (e.NewValue)
            {
                case myColors.Orange:
                    gradientStop1.Color = (Color)ColorConverter.ConvertFromString("#FF8187");
                    gradientStop1.Offset = 0;
                    gradientStop2.Color = (Color)ColorConverter.ConvertFromString("#FFB56A");
                    gradientStop2.Offset = 1;
                    break;
                case myColors.Pink:
                    gradientStop1.Color = (Color)ColorConverter.ConvertFromString("#FF6086");
                    gradientStop1.Offset = 0;
                    //gradientStop2.Color = (Color)ColorConverter.ConvertFromString("#FF80A9");
                    gradientStop2.Color = (Color)ColorConverter.ConvertFromString("#ffa7c4");
                    gradientStop2.Offset = 0.8;
                    break;
                case myColors.Purple:
                    gradientStop1.Color = (Color)ColorConverter.ConvertFromString("#7E87DD");
                    gradientStop1.Offset = 0;
                    gradientStop2.Color = (Color)ColorConverter.ConvertFromString("#B3B9F2");
                    gradientStop2.Offset = 0.85;
                    break;
                case myColors.Green:
                    gradientStop1.Color = (Color)ColorConverter.ConvertFromString("#45D8CE");
                    gradientStop1.Offset = 0.048;
                    gradientStop2.Color = (Color)ColorConverter.ConvertFromString("#FF4BECB8");
                    gradientStop2.Offset = 0.8;
                    break;
                case myColors.Blue:
                default:
                    gradientStop1.Color = (Color)ColorConverter.ConvertFromString("#4DADFF");
                    gradientStop1.Offset = 0;
                    gradientStop2.Color = (Color)ColorConverter.ConvertFromString("#03D5FE");
                    gradientStop2.Offset = 0.85;
                    break;
            }

            mainBorder.Background = gradientBrush;
        }

        //------------------------------------------------------------------Color Dependency Property------------------------------------------------------------------

        //------------------------------------------------------------------Text Dependency Property------------------------------------------------------------------
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(card), new PropertyMetadata("", new PropertyChangedCallback(onTextChanged)));

        private static void onTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            card myCard = d as card;
            myCard.onTextChanged(e);
        }

        private void onTextChanged(DependencyPropertyChangedEventArgs e)
        {
            txtText.Text = e.NewValue == null ? "" : e.NewValue.ToString();
        }
        
        //------------------------------------------------------------------Text Dependency Property------------------------------------------------------------------
    }
}
