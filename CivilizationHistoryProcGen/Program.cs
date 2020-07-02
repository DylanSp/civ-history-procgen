using System;
using Terrain;

namespace CivilizationHistoryProcGen
{
    class Program
    {
        static void Main(string[] args)
        {
            DisplayGrid();

            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }

        private static void DisplayGrid()
        {
            var terrainGenerator = new TerrainGenerator();
            var gridLength = 20;
            var grid = terrainGenerator.GenerateGrid(gridLength);

            for (var row = 0; row < grid.GetLength(0); row++)
            {
                for (var col = 0; col < grid.GetLength(1); col++)
                {
                    switch (grid[row, col])
                    {
                        case Ocean _:
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.Write(" ");
                            Console.ResetColor();
                            break;
                        case Plains _:
                            Console.Write("_");
                            break;
                        case Hills _:
                            Console.Write("^");
                            break;
                        case Mountains _:
                            Console.Write("M");
                            break;
                    }
                }
                Console.WriteLine();
            }
             
            Console.WriteLine();
        }
    }
}
