using System.Numerics;

namespace Blarser.WowContent
{
    public static class CoordinateSystem
    {
        public static Vector3 TerrainNorth { get; } = new Vector3( 1, 0, 0 );

        public static Vector3 TerrainEast { get; } = new Vector3( 0, -1, 0 );

        public static Vector3 TerrainUp { get; } = new Vector3( 0, 0, 1 );
    }
}