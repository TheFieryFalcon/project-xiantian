namespace ProjectXiantian.Commands.General {
    partial class GeneralCommands {
        public static bool Debug(bool debug) {
            debug = !debug;
            if (debug == true) {
                AnsiConsole.WriteLine("Debug mode successfully enabled.");
                return true;
            }
            else {
                AnsiConsole.WriteLine("Debug mode successfully disabled.");
                return false;
            }
        }
    }
}
