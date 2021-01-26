using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Blarser.WoWContent.Generators
{
    [ Generator ]
    public class ChunkReaderGenerator : ISourceGenerator
    {
        /// <inheritdoc />
        public void Initialize( GeneratorInitializationContext context )
        {
            context.RegisterForSyntaxNotifications( () => new SyntaxReceiver() );
        }

        /// <inheritdoc />
        public void Execute( GeneratorExecutionContext context )
        {
            var syntaxReceiver = (SyntaxReceiver) context.SyntaxReceiver;

            Debugger.Launch();

            var chunkTypeAttribute = context.Compilation.GetTypeByMetadataName( "Blarser.WowContent.WowFiles.Chunks.FileChunkAttribute" );
            var chunkFieldAttribute = context.Compilation.GetTypeByMetadataName( "Blarser.WowContent.WowFiles.Chunks.ChunkFieldAttribute" );
            var chunkEntryAttribute = context.Compilation.GetTypeByMetadataName( "Blarser.WowContent.WowFiles.Chunks.ChunkEntryAttribute" );
            var array = context.Compilation.GetTypeByMetadataName( "System.Array" );
            
            // context.AddSource();


            // context.Compilation.Assembly.Ty

            context.AddSource( "myGeneratedFile.cs", SourceText.From( @"


namespace GeneratedNamespace
{
    public class GeneratedClass
    {
        public static void GeneratedMethod()
        {
            System.Console.WriteLine(""lajsdklfjalk :("");
            // generated code
        }
    }
}", Encoding.UTF8 ) );

            List<ChunkTypeInfo> chunkClasses = new List<ChunkTypeInfo>();

            foreach(var classDefinition in syntaxReceiver!.ClassDeclarations)
            {
                SemanticModel classModel = null;
                INamedTypeSymbol? classSymbol = null;
                var chunkClass = new ChunkTypeInfo();

                foreach(var attributeSyntax in classDefinition.AttributeLists.SelectMany(a => a.Attributes))
                {
                    var semanticModel = context.Compilation.GetSemanticModel( attributeSyntax.SyntaxTree );
                    var attributeSymbol = ModelExtensions.GetSymbolInfo( semanticModel, attributeSyntax, context.CancellationToken );

                    if(attributeSymbol.Symbol is not IMethodSymbol methodSymbol)
                        continue;

                    if(chunkTypeAttribute!.Equals( methodSymbol.ContainingType, SymbolEqualityComparer.Default ))
                    {
                        chunkClass.IsChunkType = true;
                        break;
                    }
                    
                    if(chunkEntryAttribute!.Equals( methodSymbol.ContainingType, SymbolEqualityComparer.Default ))
                    {
                        classSymbol ??= GetClassModel().GetDeclaredSymbol( classDefinition );
                        
                        var recordSize = (int?) classSymbol!.GetAttributes().SingleOrDefault(d => chunkEntryAttribute!.Equals(d.AttributeClass, SymbolEqualityComparer.Default))?.ConstructorArguments.FirstOrDefault().Value;
                        chunkClass.IsChunkType = true;
                        chunkClass.Length = recordSize;
                        break;
                    }
                }

                if(!chunkClass.IsChunkType)
                    continue;

                classSymbol ??= GetClassModel().GetDeclaredSymbol( classDefinition );
                chunkClass.ClassName = classSymbol.Name;

                // Found a class that needs the parse method extension added.
                // Now find the properties needed for conversion

                foreach(var method in classDefinition.Members.Where( m => m.IsKind( SyntaxKind.PropertyDeclaration ) ).OfType<PropertyDeclarationSyntax>())
                {
                    SemanticModel model = null;
                    
                    foreach(var attributeList in method.AttributeLists)
                    {
                        foreach(AttributeSyntax attributeSyntax in attributeList.Attributes)
                        {
                            var semanticModel = context.Compilation.GetSemanticModel( attributeSyntax.SyntaxTree );
                            var attributeSymbol = ModelExtensions.GetSymbolInfo( semanticModel, attributeSyntax, context.CancellationToken );

                            if(attributeSymbol.Symbol is not IMethodSymbol methodSymbol)
                                continue;

                            if(chunkFieldAttribute!.Equals( methodSymbol.ContainingType, SymbolEqualityComparer.Default ))
                            {
                                model ??= context.Compilation.GetSemanticModel(method.SyntaxTree);

                                var propertySymbol = model.GetDeclaredSymbol(method);
                                var propertyOrder = (int?) propertySymbol!.GetAttributes().SingleOrDefault(d => chunkFieldAttribute!.Equals(d.AttributeClass, SymbolEqualityComparer.Default))?.ConstructorArguments.FirstOrDefault().Value;
                                bool isArray = propertySymbol.Type.BaseType.Equals( array, SymbolEqualityComparer.Default );

                                var tName = propertySymbol.Type.Name;

                                string type = null;

                                if(isArray)
                                    type = ((IArrayTypeSymbol) propertySymbol.Type)?.ElementType.Name;
                                else
                                    type = ((INamedTypeSymbol) propertySymbol.Type).Name;
                                
                                if(propertyOrder.HasValue)
                                    chunkClass.Properties.Add( new PropertyInfo( type, method.Identifier.Text, isArray, propertyOrder.Value ) );
                            }
                        }
                    }
                }

                SemanticModel GetClassModel() => classModel ??= context.Compilation.GetSemanticModel(classDefinition.SyntaxTree);
                
                chunkClasses.Add( chunkClass );
            }

            foreach(ChunkTypeInfo chunkClass in chunkClasses)
            {
                var source = chunkClass.GetSource();
                context.AddSource( $"Read{chunkClass.ClassName}.cs", SourceText.From( source, Encoding.UTF8 ) );
            }
        }

        private class ChunkTypeInfo
        {
            public string ClassName { get; set; }

            public bool IsChunkType { get; set; }
            
            public int? Length { get; set; }

            public List<PropertyInfo> Properties { get; } = new();
            
            public string GetSource()
            {
                const string extensionClass = "ChunkSequenceReader";
                const string extensionNamespace = "Blarser.WowContent.WowFiles";
            
                return $@"
using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using Blarser.WowContent.WowFiles.Chunks;

namespace {extensionNamespace}
{{
    public static partial class {extensionClass}
    {{
        public static {ClassName} Read{ClassName}( ref ReadOnlySequence<byte> buffer )
        {{
            var instance = new {ClassName}();
            Span<byte> tempData = stackalloc byte[4];

{GetReadCalls()}
            return instance;
        }}

        public static int Get{ClassName}Length() => {Length ?? -1};
    }}
}}";
            }

            private string GetReadCalls()
            {
                StringBuilder sb = new StringBuilder();

                foreach(var p in Properties.OrderBy( t => t.Order ))
                {
                    string prop = p.PropertyName;

                    if(p.IsArray)
                    {
                        
                        sb.Append( $@"int entryCount = (int) buffer.Length / Get{p.PropertyType}Length();
instance.{prop} = new {p.PropertyType}[entryCount];

for(int i = 0; i < entryCount; i++)
{{
    instance.{prop}[i] = Read{p.PropertyType}(ref buffer);
}}" );
                    }
                    else
                    {
                        sb.AppendLine( $"            instance.{prop} = Read{p.PropertyType}(ref buffer, ref tempData);" );
                    }
                }

                return sb.ToString();
            }
        }

        public record PropertyInfo ( string PropertyType, string PropertyName, bool IsArray, int Order );

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