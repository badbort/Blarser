using System;
using System.Buffers;
using Blarser.WowContent.WowFiles.Chunks;

namespace Blarser.WowContent.WowFiles
{
    public class Wdt
    {
        public MODF MODF { get; set; }

        public static Wdt Create(ref ReadOnlySequence<byte> buffer)
        {
            throw new NotImplementedException();
        }
    }
}