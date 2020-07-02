using System;
using System.Collections.Generic;
using System.Text;

namespace Terrain
{
    public abstract class TerrainType
    {
        public double Height { get; set; }
    }

    public class Ocean : TerrainType
    {
    }

    public class Plains : TerrainType
    {
    }

    public class Hills : TerrainType
    {
    }

    public class Mountains : TerrainType
    {
    }

    public class River : TerrainType
    {
        public TerrainType UnderlyingTerrain { get; set; }
    }
}
