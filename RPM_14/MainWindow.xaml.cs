using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LibMas;
using Microsoft.Win32;

namespace RPM_14
{
    static class config
    {
        public static int rows = 3;
        public static int columns = 3;
        public static bool wasChanged;
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int[,] matr;

        private void Output()
        {
            dg.ItemsSource = null;
            dg.ItemsSource = VisualArray.ToDataTable(matr).DefaultView;
            tb_size.Text = $"Размер: {matr.GetLength(0)}x{matr.GetLength(1)}";
            tb_ColumnRes.Clear();
            tb_ValueRes.Clear();
            tb_rows.Clear();
            tb_columns.Clear();
        }
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                StreamReader file = new StreamReader("config.ini");
                config.rows = Convert.ToInt32(file.ReadLine());
                config.columns = Convert.ToInt32(file.ReadLine());
                file.Close();
            }
            catch
            {
                MessageBox.Show("Будут применены стандартные настройки");
            }
            

            Matrica matrica = new Matrica();
            Matrica.Init(ref matr, config.rows, config.columns, 10);
            Output();
        }

        private void btn_SetSize_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tb_rows.Text) && !string.IsNullOrEmpty(tb_columns.Text))
            {
                int rows = Convert.ToInt32(tb_rows.Text);
                int columns = Convert.ToInt32(tb_columns.Text);
                Matrica.Init(ref matr, rows, columns, 10);
                Output();
            }
            else
            {
                MessageBox.Show("Заполните поля с размерами");
            }
        }

        private void tb_PreviewKeyDown_NumberOnly(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key == Key.Back || e.Key == Key.Tab)
            {
                return;
            }
            e.Handled = true;
        }

        private void btn_Calculate_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<int, int> mult = new Dictionary<int, int>();

            for (int i = 0; i < matr.GetLength(1); i++)
            {
                mult.Add(i + 1, 1);
                for (int j = 0; j < matr.GetLength(0); j++)
                {
                    mult[i + 1] *= matr[j, i];
                }
            }

            int min = int.MaxValue;
            int column = 0;

            foreach (var item in mult)
            {
                if (item.Value < min)
                {
                    min = item.Value;
                    column = item.Key;
                }
            }

            tb_ValueRes.Text = min.ToString();
            tb_ColumnRes.Text = column.ToString();

            tb_rows.Focus();
        }

        private void dg_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key == Key.Back || e.Key == Key.Tab || e.Key == Key.OemMinus)
            {
                return;
            }
            e.Handled = true;
        }

        private void dg_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;

            var currentCell = dg.CurrentCell;
            var cellContent = currentCell.Column.GetCellContent(currentCell.Item);

            string currentText = "";
            int selectionStart = 0;

            if (cellContent is TextBox textBox)
            {
                currentText = textBox.Text ?? "";
                selectionStart = textBox.SelectionStart;
            }

            if (e.Text == "-")
            {
                if (selectionStart != 0 || currentText.Contains("-"))
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        private void dg_CurrentCellChanged(object sender, EventArgs e)
        {
            DataGrid dg = (DataGrid)sender;

            if (dg.CurrentCell == null || dg.CurrentCell.Column == null)
            {
                tb_cell.Text = "Ячейка не выбрана";
                return;
            }

            int column = dg.CurrentCell.Column.DisplayIndex;

            int row = -1;
            if (dg.CurrentCell.Item != null)
            {
                row = dg.Items.IndexOf(dg.CurrentCell.Item);
            }

            tb_cell.Text = $"Ячейка: ({row + 1};{column + 1})";
        }

        private void dg_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            int column = e.Column.DisplayIndex;
            int row = dg.Items.IndexOf(dg.CurrentCell.Item);

            matr[row, column] = Convert.ToInt32(((TextBox)e.EditingElement).Text);

            tb_ColumnRes.Clear();
            tb_ValueRes.Clear();
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = ".txt";
            save.Filter = "Все файлы | *.* | Текстовые файлы | *.txt";
            save.FilterIndex = 2;
            save.Title = "Сохранение массива";
            if (save.ShowDialog() == true)
            {
                Matrica.Save(matr, save.FileName);
            }
        }

        private void btn_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.DefaultExt = ".txt";
            open.Filter = "Все файлы | *.* | Текстовые файлы | *.txt";
            open.FilterIndex = 2;
            open.Title = "Сохранение массива";
            if (open.ShowDialog() == true)
            {
                StreamReader check = new StreamReader(open.FileName);
                string type = check.ReadLine();
                check.Close();
                if (type == "Matr")
                {
                    try
                    {
                        Matrica.Open(ref matr, open.FileName);
                        Output();
                    }
                    catch
                    {
                        MessageBox.Show("Возникла непредвиденная ошибка обработки. Вероятно, файл не содержит массива / массив битый");
                    }
                }
                else
                {
                    MessageBox.Show("Вы пытаетесь открыть файл, не содержащий матрицу");
                }
            }
        }

        private void btn_Fill_Click(object sender, RoutedEventArgs e)
        {
            Matrica.Fill(ref matr, 10);
            Output();
        }

        private void btn_SetNull_Click(object sender, RoutedEventArgs e)
        {
            Matrica.Clear(ref matr);
            Output();
        }

        private void tb_rows_TextChanged(object sender, TextChangedEventArgs e)
        {
            tb_ColumnRes.Clear();
            tb_ValueRes.Clear();
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы хотите выйти?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }         
        }

        private void btn_ProgramInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Дана матрица размера M * N. \nНайти номер ее столбца с наименьшим произведением элементов и вывести данный номер, а также значение наименьшего произведения. ");
        }

        private void btn_DevInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Студент группы ИСП-31 Рункин Семён");
        }

        private void btn_NoIdeas_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Идеи закончились ещё на этапе тулбара, я не знаю, что поместить в контекстное меню, простите");
        }

        private void btn_Preferences_Click(object sender, RoutedEventArgs e)
        {
            Preferences pref = new Preferences();
            pref.Owner = this;
            pref.ShowDialog();
            if (config.wasChanged)
            {
                MessageBox.Show("Настройки были применены!");
                Matrica.Init(ref matr, config.rows, config.columns, 10);
                Output();
                config.wasChanged = false;
            }          
        }
    }
}