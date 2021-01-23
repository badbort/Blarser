using System.Diagnostics;

namespace Blarser.WowContent.Tables
{
    /// <summary>
    /// A Maps.dbc record.
    /// </summary>
    [ DebuggerDisplay( "ID={ID}, MapName={MapName}, InstanceType={InstanceType}" ) ]
    public record Map
    {
        /// <summary>
        /// The map ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The map directory in \World\Maps
        /// </summary>
        public string MapDirectory { get; set; }

        /// <summary>
        /// The instance type.
        /// </summary>
        /// <remarks>
        /// Values:
        /// 0. World?
        /// 1. Dungeon
        /// 2. Raid
        /// 3. Battleground
        /// 4. Arena
        /// 5. Scenario
        /// </remarks>
        public InstanceType InstanceType { get; set; }

        /// <summary>
        /// The flags
        /// </summary>
        public int Flags { get; set; }

        /// <summary>
        /// The map type
        /// </summary>
        public int MapType { get; set; }

        public int Unknown1 { get; set; }

        /// <summary>
        /// The map name
        /// </summary>
        public string MapName { get; set; }

        /// <summary>
        /// The area table identifier
        /// </summary>
        public int AreaTableID { get; set; }

        /// <summary>
        /// The map description0 language
        /// </summary>
        public string MapDescription0Lang { get; set; }

        /// <summary>
        /// The map description1 language
        /// </summary>
        public string MapDescription1Lang { get; set; }

        /// <summary>
        /// The loading screen identifier
        /// </summary>
        public int LoadingScreenID { get; set; }

        /// <summary>
        /// The minimap icon scale
        /// </summary>
        public float MinimapIconScale { get; set; }

        /// <summary>
        /// The corpse map identifier
        /// </summary>
        public int CorpseMapID { get; set; }

        /// <summary>
        /// The corpse x
        /// </summary>
        public float CorpseX { get; set; }

        /// <summary>
        /// The corpse y
        /// </summary>
        public float CorpseY { get; set; }

        /// <summary>
        /// The time of day override
        /// </summary>
        public int TimeOfDayOverride { get; set; }

        /// <summary>
        /// The expansion index
        /// </summary>
        public int ExpansionIndex { get; set; }

        ///// <summary>
        ///// The raid offset
        ///// </summary>
        //public int RaidOffset{get;set;} // 6.0.1.18179?

        /// <summary>
        /// The maximum players
        /// </summary>
        public int MaxPlayers { get; set; }

        /// <summary>
        /// The number of players
        /// </summary>
        public int NumberOfPlayers { get; set; }

        /// <summary>
        /// The parent map identifier
        /// </summary>
        public int ParentMapID { get; set; }

        /// <summary>
        /// The cosmetic parent map identifier
        /// </summary>
        public int CosmeticParentMapID { get; set; } // 6.0.1.18179?

        /// <summary>
        /// The time offset
        /// </summary>
        public int TimeOffset { get; set; } // 6.0.1.18179?
    }
}