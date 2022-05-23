using System;
using System.Threading.Tasks;
using Blarser.Commands.Commands;
using Blarser.WowContent.WowFiles.Chunks;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Blarser.Commands
{
    [ Subcommand( typeof(CascTestCommand) ) ]
    [ Subcommand( typeof(CascFileCommand) ) ]
    [ Subcommand( typeof(FileChunksCommand) ) ]
    [ Subcommand( typeof(DbcCommand) ) ]
    class Program
    {
        static async Task Main( string[] args )
        {
            var provider = new ServiceCollection();
            provider.AddSingleton( PhysicalConsole.Singleton );

            var _app = new CommandLineApplication<Program> {Description = "Sentient Digital Asset Pipeline"};
            _app.Conventions.UseDefaultConventions().UseConstructorInjection( provider.BuildServiceProvider() );
            _app.OnValidationError( e => Console.WriteLine( e.ErrorMessage ) );

            try
            {
                await _app.ExecuteAsync( args );
            }
            catch (Exception e)
            {
                Console.WriteLine( e.Message );
            }
        }
    }
}