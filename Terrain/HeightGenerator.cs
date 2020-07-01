using System;
using System.Collections.Generic;
using System.Text;

namespace Terrain
{
    public class HeightGenerator
    {
        private readonly Random _rng;

        public HeightGenerator(Random rng)
        {
            _rng = rng;
        }

        /// <summary>
        /// Generates the height of a random point
        /// </summary>
        /// <param name="distanceFromCenter">A number between 0 and 1, representing the fraction of the total possible distance between this point and the map center</param>
        /// <returns></returns>
        public double GetRandomHeight(double distanceFromCenter)
        {
            if (distanceFromCenter < 0 || distanceFromCenter >= 1)
            {
                throw new ArgumentOutOfRangeException(nameof(distanceFromCenter));
            }

            var rawHeight = _rng.NextDouble();
            var scaledByDistanceHeight = rawHeight * (1 - distanceFromCenter);
            return scaledByDistanceHeight;
        }
    }
}
