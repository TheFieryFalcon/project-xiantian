using ProjectXiantian.Definitions;
namespace ProjectXiantian.Commands.General {
    partial class GeneralCommands {
        public static void Info(GameContext context, char[] flags, string[] parameters, bool debug) {
            AnsiConsole.WriteLine($"You are currently in {context.CurrentNode.Address.Item1}.");
            AnsiConsole.WriteLine($"Your stats are: ");

        }
    }
}
