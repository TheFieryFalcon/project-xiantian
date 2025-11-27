using ProjectXiantian.Definitions;

namespace ProjectXiantian.Commands.General {
    partial class GeneralCommands {
        public static GameContext Teleport(GameContext context, string[] vparameters, bool debug) {
            if (debug == true) {
                // for some reason I can't chain TryParses in an if statement
                if (int.TryParse(vparameters[1].Trim(), out int stem)) {
                    if (int.TryParse(vparameters[2].Trim(), out int leaf)) {
                        context.CurrentNode = context.tree.Traverse(new(vparameters[0], stem, leaf));
                        return context;
                    }
                    else {
                        AnsiConsole.WriteLine("Stem and leaf numbers of address must be integers!");
                    }
                }
                else {
                    AnsiConsole.WriteLine("Stem and leaf numbers of address must be integers!");
                }
            }
            else {
                AnsiConsole.WriteLine("Debug mode is not enabled!");
            }
            return context;
        }
    }
}
