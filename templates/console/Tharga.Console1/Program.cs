using Tharga.Console;
using Tharga.Console.Commands;
using Tharga.Console.Consoles;

using var console = new ClientConsole();
var command = new RootCommandIoc(console);
#if IncludeSample
command.RegisterCommand<Tharga.Console1.Commands.HelloCommand>();
#endif
var engine = new CommandEngine(command);
engine.Start(args);
