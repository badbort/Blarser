using System;
using System.Buffers;
using System.Linq;
using System.Text;
using Blarser.WowContent.WowFiles.Chunks;

namespace Blarser.WowContent.WowFiles
{
    public static class DB2Reader
    {
        public static DB2 Read( ReadOnlySequence<byte> buffer )
        {
            Span<byte> chunkLengthSpan = stackalloc byte[4];

            buffer.Slice( 4, 4 ).CopyTo( chunkLengthSpan );

            throw new NotImplementedException();
        }

        public static bool TryReadHeader( ref ReadOnlySequence<byte> buffer, out wdc3_db2_header header )
        {
            if(buffer.Length < 68)
            {
                header = default;
                return false;
            }

            header = ChunkReader.Readwdc3_db2_header(ref buffer);
            return true;
            
            // Span<byte> b = stackalloc byte[4];
            //
            // header = new wdc3_db2_header();
            // header.version = ChunkReader.ReadStringReverse( ref buffer, 4 );
            // header.record_count = ChunkReader.ReadUInt32(ref buffer, ref b);
            // header.field_count = ChunkReader.ReadUInt32(ref buffer, ref b);
            // header.record_size = ChunkReader.ReadUInt32(ref buffer, ref b);
            // header.string_table_size = ChunkReader.ReadUInt32(ref buffer, ref b);
            // header.table_hash = ChunkReader.ReadUInt32(ref buffer, ref b);
            // header.layout_hash = ChunkReader.ReadUInt32(ref buffer, ref b);
            // header.min_id = ChunkReader.ReadUInt32(ref buffer, ref b);
            // header.max_id = ChunkReader.ReadUInt32(ref buffer, ref b);
            // header.locale = ChunkReader.ReadUInt32(ref buffer, ref b);
            // header.flags = ChunkReader.ReadUInt16(ref buffer, ref b);
            // header.id_index = ChunkReader.ReadUInt16(ref buffer, ref b);
            // header.total_field_count = ChunkReader.ReadUInt32(ref buffer, ref b);
            // header.bitpacked_data_offset = ChunkReader.ReadUInt32(ref buffer, ref b);
            // header.lookup_column_count = ChunkReader.ReadUInt32(ref buffer, ref b);
            // header.field_storage_info_size = ChunkReader.ReadUInt32(ref buffer, ref b);
            // header.common_data_size = ChunkReader.ReadUInt32(ref buffer, ref b);
            // header.pallet_data_size = ChunkReader.ReadUInt32(ref buffer, ref b);
            // header.section_count = ChunkReader.ReadUInt32(ref buffer, ref b);

            throw new NotImplementedException();
            return true;
        }

        public static bool TryReadSectionHeaders( wdc3_db2_header wdc3Db2Header, ref ReadOnlySequence<byte> buffer, out wdc3_section_header[] sectionHeader )
        {
            if(buffer.Length < wdc3_section_header.Size * wdc3Db2Header.section_count)
            {
                sectionHeader = default;
                return false;
            }

            sectionHeader = new wdc3_section_header[wdc3Db2Header.section_count];

            for(int i = 0; i < wdc3Db2Header.section_count; i++)
                sectionHeader[i] = ChunkReader.Readwdc3_section_header( ref buffer );
            
            return true;
        }
        
    }

    public class DB2
    {
    }

    [GenerateRead(size: 68)]
    public class wdc3_db2_header
    {
        [ChunkField] public UInt32 version { get; set; } // 'WDC3'
        [ChunkField] public UInt32 record_count { get; set; } // this is for all sections combined now
        [ChunkField] public UInt32 field_count { get; set; }
        [ChunkField] public UInt32 record_size { get; set; }
        [ChunkField] public UInt32 string_table_size { get; set; } // this is for all sections combined now
        [ChunkField] public UInt32 table_hash { get; set; } // hash of the table name
        [ChunkField] public UInt32 layout_hash { get; set; } // this is a hash field that changes only when the structure of the data changes
        [ChunkField] public UInt32 min_id { get; set; }
        [ChunkField] public UInt32 max_id { get; set; }
        [ChunkField] public UInt32 locale { get; set; } // as seen in TextWowEnum
        [ChunkField] public UInt16 flags { get; set; } // possible values are listed in Known Flag Meanings
        [ChunkField] public UInt16 id_index { get; set; } // this is the index of the field containing ID values; this is ignored if flags & 0x04 != 0
        [ChunkField] public UInt32 total_field_count { get; set; } // from WDC1 onwards, this value seems to always be the same as the 'field_count' value
        [ChunkField] public UInt32 bitpacked_data_offset { get; set; } // relative position in record where bitpacked data begins; not important for parsing the file
        [ChunkField] public UInt32 lookup_column_count { get; set; }
        [ChunkField] public UInt32 field_storage_info_size { get; set; }
        [ChunkField] public UInt32 common_data_size { get; set; }
        [ChunkField] public UInt32 pallet_data_size { get; set; }
        [ChunkField] public UInt32 section_count { get; set; } // new to WDC2, this is number of sections of data
    }

    [GenerateRead(size: wdc3_section_header.Size, generateArray: true)]
    public class wdc3_section_header
    {
        public const int Size = 36;
        
        [ChunkField] public UInt64 tact_key_hash { get; set; } // TactKeyLookup hash
        [ChunkField] public UInt32 file_offset { get; set; } // absolute position to the beginning of the section
        [ChunkField] public UInt32 record_count { get; set; } // 'record_count' for the section
        [ChunkField] public UInt32 string_table_size { get; set; } // 'string_table_size' for the section
        [ChunkField] public UInt32 offset_records_end { get; set; } // Offset to the spot where the records end in a file with an offset map structure;
        [ChunkField] public UInt32 id_list_size { get; set; } // Size of the list of ids present in the section
        [ChunkField] public UInt32 relationship_data_size { get; set; } // Size of the relationship data in the section
        [ChunkField] public UInt32 offset_map_id_count { get; set; } // Count of ids present in the offset map in the section
        [ChunkField] public UInt32 copy_table_count { get; set; } // Count of the number of deduplication entries (you can multiply by 8 to mimic the old 'copy_table_size' field)
    };
    
    [GenerateRead(size: 32, generateArray: true)]
    public class field_structure
    {
        [ChunkField] public Int16 size { get; set; } // size in bits as calculated by: byteSize = (32 - size) / 8; this value can be negative to indicate field sizes larger than 32-bits
        [ChunkField] public UInt16 position { get; set; } // position of the field within the record, relative to the start of the record
    };
}