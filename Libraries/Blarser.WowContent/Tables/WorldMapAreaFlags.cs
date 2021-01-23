using System;

namespace Blarser.WowContent.Tables
{
    /// <summary>
    /// 
    /// </summary>
    [ Flags ]
    public enum WorldMapAreaFlags
    {
        /// <summary>
        /// The 1st bit unknown flag.
        /// </summary>
        Unknown1 = 0x1,

        /// <summary>
        /// The 2nd bit unknown flag.
        /// </summary>
        Unknown2 = 0x2,

        /// <summary>
        /// The <see cref="WorldMapArea" /> has a root world map.
        /// </summary>
        /// <remarks>
        /// This occurs for zones which have a <see cref="WorldMapArea" /> that displays the entire area. One odd example
        /// is Siege of Orgrimmar which has 15 world maps, 14 of which are in DungeonMap.dbc, but one also exists in the
        /// WorldMapArea record. i.e. OrgrimmarRaid1.blp - OrgrimmarRaid12.blp.
        /// </remarks>
        HasRootWorldMap = 0x4,

        /// <summary>
        /// The 4th bit unknown flag.
        /// </summary>
        Unknown4 = 0x8,

        /// <summary>
        /// The 5th bit unknown flag.
        /// </summary>
        Unknown5 = 0x10,

        /// <summary>
        /// The 8th bit flag.
        /// </summary>
        Unknown8 = 0x100,
    }
}