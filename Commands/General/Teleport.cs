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
                        Exceptions.E1();
                    }
                }
                else {
                    Exceptions.E1();
                }
            }
            else {
                Exceptions.E3();
            }
            return context;
        }
    }
}
