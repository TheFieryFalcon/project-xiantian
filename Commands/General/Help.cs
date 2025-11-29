namespace ProjectXiantian.Commands.General {
    partial class GeneralCommands {
        public static void Help(string[] parameters, bool debug) {
            if (parameters.Length > 0) {
                switch (parameters[0]) {
                    case "save":
                        AnsiConsole.WriteLine("Command 003: Save\n\nSaves your game.Intransitive verb(takes 0 arguments).\n\nFlags:\n\n - d<number>: deletes that save\n\n - n<number>: specifies a save slot number(overwrites your save)\n\n - q: quits the game.\n\nA location is saveable if you are in one of the following things:\n\n1.The primary town square of a town or city\n2.A save point, usually some sort of glowing tablet or statue\n3.Any location where a long rest is possible(e.g.oasis, camp, ship, etc)\n\nThere are five save slots, no more, no less.");
                        break;
                    case "load":
                        AnsiConsole.WriteLine("Command 004: Load\n\nLoads a game from your saves folder. Intransitive verb (takes 0 arguments).\n\nFlags:\n\n-n [number]: Loads from that save slot. If not given, displays load dialog.\n\nFor more information about saves, run help -h save.");
                        break;
                    case "default":
                        AnsiConsole.WriteLine("No such command or command explains itself.");
                        break;
                }
            }
            else {
                AnsiConsole.WriteLine("\n\nHow to use Verbs:\n\nVerbs in Project Xiantian take a specific template (verbs and commands are interchangeable). The template is as follows:\n\nverb [any arguments to verb] -[flags (max one that takes an argument)] [any arguments to flags]\n\nFor example, if I wanted to sell five Iron to Gerald, my command would look something like this:\n\nsell 5 iron -r gerald\n\nwhere sell is the verb, 5 iron is the verb’s argument, r is the flag, and gerald is the flag’s argument.\n\nEverything to do with verbs is ALWAYS lowercase, except for some debug commands.\n\nIn this help page, required arguments are represented by angled brackets (<>), and optional arguments are represented by square brackets ([]).\n\nGeneral Context Verbs\n\n002 - help [-h <verb>]: Opens this screen. -h gives more specific information on verbs (if available).\n\n003 - save [-q/n <number>]: Saves your game. -q quits the game too.\n\n004 - load [-n <number>]: Loads your game.\n\n006 - undo: Fully undoes your previous action, unless it can’t be undone.\n\nStory Verbs\n\n998 - continue: Continues the story.\n\n999 - choose <choice>: Makes a choice in the story.\n");
                if (debug) {
                    AnsiConsole.WriteLine("Debug Help\n\nEnums that need to be upper case are displayed as such.\n\n000 - debug: Toggles the debug variable. Does not persist through loads.\n\n001 - teleport <SECTION> <stem> <leaf>: Teleports you to literally any node.");
                }
                AnsiConsole.WriteLine("Use -h [command] for more specific help");
            }
        }
    }
}
