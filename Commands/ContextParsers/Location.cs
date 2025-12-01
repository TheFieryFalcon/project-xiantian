using ProjectXiantian.Commands.Location;
using ProjectXiantian.Definitions;

namespace ProjectXiantian.Commands.ContextParsers {
    internal class Location {
        public static GameContext ParseLocation(GameContext context, string verb, string[] vparameters, char[] flags, string[] parameters) {
            if (context == null || verb == null) {
                Exceptions.E2();
                return context;
            }
            switch (verb) {
                case "use":
                    if (vparameters.Length < 1) {
                        Exceptions.E1();
                        return context;
                    }
                    context = LocationCommands.Use(context, vparameters, flags, parameters);
                    return context;
                default:
                    Exceptions.E4();
                    return context;
            }
        }
    }
}
