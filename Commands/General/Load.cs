using ProjectXiantian.Definitions;
using ProjectXiantian.Methods;
namespace ProjectXiantian.Commands.General {
    partial class GeneralCommands {
        public static GameContext Load(GameContext context, char[] flags, string[] parameters) {
            int number = 1;
            if (flags.Contains(char.Parse("n"))) {
                try { number = int.Parse(parameters[0]); } catch { Console.WriteLine("Parameter of -n must be a number between 1 and 5!"); return context; }
            }
            Functions.Load(context, number);
            AnsiConsole.Clear();
            AnsiConsole.WriteLine($"Loaded save slot {parameters[0]} successfully!\n");
            return context;
        }
    }
}