using ProjectXiantian.Definitions;
using ProjectXiantian.Functions;
namespace ProjectXiantian {
    class Entry {
        public static bool debug = true; //CHANGE BEFORE RELEASE
        public static void Main() {
            TweeTree tree = TweeParser.Parse();
            TweeNode CurrentNode = tree.Root;
            GameContext context = new(tree, CurrentNode, new());
            Verbs.Fill();
            DirectoryInfo di = new DirectoryInfo("./Saves");
            if (di.GetFiles().Length != 0) {
                if (di.GetFiles().Length == 1 && di.GetFiles()[0].Length > 2) {
                    context = Functions.SaveLoad.Load(context, int.Parse(di.GetFiles()[0].Name[^5..^4]));
                }
                else {
                    int? filenum = null;
                    Console.WriteLine("Multiple save games detected. Which one would you like to load?");
                    string SaveExists(bool FileExists) => FileExists switch { false => "EMPTY", true => "EXISTS"};
                    int j;
                    for (int i = 1; i < 6; i++) { 
                        j = 0;
                        Console.Write($"Save {i}: {SaveExists(File.Exists($"./Saves/save{i}.sav"))}");
                        if (File.Exists($"./Saves/save{i}.sav")) { Console.WriteLine($" Last Accessed: {di.GetFiles()[j].LastAccessTime}"); j++;} else { Console.WriteLine(); }
                        
                    }
                    while (filenum == null) {
                        try {filenum = int.Parse(Console.ReadLine()); } catch { Console.WriteLine("Invalid input. Please try again"); }
                    }
                    SaveLoad.Load(context, filenum);
                }
            }
            if (context == null) {
                throw new NullReferenceException("Something went wrong while loading the game!");
            }
            else {
                Console.Clear();
                Loop(context);
            }
        }

        public static void Loop(GameContext context) {
            // THIS IS THE MAIN LOOP, it should contain:
            // 1: Input parser
            // 2: Help verb (and other non-context dependent verbs, like maybe map or something similar)
            // 3: Data (probably)
            if (context.PastNode == null || context.CurrentNode != context.PastNode) {
                Console.Write(context.CurrentNode.Content);
                Console.WriteLine();
            }
            context.PastNode = context.CurrentNode;
            Console.WriteLine("Make an action:");
            string input = Console.ReadLine();
            string verb = input.Split(" ")[0];
            string flags = "";
            string[] parameters = null;
            string[] vparameters = null;
            if (input.Split(" ").Length > 1 && !Verbs.TransitiveVerbs.Forward.ContainsKey(verb)) { 
                flags = input.Split(" ")[1][1..];
                if (input.Split(" ").Length > 2) {
                    parameters = input.Split(" ")[2..];
                }
            }
            else if (Verbs.TransitiveVerbs.Forward.ContainsKey(verb)) {
                int transitivity = Verbs.TransitiveVerbs.Forward.GetValueOrDefault(verb);
                vparameters = input.Split(" ")[1..(transitivity+1)];
                if (input.Split(" ").Length > transitivity + 1) {
                    flags = input.Split(" ")[transitivity + 1][1..];
                    if (input.Split(" ").Length > transitivity + 2) {
                        parameters = input.Split(" ")[(transitivity+2)..];
                    }
                }

            }
            switch (verb) {
                    // DEBUG COMMANDS
                case "debug":
                    debug = !debug;
                    if (debug == true) {
                        Console.WriteLine("Debug mode successfully enabled.");
                    }
                    else {
                        Console.WriteLine("Debug mode successfully disabled.");
                    }
                        break;
                case "teleport":
                    if (debug == true) {
                        // for some reason I can't chain TryParses in an if statement
                        if (int.TryParse(vparameters[1].Trim(), out int stem)) {
                            if (int.TryParse(vparameters[2].Trim(), out int leaf)) {
                                context.CurrentNode = context.tree.Traverse(new(vparameters[0], stem, leaf));
                            }
                            else {
                                Console.WriteLine("Stem and leaf numbers of address must be integers!");
                            }
                        }
                        else {
                            Console.WriteLine("Stem and leaf numbers of address must be integers!");
                        }
                    }
                    else {
                        Console.WriteLine("Debug mode is not enabled!");
                    }
                    break;
                case "help":
                    if (parameters != null) {
                        switch (parameters[0]) {
                            case "save":
                                Console.WriteLine("Command 003: Save\nSaves your game.\nFlags:\n-h: Pulls up this screen.\n-n <number>: specifies a number.\n-q: Quits game.\nA location is saveable if you are in one of the following things:\n1. The primary town square of a town or city\n2. A save point, usually some sort of glowing tablet or statue\n3. Any location where a long rest is possible (e.g. oasis, camp, ship, etc)");
                                if (debug) {
                                    Console.WriteLine("Debug Information:\n - c: Saves to clipboard. (ONLY IF DEBUG IS ACTIVE)\nSaves are stored in JSON with base64 encryption.");
                                }
                                break;
                        }
                    }
                    else {
                        Console.WriteLine("PLACEHOLDER (GENERAL HELP)");
                        Console.WriteLine("Use -h [command] for more specific help");
                    }
                        break;
                case "info":
                    Console.WriteLine($"You are currently in {context.CurrentNode.Address.Item1}.");
                    Console.WriteLine($"Your stats are ");
                    break;
                case "save":
                    if (context.CurrentNode.Properties.Contains("saveable")) {
                        Console.WriteLine("Saving Game...");
                        int number = 1;
                        if (flags.Contains(char.Parse("n"))) {
                            try { number = int.Parse(parameters[0]); } catch { Console.WriteLine("Parameter of -n must be a number between 1 and 5!"); break; }
                        }
                        string outcome = Functions.SaveLoad.Save(context, number);
                        if (outcome is null) {
                            Console.WriteLine("Successfully saved game!");
                        }
                        else {
                            Console.WriteLine("Save failed.\nReason:");
                            Console.WriteLine(outcome);
                        }
                        if (flags.Contains(char.Parse("q"))) {
                            Environment.Exit(0);
                        }
                        if (flags.Contains(char.Parse("c"))) {
                            // TODO: IMPLEMENT THIS AFTER IMPLEMENTING SAVE
                        }
                    }
                    else {
                        Console.WriteLine("You cannot save here! \nRun help -h save to see when a node is saveable.");
                    }

                    break;
                default:
                    Console.WriteLine("Invalid command or invalid context of command; story commands can only be used in stories, battle commands only in battles, etc.");
                    break;
                }
            Console.WriteLine();
            Loop(context);
        }
    }
}
