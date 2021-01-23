using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Blarser.WowContent.Tables
{
    [ DebuggerDisplay( "ID={ID}, MapID={MapID}" ) ]
    public record DungeonMap
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
        /// The floor index
        /// </summary>
        public int FloorIndex { get; set; }

        /// <summary>
        /// The right position.
        /// </summary>
        public float Right { get; set; }

        /// <summary>
        /// The bottom position.
        /// </summary>
        public float Bottom { get; set; }

        /// <summary>
        /// The left position.
        /// </summary>
        public float Left { get; set; }

        /// <summary>
        /// The top position.
        /// </summary>
        public float Top { get; set; }

        /// <summary>
        /// The parent world map identifier
        /// </summary>
        public int ParentWorldMapID { get; set; }

        /// <summary>
        /// The flags
        /// </summary>
        public int Flags { get; set; }

        /// <summary>
        /// Gets the x minimum.
        /// </summary>
        public float XMin => Bottom;

        /// <summary>
        /// Gets the x maximum.
        /// </summary>
        public float XMax => Top;

        /// <summary>
        /// Gets the y minimum.
        /// </summary>
        public float YMin => Right;

        /// <summary>
        /// Gets the y maximum.
        /// </summary>
        public float YMax => Left;
    }
}