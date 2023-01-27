using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace ASCII_art
{
    class Program
    {
        private const double WIDTH_OFFSET = 2;

        private const int MAX_WIDTH = 474;


        private static Bitmap ResizeBitmap(Bitmap bitmap)
        {      
            int newHeigth = (int) (bitmap.Height / WIDTH_OFFSET * MAX_WIDTH / bitmap.Width);

            if (bitmap.Width > MAX_WIDTH || bitmap.Height > newHeigth)
            {
                bitmap = new Bitmap(bitmap, new Size(MAX_WIDTH, newHeigth));
            }
            return bitmap;
        }

        [STAThread]
        static void Main(string[] args)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Images | *.bmp; *.png; *.jpg; *.JPEG"
            };

            Console.WriteLine("Press ENTER to start");

            while (true)
            {
                Console.ReadLine();

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    continue;
                }
                Console.Clear();

                Bitmap bitmap = new Bitmap(openFileDialog.FileName);

                bitmap = ResizeBitmap(bitmap);

                bitmap.ToGrayScale();

                BitmapToASCIIConverter converter = new BitmapToASCIIConverter(bitmap);

                char[][] rows = converter.Convert();

                foreach (var row in rows)
                {
                    Console.WriteLine(row);
                }

                char[][] rowsNegative = converter.ConvertNegative();

                string[] rowsOfString = new string[rows.Length];

                for (int i = 0; i < rowsNegative.Length; i++)
                {
                    for (int j = 0; j < rowsNegative[i].Length; j++)
                    {
                        rowsOfString[i] = rowsOfString[i] + rowsNegative[i][j];
                    }                    
                }
                File.WriteAllLines("image.txt", rowsOfString);

                Console.SetCursorPosition(0, 0);
            }
        }
    }
}
