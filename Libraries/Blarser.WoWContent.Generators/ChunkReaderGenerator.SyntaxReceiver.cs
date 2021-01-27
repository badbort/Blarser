using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Blarser.WoWContent.Generators
{
    public partial class ChunkReaderGenerator
    {
        private sealed class SyntaxReceiver : ISyntaxReceiver
        {
            public List<ClassDeclarationSyntax> ClassDeclarations { get; } = new();
            
            public void OnVisitSyntaxNode( SyntaxNode syntaxNode )
            {
                // Any class
                if(syntaxNode is ClassDeclarationSyntax c && c.AttributeLists.Count > 0)
                    ClassDeclarations.Add( c );
            }
        }
    }
}