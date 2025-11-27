using ProjectXiantian.Definitions;

namespace ProjectXiantian.Commands.General {
    partial class GeneralCommands {
        public static GameContext Undo(GameContext context) {
            try { context = context.PastContext; } catch { AnsiConsole.WriteLine("Unable to undo any further!"); }
            return context;
        }
    }
}