using ProjectXiantian.Definitions;
using ProjectXiantian.Methods;

namespace ProjectXiantian.Commands.General {
    partial class GeneralCommands {
        public static void Save(GameContext context, char[] flags, string[] parameters) {
            int number = 1;
            if (flags.Contains(char.Parse("d"))) {
                try { File.Delete($"./Saves/save{parameters[0]}.sav"); } catch { Misc.InvalidArgumentException(); return;}
                AnsiConsole.WriteLine("Save successfully deleted!");
                }
            else if (context.CurrentNode.Properties.Contains("saveable")) {
                if (flags.Contains(char.Parse("n"))) {
                    try { number = int.Parse(parameters[0]); } catch { Console.WriteLine("Parameter of -n must be a number between 1 and 5!"); return; } // I despise tryparse, it doesn't even check for null or index exceptions
                }
                string outcome = Functions.Save(context, number);
                if (outcome is null) {
                    AnsiConsole.WriteLine("Successfully saved game!");
                }
                else {
                    AnsiConsole.MarkupLine("\n[bold]Save failed.[/]\nReason:");
                    AnsiConsole.WriteLine(outcome);
                }
                if (flags.Contains(char.Parse("q"))) {
                    Environment.Exit(0);
                }
            }
            else {
                AnsiConsole.WriteLine("You cannot save here! \nRun help -h save to see when a node is saveable.");
            }
        }
    }
}
