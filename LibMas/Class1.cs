using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Data;

namespace LibMas
{
    public class Massiv
    {
        public static void Init(ref int[] mas, int column, int rndMax)
        {
            mas = new int[column];
            Random rnd = new Random(); 
            for (int i = 0; i<mas.Length; i++)
            {
                mas[i] = rnd.Next(-rndMax+1,rndMax);
            }
        }

        public static void InitEmpty(ref int[] mas, int column)
        {
            mas = new int[column];
            for (int i = 0; i < mas.Length; i++)
            {
                mas[i] = 0;
            }
        }

        public static void Fill(ref int[] mas, int rndMax)
        {
            Random rnd = new Random();
            for (int i = 0; i<mas.Length; i++)
            {
                mas[i] = rnd.Next(-rndMax+1, rndMax);
            }
        }
        
        public static void Clear(ref int[] mas)
        {
            for (int i = 0; i<mas.Length; i++)
            {
                mas[i] = 0;
            }
        }
        
        public static void Save(int[] mas, string path)
        {
            StreamWriter file = new StreamWriter(path);
            file.WriteLine("Mas");
            file.WriteLine(mas.Length);
            for (int i = 0; i < mas.Length; i++)
            {
                file.WriteLine(mas[i]);
            }
            file.Close();
        }

        public static void Open(ref int[] mas, string path)
        {
            StreamReader file = new StreamReader(path);
            file.ReadLine();
            int dlina = Convert.ToInt32(file.ReadLine());
            mas = new int[dlina];
            for (int i = 0; i < dlina; i++)
            {
                mas[i] = Convert.ToInt32(file.ReadLine());
            }
            file.Close();
        }
    }

    public class Matrica
    {
        public static void Init(ref int[,] mas, int row, int column, int rndMax)
        {
            mas = new int[row, column];
            Random rnd = new Random();
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    mas[i, j] = rnd.Next(-rndMax+1, rndMax);
                }
            }
        }

        public static void Fill(ref int[,] mas, int rndMax)
        {
            Random rnd = new Random();
            int row = mas.GetLength(0);
            int column = mas.GetLength(1);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    mas[i, j] = rnd.Next(-rndMax+1, rndMax);
                }
            }
        }

        public static void Clear(ref int[,] mas)
        {
            int row = mas.GetLength(0);
            int column = mas.GetLength(1);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    mas[i, j] = 0;
                }
            }
        }

        public static void Save(int[,] mas, string path)
        {
            StreamWriter file = new StreamWriter(path);
            int row = mas.GetLength(0);
            int column = mas.GetLength(1);
            file.WriteLine("Matr");
            file.WriteLine(row);
            file.WriteLine(column);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    file.WriteLine(mas[i, j]);
                }
            }
            file.Close();
        }

        public static void Open(ref int[,] mas, string path)
        {
            StreamReader file = new StreamReader(path);
            file.ReadLine();
            int row = Convert.ToInt32(file.ReadLine());
            int column = Convert.ToInt32(file.ReadLine());
            mas = new int[row, column];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    mas[i, j] = Convert.ToInt32(file.ReadLine());
                }
            }
            file.Close();
        }
    }

    public static class VisualArray
    {
        public static DataTable ToDataTable<T>(this T[] arr)
        {
            var res = new DataTable();
            for (int i = 0; i < arr.Length; i++)
            {
                res.Columns.Add((i + 1).ToString(), typeof(T));
            }
            var row = res.NewRow();
            for (int i = 0; i < arr.Length; i++)
            {
                row[i] = arr[i];
            }
            res.Rows.Add(row);
            return res;
        }

        public static DataTable ToDataTable<T>(this T[,] arr)
        {
            var res = new DataTable(); 
            for (int i = 0; i < arr.GetLength(1); i++)
            {
                res.Columns.Add((i + 1).ToString(), typeof(T));
            }
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                var row = res.NewRow();
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    row[j] = arr[i, j];
                }
                res.Rows.Add(row);
            }
            return res;
        }
    }
}
