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
            var gridLength = 10;
            var grid = terrainGenerator.GenerateGrid(gridLength);

            for (var row = 0; row < grid.GetLength(0); row++)
            {
                for (var col = 0; col < grid.GetLength(1); col++)
                {
                    switch (grid[row, col])
                    {
                        case Ocean _: 
                            Console.Write("o");
                            break;
                        case Plains _:
                            Console.Write("p");
                            break;
                        case Hills _:
                            Console.Write("H");
                            break;
                        case Mountains _:
                            Console.Write("^");
                            break;
                    }
                }
                Console.WriteLine();
            }
             
            Console.WriteLine();
        }
    }
}
