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
using System.Windows.Shapes;

namespace RPM_14
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Window
    {

        public Autorization()
        {
            InitializeComponent();
            tb_Password.Focus();
        }

        private void btn_Enter_click(object sender, RoutedEventArgs e)
        {
            if (tb_Password.Password == "123")
            {               
                MainWindow window = new MainWindow();
                window.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Неверный пароль");
            }
        }

        private void btn_Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
