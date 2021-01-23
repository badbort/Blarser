using System;
using System.IO;
using System.Linq;

namespace Blarser.WowContent.WowFiles
{
    /// <summary>
    /// https://wowdev.wiki/ADT/v18
    /// </summary>
    public class Adt
    {
        /// <summary>
        /// ADT files and blocks
        /// There is an .adt file for each existing block. If a block is unused it won't have an .adt file. The file will be: World/Maps/[InternalMapName]/[InternalMapName]_[BlockX]_[BlockY].adt.
        /// 
        /// InternalMapName - MapRec::m_Directory
        ///     BlockX - Index of the tile on the X axis
        ///     BlockY - Index of the tile on the Y axis
        /// Converting ADT co-ords to block X/Y can be done with the following formula (where axis is x or y): floor((32 - (axis / 533.33333)))
        /// </summary>
        /// <param name="adtPath"></param>
        /// <param name="map"></param>
        /// <param name="blockX"></param>
        /// <param name="blockY"></param>
        /// <param name="suffix"></param>
        public static void GetADTBlock( string adtPath, out string map, out int blockX, out int blockY, out string suffix )
        {
            map = Path.GetFileName( Path.GetDirectoryName( adtPath ) );

            if(map == null)
                throw new InvalidOperationException( "Invalid adt file path. Example: world/maps/unknown_901_33978_002/unknown_901_33978_002_25_20_3508608.adt" );

            var name = Path.GetFileNameWithoutExtension( adtPath );

            var blocksAndSuffix = name.Substring( map.Length + 1);
            var parts = blocksAndSuffix.Split( '_' );

            blockX = int.Parse( parts[0] );
            blockY = int.Parse( parts[1] );

            if(parts.Length > 2)
                suffix = string.Join( "_", parts.Skip( 2 ) );
            else
                suffix = default;
        }
    }
}