using System.Diagnostics;

namespace ProjectXiantian.Commands.General {
    partial class GeneralCommands {
        public static void Help(string[] parameters, bool debug) {
            if (parameters != null) {
                switch (parameters[0]) {
                    case "save":
                        AnsiConsole.WriteLine("Command 003: Save\nSaves your game.\nFlags:\n-h: Pulls up this screen.\n-n <number>: specifies a number.\n-q: Quits game.\nA location is saveable if you are in one of the following things:\n1. The primary town square of a town or city\n2. A save point, usually some sort of glowing tablet or statue\n3. Any location where a long rest is possible (e.g. oasis, camp, ship, etc)");
                        if (debug) {
                            AnsiConsole.WriteLine("Debug Information:\n - c: Saves to clipboard. (ONLY IF DEBUG IS ACTIVE)\nSaves are stored in JSON with base64 encryption.");
                        }
                        break;
                }
            }
            else {
                AnsiConsole.WriteLine("PLACEHOLDER (GENERAL HELP)");
                AnsiConsole.WriteLine("Use -h [command] for more specific help");
            }
        }
    }
}
