using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace RPM_14
{
    /// <summary>
    /// Логика взаимодействия для Preferences.xaml
    /// </summary>
    public partial class Preferences : Window
    {
        public Preferences()
        {
            InitializeComponent();
        }

        private void btn_Enter_click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tb_rows.Text) && !string.IsNullOrEmpty(tb_columns.Text))
            {
                int rows = Convert.ToInt32(tb_rows.Text);
                int columns = Convert.ToInt32(tb_columns.Text);

                StreamWriter file = new StreamWriter("config.ini", false);

                file.WriteLine(rows);
                config.rows = rows;

                file.WriteLine(columns);
                config.columns = columns;

                file.Close();

                config.wasChanged = true;

                this.Close();
            }
            else
            {
                MessageBox.Show("Заполните поля");
            }
        }

        private void btn_Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
            config.wasChanged = false;
        }

        private void tb_PreviewKeyDown_NumberOnly(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key == Key.Back || e.Key == Key.Tab)
            {
                return;
            }
            e.Handled = true;
        }
    }
}
