using System;

namespace Terrain
{
    public class TerrainGenerator
    {
        private readonly HeightGenerator _heightGenerator;

        public TerrainGenerator()
        {
            var rng = new Random();
            _heightGenerator = new HeightGenerator(rng);
        }

        public TerrainType[,] GenerateGrid(int gridRadius)
        {
            var sideLength = gridRadius * 2 + 1;
            var grid = new TerrainType[sideLength, sideLength];

            var maxDistance = Math.Sqrt(2) * sideLength;

            for (var row = 0; row < sideLength; row++)
            {
                for (var col = 0; col < sideLength; col++)
                {
                    var distanceFromCenter =
                        Math.Sqrt(Math.Pow(Math.Abs(row - gridRadius), 2) + Math.Pow(Math.Abs(col - gridRadius), 2));
                    var distanceScaled = distanceFromCenter / maxDistance;

                    // arbitrary
                    var oceanDistanceThreshold = 0.8;

                    grid[row, col] = GenerateTerrainTile(distanceScaled, oceanDistanceThreshold);
                }
            }

            return grid;
        }

        /// <summary>
        /// Generates a random terrain tile
        /// </summary>
        /// <param name="distanceFromCenter">Number between 0 and 1, scaled distance from center</param>
        /// <param name="oceanDistanceThreshold">Number between 0 and 1, scaled threshold at which to start creating oceans</param>
        /// <returns></returns>
        private TerrainType GenerateTerrainTile(double distanceFromCenter, double oceanDistanceThreshold)
        {
            var height = _heightGenerator.GetRandomHeight(distanceFromCenter);

            var OCEAN_MAX_HEIGHT = 0.2; // arbitrary
            if (height <= OCEAN_MAX_HEIGHT && distanceFromCenter >= oceanDistanceThreshold)
            {
                return new Ocean
                {
                    Height = OCEAN_MAX_HEIGHT,
                };
            }

            // arbitrary
            if (height <= 0.5)
            {
                return new Plains
                {
                    Height = height,
                };
            }

            // arbitrary
            if (height <= 0.8)
            {
                return new Hills
                {
                    Height = height,
                };
            }

            return new Mountains
            {
                Height = height,
            };
        }
    }
}
