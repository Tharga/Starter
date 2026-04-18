using Tharga.Console.Commands.Base;

namespace Tharga.Console1.Commands;

internal class HelloCommand : ActionCommandBase
{
    public HelloCommand()
        : base("hello", "Says hello")
    {
    }

    public override void Invoke(string[] param)
    {
        OutputInformation("Hello Tharga World!");
    }
}
