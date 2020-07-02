using System;
using System.Collections.Generic;
using System.Linq;

namespace Terrain
{
    public class TerrainGenerator
    {
        private readonly HeightGenerator _heightGenerator;
        private readonly Random _rng;

        public TerrainGenerator()
        {
            _rng = new Random();
            _heightGenerator = new HeightGenerator(_rng);
        }

        public TerrainType[,] GenerateGrid(int gridRadius)
        {
            var sideLength = gridRadius * 2 + 1;
            var grid = new TerrainType[sideLength, sideLength];

            var maxDistance = Math.Sqrt(2) * gridRadius;

            // generate initial map
            for (var row = 0; row < sideLength; row++)
            {
                for (var col = 0; col < sideLength; col++)
                {
                    var distanceFromCenter =
                        Math.Sqrt(Math.Pow(Math.Abs(row - gridRadius), 2) + Math.Pow(Math.Abs(col - gridRadius), 2));
                    var distanceScaled = distanceFromCenter / maxDistance;

                    // arbitrary
                    var oceanDistanceThreshold = 0.6;

                    grid[row, col] = GenerateTerrainTile(distanceScaled, oceanDistanceThreshold);
                }
            }

            // clean up unconnected rivers
            for (var row = 0; row < sideLength; row++)
            {
                for (var col = 0; col < sideLength; col++)
                {
                    if (grid[row, col] is River river && !TileLeadsToOcean(grid, row, col))
                    {
                        grid[row, col] = river.UnderlyingTerrain;
                    }
                }
            }

            // spawn rivers
            for (var row = 0; row < sideLength; row++)
            {
                for (var col = 0; col < sideLength; col++)
                {
                    var tile = grid[row, col];
                    if ((tile is Mountains || tile is Hills))
                    {
                        SpawnAdjacentRiver(grid, row, col);
                    }
                }
            }

            return grid;
        }

        private void SpawnAdjacentRiver(TerrainType[,] grid, int row, int col)
        {
            if (!GetAdjacentTiles(grid, row, col).Any(adjacent => adjacent.tile.Height < grid[row, col].Height && TileLeadsToOcean(grid, adjacent.adjacentRow, adjacent.adjacentCol)))
            {
                return;
            }

            int randomAdjacentRow, randomAdjacentCol;
            do
            {
                randomAdjacentRow = Math.Clamp(0, row + _rng.Next(-1, 2), grid.GetLength(0) - 1);
                randomAdjacentCol = Math.Clamp(0, col + _rng.Next(-1, 2), grid.GetLength(1) - 1);
            } while ((randomAdjacentRow == row && randomAdjacentCol == col) || grid[randomAdjacentRow, randomAdjacentCol].Height >= grid[row, col].Height || !TileLeadsToOcean(grid, randomAdjacentRow, randomAdjacentCol));

            if (!(grid[randomAdjacentRow, randomAdjacentCol] is Ocean))
            {
                var previous = grid[randomAdjacentRow, randomAdjacentCol];
                grid[randomAdjacentRow, randomAdjacentCol] = new River
                {
                    Height = previous.Height,
                    UnderlyingTerrain = previous,
                };
                SpawnAdjacentRiver(grid, randomAdjacentRow, randomAdjacentCol);
            }
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
            if (height <= OCEAN_MAX_HEIGHT)
            {
                if (distanceFromCenter >= oceanDistanceThreshold)
                {
                    return new Ocean
                    {
                        Height = OCEAN_MAX_HEIGHT,
                    };
                }
                else
                {
                    return new River
                    {
                        Height = height,
                        UnderlyingTerrain = new Plains
                        {
                            Height = height,
                        },
                    };
                }
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
            if (height <= 0.7)
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

        private bool TileLeadsToOcean(TerrainType[,] grid, int row, int col)
        {
            var tile = grid[row, col];
            if (tile is Ocean)
            {
                return true;
            }

            var adjacentTiles = GetAdjacentTiles(grid, row, col);

            return adjacentTiles.Any(adjacent => adjacent.tile.Height < tile.Height && TileLeadsToOcean(grid, adjacent.adjacentRow, adjacent.adjacentCol));
        }

        private List<(TerrainType tile, int adjacentRow, int adjacentCol)> GetAdjacentTiles(TerrainType[,] grid, int row, int col)
        {
            var adjacentTiles = new List<(TerrainType tile, int adjacentRow, int adjacentCol)>();
            var offsets = from rowOffset in Enumerable.Range(-1, 3)
                          from colOffset in Enumerable.Range(-1, 3)
                          select (rowOffset, colOffset);
            foreach (var (rowOffset, colOffset) in offsets)
            {
                var adjacentRow = row + rowOffset;
                var adjacentCol = col + colOffset;
                if (adjacentRow < 0 || adjacentRow >= grid.GetLength(0) || adjacentCol < 0 || adjacentCol >= grid.GetLength(0))
                {
                    continue;
                }
                adjacentTiles.Add((grid[adjacentRow, adjacentCol], adjacentRow, adjacentCol));
            }

            return adjacentTiles;
        }
    }
}
