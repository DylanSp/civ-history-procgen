using System;
using System.Drawing;
using System.IO;
using Terrain;

namespace CivilizationHistoryProcGen
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var bitmap = CreateHeightBitmap())
            {
                var filename = "heightMap.bmp";
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
                bitmap.Save(filename);
            }

            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }

        private static Bitmap CreateHeightBitmap()
        {
            var terrainGenerator = new TerrainGenerator();
            var gridRadius = 500;
            var grid = terrainGenerator.GenerateGrid(gridRadius);

            var bitmap = new Bitmap(grid.GetLength(0), grid.GetLength(1));
            for (var row = 0; row < grid.GetLength(0); row++)
            {
                for (var col = 0; col < grid.GetLength(1); col++)
                {
                    if (grid[row, col].Item1)
                    {
                        bitmap.SetPixel(row, col, Color.Blue);
                    }
                    else
                    {
                        var grayscaledHeight = (int) Math.Floor(grid[row, col].Item2 * 255);
                        var color = Color.FromArgb(grayscaledHeight, grayscaledHeight, grayscaledHeight);
                        bitmap.SetPixel(row, col, color);
                    }
                }
            }
            return bitmap;
        }
    }
}
