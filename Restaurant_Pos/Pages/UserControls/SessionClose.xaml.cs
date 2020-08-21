using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Restaurant_Pos.Pages.UserControls
{
    /// <summary>
    /// Interaction logic for SessionClose.xaml
    /// </summary>
    public partial class SessionClose : UserControl
    {
        public SessionClose()
        {
            InitializeComponent();
        }

        #region Session Close Screens

        private string rb_Name;

        private void R_sessionClose_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            rb_Name = rb.Name;
        }

        private void R_sessionClose_Click(object sender, RoutedEventArgs e)
        {
            ScessionClose_ScreenChange(rb_Name);
        }

        private void ScessionClose_ScreenChange(string Name)
        {
            switch (Name)
            {
                case "R_sessionClose_Denomination":
                    SessionClose_Denomination.Visibility = Visibility.Visible;
                    SessionClose_OnlyTotal.Visibility = Visibility.Hidden;
                    SessionClose_NoSale.Visibility = Visibility.Hidden;
                    SessionClose_empty_inputs();
                    Keyboard.Focus(SessionClose_card_input);
                    break;

                case "R_sessionClose_OnlyTotal":
                    SessionClose_Denomination.Visibility = Visibility.Hidden;
                    SessionClose_OnlyTotal.Visibility = Visibility.Visible;
                    SessionClose_NoSale.Visibility = Visibility.Hidden;
                    SessionClose_empty_inputs();
                    Keyboard.Focus(SessionClose_only_total_input);
                    break;

                case "R_sessionClose_No_Sale":
                    SessionClose_Denomination.Visibility = Visibility.Hidden;
                    SessionClose_OnlyTotal.Visibility = Visibility.Hidden;
                    SessionClose_NoSale.Visibility = Visibility.Visible;
                    SessionClose_empty_inputs();
                    break;

                default:
                    break;
            }
        }

        private void SessionClose_empty_inputs()
        {
            SessionClose_card_input.Text = String.Empty;
            SessionClose_card_total.Text = "0.00";
            SessionClose_500x_input.Text = String.Empty;
            SessionClose_500x_total.Text = "0.00";
            SessionClose_100x_input.Text = String.Empty;
            SessionClose_100x_total.Text = "0.00";
            SessionClose_50x_input.Text = String.Empty;
            SessionClose_50x_total.Text = "0.00";
            SessionClose_10x_input.Text = String.Empty;
            SessionClose_10x_total.Text = "0.00";
            SessionClose_5x_input.Text = String.Empty;
            SessionClose_5x_total.Text = "0.00";
            SessionClose_1x_input.Text = String.Empty;
            SessionClose_1x_total.Text = "0.00";
            SessionClose_50dx_input.Text = String.Empty;
            SessionClose_50dx_total.Text = "0.00";
            SessionClose_25dx_input.Text = String.Empty;
            SessionClose_25dx_total.Text = "0.00";
            SessionClose_only_total_input.Text = String.Empty;
            SessionClose_grand_total.Text = "0.00";
        }

        private string SessionClose_input_GotFocus_field;

        private void SessionClose_input_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox s_text_input = sender as TextBox;
            SessionClose_input_GotFocus_field = s_text_input.Name;

            switch (SessionClose_input_GotFocus_field)
            {
                case "SessionClose_card_input":
                    SessionClose_card_input.Select(SessionClose_card_input.Text.Length, 0);
                    break;

                case "SessionClose_500x_input":
                    SessionClose_500x_input.Select(SessionClose_500x_input.Text.Length, 0);
                    break;

                case "SessionClose_100x_input":
                    SessionClose_100x_input.Select(SessionClose_100x_input.Text.Length, 0);
                    break;

                case "SessionClose_50x_input":
                    SessionClose_50x_input.Select(SessionClose_50x_input.Text.Length, 0);
                    break;

                case "SessionClose_10x_input":
                    SessionClose_10x_input.Select(SessionClose_10x_input.Text.Length, 0);
                    break;

                case "SessionClose_5x_input":
                    SessionClose_5x_input.Select(SessionClose_5x_input.Text.Length, 0);
                    break;

                case "SessionClose_1x_input":
                    SessionClose_1x_input.Select(SessionClose_1x_input.Text.Length, 0);
                    break;

                case "SessionClose_50dx_input":
                    SessionClose_50dx_input.Select(SessionClose_50dx_input.Text.Length, 0);
                    break;

                case "SessionClose_25dx_input":
                    SessionClose_25dx_input.Select(SessionClose_25dx_input.Text.Length, 0);
                    break;

                case "SessionClose_only_total_input":
                    SessionClose_only_total_input.Select(SessionClose_only_total_input.Text.Length, 0);
                    break;

                default:
                    break;
            }
        }

        #endregion Session Close Screens

        private void SessionClose_KeyPad_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)e.OriginalSource;

            string s = btn.Content.ToString();
            switch (SessionClose_input_GotFocus_field)
            {
                case "SessionClose_card_input":
                    SessionClose_card_input.Text += s;

                    SessionClose_card_total.Text = Convert.ToDouble(SessionClose_card_input.Text).ToString("0.00");
                    SessionClose_card_input.Select(SessionClose_card_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_500x_input":
                    SessionClose_500x_input.Text += s;

                    SessionClose_500x_total.Text = (Convert.ToDouble(SessionClose_500x_input.Text) * 500).ToString("0.00");
                    SessionClose_500x_input.Select(SessionClose_500x_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_100x_input":
                    SessionClose_100x_input.Text += s;

                    SessionClose_100x_total.Text = (Convert.ToDouble(SessionClose_100x_input.Text) * 100).ToString("0.00");
                    SessionClose_100x_input.Select(SessionClose_100x_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_50x_input":
                    SessionClose_50x_input.Text += s;

                    SessionClose_50x_total.Text = (Convert.ToDouble(SessionClose_50x_input.Text) * 50).ToString("0.00");
                    SessionClose_50x_input.Select(SessionClose_50x_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_10x_input":
                    SessionClose_10x_input.Text += s;

                    SessionClose_10x_total.Text = (Convert.ToDouble(SessionClose_10x_input.Text) * 10).ToString("0.00");
                    SessionClose_10x_input.Select(SessionClose_10x_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_5x_input":
                    SessionClose_5x_input.Text += s;

                    SessionClose_5x_total.Text = (Convert.ToDouble(SessionClose_5x_input.Text) * 5).ToString("0.00");
                    SessionClose_5x_input.Select(SessionClose_5x_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_1x_input":
                    SessionClose_1x_input.Text += s;

                    SessionClose_1x_total.Text = (Convert.ToDouble(SessionClose_1x_input.Text) * 1).ToString("0.00");
                    SessionClose_1x_input.Select(SessionClose_1x_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_50dx_input":
                    SessionClose_50dx_input.Text += s;

                    SessionClose_50dx_total.Text = (Convert.ToDouble(SessionClose_50dx_input.Text) * 50).ToString("0.00");
                    SessionClose_50dx_input.Select(SessionClose_50dx_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_25dx_input":
                    SessionClose_25dx_input.Text += s;

                    SessionClose_25dx_total.Text = (Convert.ToDouble(SessionClose_25dx_input.Text) * 25).ToString("0.00");
                    SessionClose_25dx_input.Select(SessionClose_25dx_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_only_total_input":
                    SessionClose_only_total_input.Text += s;
                    SessionClose_only_total_input.Text = SessionClose_only_total_input.Text;
                    SessionClose_grand_total.Text = SessionClose_only_total_input.Text;
                    SessionClose_only_total_input.Select(SessionClose_only_total_input.Text.Length, 0);
                    break;

                default:
                    break;
            }
        }

        private void SessionClose_KeyPadErase_Click(object sender, RoutedEventArgs e)
        {
            if (SessionClose_input_GotFocus_field == "SessionClose_card_input" && SessionClose_card_input.Text != String.Empty)
            {
                SessionClose_card_input.Text = SessionClose_card_input.Text.Remove(SessionClose_card_input.Text.Length - 1);
                if (SessionClose_card_input.Text == String.Empty)
                {
                    SessionClose_card_total.Text = "0.00";
                    SessionClose_grand_total_cal();
                    return;
                }
                SessionClose_card_total.Text = Convert.ToDouble(SessionClose_card_input.Text).ToString("0.00");
                SessionClose_card_input.Select(SessionClose_card_input.Text.Length, 0);
                SessionClose_grand_total_cal();
            }
            if (SessionClose_input_GotFocus_field == "SessionClose_500x_input" && SessionClose_500x_input.Text != String.Empty)
            {
                SessionClose_500x_input.Text = SessionClose_500x_input.Text.Remove(SessionClose_500x_input.Text.Length - 1);
                if (SessionClose_500x_input.Text == String.Empty)
                {
                    SessionClose_500x_total.Text = "0.00";
                    SessionClose_grand_total_cal();
                    return;
                }
                SessionClose_500x_total.Text = (Convert.ToDouble(SessionClose_500x_input.Text) * 500).ToString("0.00");

                //SessionClose_500x_total.Text = (Convert.ToDouble(SessionClose_500x_input.Text) * 25).ToString("0.00");
                SessionClose_500x_input.Select(SessionClose_500x_input.Text.Length, 0);
                SessionClose_grand_total_cal();
            }
            if (SessionClose_input_GotFocus_field == "SessionClose_100x_input" && SessionClose_100x_input.Text != String.Empty)
            {
                SessionClose_100x_input.Text = SessionClose_100x_input.Text.Remove(SessionClose_100x_input.Text.Length - 1);
                if (SessionClose_100x_input.Text == String.Empty)
                {
                    SessionClose_100x_total.Text = "0.00";
                    SessionClose_grand_total_cal();
                    return;
                }
                SessionClose_100x_total.Text = (Convert.ToDouble(SessionClose_100x_input.Text) * 100).ToString("0.00");
                SessionClose_100x_input.Select(SessionClose_100x_input.Text.Length, 0);
                SessionClose_grand_total_cal();
            }
            if (SessionClose_input_GotFocus_field == "SessionClose_50x_input" && SessionClose_50x_input.Text != String.Empty)
            {
                SessionClose_50x_input.Text = SessionClose_50x_input.Text.Remove(SessionClose_50x_input.Text.Length - 1);
                if (SessionClose_50x_input.Text == String.Empty)
                {
                    SessionClose_50x_total.Text = "0.00";
                    SessionClose_grand_total_cal();
                    return;
                }
                SessionClose_50x_total.Text = (Convert.ToDouble(SessionClose_50x_input.Text) * 50).ToString("0.00");
                SessionClose_50x_input.Select(SessionClose_50x_input.Text.Length, 0);
                SessionClose_grand_total_cal();
            }
            if (SessionClose_input_GotFocus_field == "SessionClose_10x_input" && SessionClose_10x_input.Text != String.Empty)
            {
                SessionClose_10x_input.Text = SessionClose_10x_input.Text.Remove(SessionClose_10x_input.Text.Length - 1);
                if (SessionClose_10x_input.Text == String.Empty)
                {
                    SessionClose_10x_total.Text = "0.00";
                    SessionClose_grand_total_cal();
                    return;
                }
                SessionClose_10x_total.Text = (Convert.ToDouble(SessionClose_10x_input.Text) * 10).ToString("0.00");
                SessionClose_10x_input.Select(SessionClose_10x_input.Text.Length, 0);
                SessionClose_grand_total_cal();
            }
            if (SessionClose_input_GotFocus_field == "SessionClose_5x_input" && SessionClose_5x_input.Text != String.Empty)
            {
                SessionClose_5x_input.Text = SessionClose_5x_input.Text.Remove(SessionClose_5x_input.Text.Length - 1);
                if (SessionClose_5x_input.Text == String.Empty)
                {
                    SessionClose_5x_total.Text = "0.00";
                    SessionClose_grand_total_cal();
                    return;
                }
                SessionClose_5x_total.Text = (Convert.ToDouble(SessionClose_5x_input.Text) * 5).ToString("0.00");
                SessionClose_5x_input.Select(SessionClose_5x_input.Text.Length, 0);
                SessionClose_grand_total_cal();
            }
            if (SessionClose_input_GotFocus_field == "SessionClose_1x_input" && SessionClose_1x_input.Text != String.Empty)
            {
                SessionClose_1x_input.Text = SessionClose_1x_input.Text.Remove(SessionClose_1x_input.Text.Length - 1);
                if (SessionClose_1x_input.Text == String.Empty)
                {
                    SessionClose_1x_total.Text = "0.00";
                    SessionClose_grand_total_cal();
                    return;
                }
                SessionClose_1x_total.Text = (Convert.ToDouble(SessionClose_1x_input.Text) * 1).ToString("0.00");
                SessionClose_1x_input.Select(SessionClose_1x_input.Text.Length, 0);
                SessionClose_grand_total_cal();
            }
            if (SessionClose_input_GotFocus_field == "SessionClose_50dx_input" && SessionClose_50dx_input.Text != String.Empty)
            {
                SessionClose_50dx_input.Text = SessionClose_50dx_input.Text.Remove(SessionClose_50dx_input.Text.Length - 1);
                if (SessionClose_50dx_input.Text == String.Empty)
                {
                    SessionClose_50dx_total.Text = "0.00";
                    SessionClose_grand_total_cal();
                    return;
                }
                SessionClose_50dx_total.Text = (Convert.ToDouble(SessionClose_50dx_input.Text) * 50).ToString("0.00");
                SessionClose_50dx_input.Select(SessionClose_50dx_input.Text.Length, 0);
                SessionClose_grand_total_cal();
            }
            if (SessionClose_input_GotFocus_field == "SessionClose_25dx_input" && SessionClose_25dx_input.Text != String.Empty)
            {
                SessionClose_25dx_input.Text = SessionClose_25dx_input.Text.Remove(SessionClose_25dx_input.Text.Length - 1);
                if (SessionClose_25dx_input.Text == String.Empty)
                {
                    SessionClose_25dx_total.Text = "0.00";
                    SessionClose_grand_total_cal();
                    return;
                }
                SessionClose_25dx_total.Text = (Convert.ToDouble(SessionClose_25dx_input.Text) * 25).ToString("0.00");
                SessionClose_25dx_input.Select(SessionClose_25dx_input.Text.Length, 0);
                SessionClose_grand_total_cal();
            }
            if (SessionClose_input_GotFocus_field == "SessionClose_only_total_input" && SessionClose_only_total_input.Text != String.Empty)
            {
                SessionClose_only_total_input.Text = SessionClose_only_total_input.Text.Remove(SessionClose_only_total_input.Text.Length - 1);
                if (SessionClose_only_total_input.Text != String.Empty)
                {
                    SessionClose_only_total_total.Text = "0.00";
                    SessionClose_grand_total_cal();
                    return;
                }
                SessionClose_only_total_total.Text = Convert.ToDouble(SessionClose_only_total_input.Text).ToString("0.00");
                SessionClose_only_total_input.Select(SessionClose_only_total_input.Text.Length, 0);
                SessionClose_grand_total.Text = SessionClose_only_total_input.Text;
            }
        }

        private void SessionClose_KeyPad_clear_Click(object sender, RoutedEventArgs e)
        {
            switch (SessionClose_input_GotFocus_field)
            {
                case "SessionClose_card_input":
                    SessionClose_card_input.Text = String.Empty;
                    SessionClose_card_total.Text = "0.00";
                    Keyboard.Focus(SessionClose_card_input);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_500x_input":
                    SessionClose_500x_input.Text = String.Empty;
                    SessionClose_500x_total.Text = "0.00";
                    Keyboard.Focus(SessionClose_500x_input);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_100x_input":
                    SessionClose_100x_input.Text = String.Empty;
                    SessionClose_100x_total.Text = "0.00";
                    Keyboard.Focus(SessionClose_100x_input);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_50x_input":
                    SessionClose_50x_input.Text = String.Empty;
                    SessionClose_50x_total.Text = "0.00";
                    Keyboard.Focus(SessionClose_50x_input);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_10x_input":
                    SessionClose_10x_input.Text = String.Empty;
                    SessionClose_10x_total.Text = "0.00";
                    Keyboard.Focus(SessionClose_10x_input);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_5x_input":
                    SessionClose_5x_input.Text = String.Empty;
                    SessionClose_5x_total.Text = "0.00";
                    Keyboard.Focus(SessionClose_5x_input);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_1x_input":
                    SessionClose_1x_input.Text = String.Empty;
                    SessionClose_1x_total.Text = "0.00";
                    Keyboard.Focus(SessionClose_1x_input);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_50dx_input":
                    SessionClose_50dx_input.Text = String.Empty;
                    SessionClose_50dx_total.Text = "0.00";
                    Keyboard.Focus(SessionClose_50dx_input);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_25dx_input":
                    SessionClose_25dx_input.Text = String.Empty;
                    SessionClose_25dx_total.Text = "0.00";
                    Keyboard.Focus(SessionClose_25dx_input);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_only_total_input":
                    SessionClose_only_total_input.Text = String.Empty;
                    SessionClose_only_total_total.Text = "0.00";
                    Keyboard.Focus(SessionClose_only_total_input);
                    SessionClose_grand_total.Text = "0.00";
                    break;

                default:
                    break;
            }
        }

        private void SessionClose_KeyPadEnter_Click(object sender, RoutedEventArgs e)
        {
            switch (SessionClose_input_GotFocus_field)
            {
                case "SessionClose_card_input":
                    Keyboard.Focus(SessionClose_500x_input);
                    break;

                case "SessionClose_500x_input":
                    Keyboard.Focus(SessionClose_100x_input);
                    break;

                case "SessionClose_100x_input":
                    Keyboard.Focus(SessionClose_50x_input);
                    break;

                case "SessionClose_50x_input":
                    Keyboard.Focus(SessionClose_10x_input);
                    break;

                case "SessionClose_10x_input":
                    Keyboard.Focus(SessionClose_5x_input);
                    break;

                case "SessionClose_5x_input":
                    Keyboard.Focus(SessionClose_1x_input);
                    break;

                case "SessionClose_1x_input":
                    Keyboard.Focus(SessionClose_50dx_input);
                    break;

                case "SessionClose_50dx_input":
                    Keyboard.Focus(SessionClose_25dx_input);
                    break;

                case "SessionClose_25dx_input":
                    //Keyboard.Focus(SessionClose_25dx_input);
                    break;

                case "SessionClose_only_total_input":
                    //Keyboard.Focus(SessionClose_only_total_input);
                    break;

                default:
                    break;
            }
        }

        private void SessionClose_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            switch (SessionClose_input_GotFocus_field)
            {
                case "SessionClose_card_input":
                    Keyboard.Focus(SessionClose_card_input);
                    SessionClose_card_input.Select(SessionClose_card_input.Text.Length, 0);
                    break;

                case "SessionClose_500x_input":
                    Keyboard.Focus(SessionClose_500x_input);
                    SessionClose_500x_input.Select(SessionClose_500x_input.Text.Length, 0);
                    break;

                case "SessionClose_100x_input":
                    Keyboard.Focus(SessionClose_100x_input);
                    SessionClose_100x_input.Select(SessionClose_100x_input.Text.Length, 0);

                    break;

                case "SessionClose_50x_input":
                    Keyboard.Focus(SessionClose_50x_input);
                    SessionClose_50x_input.Select(SessionClose_50x_input.Text.Length, 0);

                    break;

                case "SessionClose_10x_input":
                    Keyboard.Focus(SessionClose_10x_input);
                    SessionClose_10x_input.Select(SessionClose_10x_input.Text.Length, 0);

                    break;

                case "SessionClose_5x_input":
                    Keyboard.Focus(SessionClose_5x_input);
                    SessionClose_5x_input.Select(SessionClose_5x_input.Text.Length, 0);

                    break;

                case "SessionClose_1x_input":
                    Keyboard.Focus(SessionClose_1x_input);
                    SessionClose_1x_input.Select(SessionClose_1x_input.Text.Length, 0);

                    break;

                case "SessionClose_50dx_input":
                    Keyboard.Focus(SessionClose_50dx_input);
                    SessionClose_50dx_input.Select(SessionClose_50dx_input.Text.Length, 0);

                    break;

                case "SessionClose_25dx_input":
                    Keyboard.Focus(SessionClose_25dx_input);
                    SessionClose_25dx_input.Select(SessionClose_25dx_input.Text.Length, 0);

                    break;

                case "SessionClose_only_total_input":
                    Keyboard.Focus(SessionClose_only_total_input);
                    SessionClose_only_total_input.Select(SessionClose_only_total_input.Text.Length, 0);

                    break;

                default:
                    break;
            }
        }

        private void SessionClose_input_TextChanged(object sender, TextChangedEventArgs e)
        {
            switch (SessionClose_input_GotFocus_field)
            {
                case "SessionClose_card_input":
                    SessionClose_card_input.Select(SessionClose_card_input.Text.Length, 0);
                    break;

                case "SessionClose_500x_input":
                    SessionClose_500x_input.Select(SessionClose_500x_input.Text.Length, 0);
                    break;

                case "SessionClose_100x_input":
                    SessionClose_100x_input.Select(SessionClose_100x_input.Text.Length, 0);
                    break;

                case "SessionClose_50x_input":
                    SessionClose_50x_input.Select(SessionClose_50x_input.Text.Length, 0);
                    break;

                case "SessionClose_10x_input":
                    SessionClose_10x_input.Select(SessionClose_10x_input.Text.Length, 0);
                    break;

                case "SessionClose_5x_input":
                    SessionClose_5x_input.Select(SessionClose_5x_input.Text.Length, 0);
                    break;

                case "SessionClose_1x_input":
                    SessionClose_1x_input.Select(SessionClose_1x_input.Text.Length, 0);
                    break;

                case "SessionClose_50dx_input":
                    SessionClose_50dx_input.Select(SessionClose_50dx_input.Text.Length, 0);
                    break;

                case "SessionClose_25dx_input":
                    SessionClose_25dx_input.Select(SessionClose_25dx_input.Text.Length, 0);
                    break;

                case "SessionClose_only_total_input":
                    SessionClose_only_total_input.Select(SessionClose_only_total_input.Text.Length, 0);
                    break;

                default:
                    break;
            }
        }

        private void SessionClose_grand_total_cal()
        {
            SessionClose_grand_total.Text = (
                Convert.ToDouble(SessionClose_card_total.Text == String.Empty ? "0" : SessionClose_card_total.Text) +
                Convert.ToDouble(SessionClose_500x_total.Text == String.Empty ? "0" : SessionClose_500x_total.Text) +
                Convert.ToDouble(SessionClose_100x_total.Text == String.Empty ? "0" : SessionClose_100x_total.Text) +
                Convert.ToDouble(SessionClose_50x_total.Text == String.Empty ? "0" : SessionClose_50x_total.Text) +
                Convert.ToDouble(SessionClose_10x_total.Text == String.Empty ? "0" : SessionClose_10x_total.Text) +
                Convert.ToDouble(SessionClose_5x_total.Text == String.Empty ? "0" : SessionClose_5x_total.Text) +
                Convert.ToDouble(SessionClose_1x_total.Text == String.Empty ? "0" : SessionClose_1x_total.Text) +
                Convert.ToDouble(SessionClose_50dx_total.Text == String.Empty ? "0" : SessionClose_50dx_total.Text) +
                Convert.ToDouble(SessionClose_25dx_total.Text == String.Empty ? "0" : SessionClose_25dx_total.Text)
                ).ToString("0.00");
        }
        #region Validations

        /// <summary>
        /// Allowes Number with Decimal Points
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Number_dot_ValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            //Regex regex = new Regex("[^0-9]+");
            //e.Handled = regex.IsMatch(e.Text);

            var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");//allow decimal points
            if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                e.Handled = false;
            else
                e.Handled = true;
        }

        /// <summary>
        /// Allowes Only Numbers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            //Regex regex = new Regex("[^0-9]+");
            //e.Handled = regex.IsMatch(e.Text);

            var regex = new Regex("^[0-9]*$");//only numbers
            if (regex.IsMatch(e.Text))
                e.Handled = false;
            else
                e.Handled = true;
        }

        #endregion Validations
        private void BackToCart_from_session_close_page_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as Grid).Children.Remove(this);
        }
    }
}
