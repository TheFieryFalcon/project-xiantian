using ProjectXiantian.Definitions;

namespace ProjectXiantian.Commands.General {
    partial class GeneralCommands {
        public static GameContext Undo(GameContext context) {
            if (context.PastContext.CurrentNode == null) { AnsiConsole.WriteLine("Unable to undo any further!"); return context; }
            context = context.PastContext;
            return context;
        }
    }
}