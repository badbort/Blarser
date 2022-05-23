using System;

namespace Blarser.WowContent.WowFiles.Chunks
{
    [FileChunk]
    public class MOMT
    {
        [ ChunkField ]
        public SMOMaterial[] Materials { get; set; }
    }

    [ GenerateRead( 64 ) ]
    public class SMOMaterial
    {
        /// <summary>
        /// /*0x00*/ uint32_t F_UNLIT    : 1;                 // disable lighting logic in shader (but can still use vertex colors)
        /// /*0x00*/ uint32_t F_UNFOGGED : 1;                 // disable fog shading (rarely used)
        /// /*0x00*/ uint32_t F_UNCULLED : 1;                 // two-sided
        /// /*0x00*/ uint32_t F_EXTLIGHT : 1;                 // darkened, the intern face of windows are flagged 0x08
        /// /*0x00*/ uint32_t F_SIDN     : 1;                 // (bright at night, unshaded) (used on windows and lamps in Stormwind, for example) (see emissive color)
        /// /*0x00*/ uint32_t F_WINDOW   : 1;                 // lighting related (flag checked in CMapObj::UpdateSceneMaterials)
        /// /*0x00*/ uint32_t F_CLAMP_S  : 1;                 // tex clamp S (force this material's textures to use clamp s addressing)
        /// /*0x00*/ uint32_t F_CLAMP_T  : 1;                 // tex clamp T (force this material's textures to use clamp t addressing)
        /// /*0x00*/ uint32_t flag_0x100 : 1;
        /// /*0x00*/ uint32_t            : 23;                // unused as of 7.0.1.20994
        /// </summary>
        [ ChunkField ]
        public UInt32 Flags { get; set; }

        /// <summary>
        /// Index into CMapObj::s_wmoShaderMetaData
        /// </summary>
        [ ChunkField ]
        public UInt32 Shader { get; set; }

        [ ChunkField ]
        public UInt32 blendMode { get; set; }

        [ ChunkField ]
        public UInt32 texture_1 { get; set; }

        [ ChunkField ]
        public CImVector sidnColor { get; set; }

        [ ChunkField ]
        public CImVector frameSidnColor { get; set; }

        [ ChunkField ]
        public UInt32 texture_2 { get; set; }

        [ ChunkField ]
        public CArgb diffColor { get; set; }

        [ ChunkField ]
        public UInt32 ground_type_key { get; set; }

        [ ChunkField ]
        public UInt32 texture_3 { get; set; }

        [ ChunkField ]
        public UInt32 color_2 { get; set; }

        [ ChunkField ]
        public UInt32 flags_2 { get; set; }

        [ ChunkField ]
        public UInt32 runTimeData1 { get; set; }

        [ ChunkField ]
        public UInt32 runTimeData2 { get; set; }

        [ ChunkField ]
        public UInt32 runTimeData3 { get; set; }

        [ ChunkField ]
        public UInt32 runTimeData4 { get; set; }
    }
}