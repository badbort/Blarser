using System;

namespace Blarser.WowContent.WowFiles.Chunks
{
    [ FileChunk ]
    public class MOHD
    {
        /*000h*/
        [ ChunkField ]
        public UInt32 nTextures { get; set; }

/*004h*/
        [ ChunkField ]
        public UInt32 nGroups { get; set; }

/*008h*/
        [ ChunkField ]
        public UInt32 nPortals { get; set; }

/*00Ch*/
        [ ChunkField ]
        public UInt32 nLights { get; set; } // Blizzard seems to add one to the MOLT entry count when there are MOLP chunks in the groups (and maybe for MOLS too?)ᵘ

/*010h*/
        [ ChunkField ]
        public UInt32 nDoodadNames { get; set; }

/*014h*/
        [ ChunkField ]
        public UInt32 nDoodadDefs { get; set; } // *

/*018h*/
        [ ChunkField ]
        public UInt32 nDoodadSets { get; set; }

/*01Ch*/
        [ ChunkField ]
        public CArgb ambColor { get; set; } // Color settings for base (ambient) color. See the flag at /*03Ch*/.   

        /// /*020h*/ foreign_keyⁱ<uint32_t, &WMOAreaTableRec::m_WMOID> wmoID;
        [ ChunkField ]
        public UInt32 foreign_key { get; set; }

        /// <summary>
        /// /*024h*/
        /// </summary>
        [ ChunkField ]
        public CAaBox bounding_box { get; set; } // in the alpha, this bounding box was computed upon loading

        /// <summary>
        /// <para>
        /// /*03Ch*/
        /// flag_do_not_attenuate_vertices_based_on_distance_to_portal
        /// </para>
        /// <para>
        /// /*03Ch*/
        /// flag_use_unified_render_path
        /// In 3.3.5a this flag switches between classic render path (MOHD color is baked into MOCV values, all three batch types have their own rendering logic)
        /// and unified (MOHD color is added to lighting at runtime, int. and ext. batches share the same rendering logic). See [[1]] for more details.
        /// </para>
        /// <para>
        /// /*03Ch*/
        /// flag_use_liquid_type_dbc_id
        /// use real liquid type ID from DBCs instead of local one. See MLIQ for further reference.
        /// </para>
        /// <para>
        /// /*03Ch*/
        /// flag_do_not_fix_vertex_color_alpha
        /// In 3.3.5.a (and probably before) it prevents CMapObjGroup::FixColorVertexAlpha function to be executed. Alternatively, for the wotlk version
        /// of it, the function can be called with MOCV.a being set to 64, whjch will produce the same effect for easier implementation. For wotlk+ rendering,
        /// it alters the behavior of the said function instead. See [[2]] for more details.
        /// </para>
        /// <para>
        /// /*03Ch*/ Legion (20740)
        /// flag_lod
        /// </para>
        /// <para>
        /// flag_default_max_lod { get; set; }
        /// /*03Ch*/
        /// ≥ Legion (21796)ᵘ. Usually maxLodLevel = -1 but with this flag, numLod. Entries at this level are defaulted
        /// </para>
        /// <para>
        /// /*03Ch*/ unused as of Legion (20994)
        /// </para>
        /// </summary>
        [ ChunkField ]
        public UInt16 Flags { get; set; }

        /// <summary>
        /// /*03Eh*/ ≥ Legion (21108) includes base lod (→ numLod = 3 means '.wmo', 'lod0.wmo' and 'lod1.wmo')
        /// </summary>
        [ ChunkField ]
        public UInt16 numLod { get; set; }
    }
}