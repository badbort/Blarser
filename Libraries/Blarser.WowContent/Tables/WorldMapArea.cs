using System.Diagnostics;

namespace Blarser.WowContent.Tables
{
    /// <summary>
    /// A WorldMap for a non-dungeon map.
    /// </summary>
    [ DebuggerDisplay( "ID={ID}, AreaName={AreaName}" ) ]
    public record WorldMapArea
    {
        /// <summary>
        /// The identifier
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The map identifier
        /// </summary>
        public int MapID { get; set; }

        /// <summary>
        /// The area identifier
        /// </summary>
        public int AreaID { get; set; }

        /// <summary>
        /// The area name
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// The loc left
        /// </summary>
        public float LocLeft { get; set; }

        /// <summary>
        /// The loc right
        /// </summary>
        public float LocRight { get; set; }

        /// <summary>
        /// The loc top
        /// </summary>
        public float LocTop { get; set; }

        /// <summary>
        /// The loc bottom
        /// </summary>
        public float LocBottom { get; set; }

        /// <summary>
        /// The display map identifier
        /// </summary>
        public int DisplayMapID { get; set; }

        /// <summary>
        /// The default dungeon floor
        /// </summary>
        public int DefaultDungeonFloor { get; set; }

        /// <summary>
        /// The parent world map identifier
        /// </summary>
        public int ParentWorldMapID { get; set; }

        /// <summary>
        /// The flags
        /// </summary>
        public WorldMapAreaFlags Flags { get; set; }

        /// <summary>
        /// The level range minimum
        /// </summary>
        public int LevelRangeMin { get; set; }

        /// <summary>
        /// The level range maximum
        /// </summary>
        public int LevelRangeMax { get; set; }

        /// <summary>
        /// Gets the bottom in world coordinates.
        /// </summary>
        public float XMin => LocBottom;

        /// <summary>
        /// Gets the top in world coordinates.
        /// </summary>
        public float XMax => LocTop;

        /// <summary>
        /// Gets the y minimum.
        /// </summary>
        public float YMin => LocRight;

        /// <summary>
        /// Gets the y maximum.
        /// </summary>
        public float YMax => LocLeft;
    }
}