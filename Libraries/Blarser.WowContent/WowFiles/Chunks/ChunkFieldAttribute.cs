using System;
using System.Runtime.CompilerServices;

namespace Blarser.WowContent.WowFiles.Chunks
{
    [ AttributeUsage( AttributeTargets.Property ) ]
    public class ChunkFieldAttribute : Attribute
    {
        public int Order { get; }

        public ChunkFieldAttribute( [ CallerLineNumber ] int order = 0 ) => Order = order;
    }
}