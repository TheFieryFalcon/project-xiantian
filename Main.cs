using ProjectXiantian.Definitions;
namespace ProjectXiantian {
    class Entry {
        public static bool debug = true; //CHANGE BEFORE RELEASE
        public static void Main() {
            TweeTree tree = TweeParser.Parse();
            TweeNode CurrentNode = tree.Root;
            Verbs.Fill();
            //Functions.SaveLoad.Load();
            Loop(tree, CurrentNode, null);
        }

        public static void Loop(TweeTree tree, TweeNode CurrentNode, TweeNode PastNode) {
            // THIS IS THE MAIN LOOP, it should contain:
            // 1: Input parser
            // 2: Help verb (and other non-context dependent verbs, like maybe map or something similar)
            // 3: Data (probably)
            if (PastNode == null || CurrentNode != PastNode) {
                Console.Write(CurrentNode.Content);
                Console.WriteLine();
            }
            PastNode = CurrentNode;
            Console.WriteLine("Make an action:");
            string input = Console.ReadLine();
            string verb = input.Split(" ")[0];
            string flags = "";
            string[] parameters = null;
            string[] vparameters = null;
            if (input.Split(" ").Length > 1 && !Verbs.TransitiveVerbs.Forward.ContainsKey(verb)) { 
                flags = input.Split(" ")[1].Remove(0);
                if (input.Split(" ").Length > 2) {
                    parameters = input.Split(" ")[2..];
                }
            }
            else if (Verbs.TransitiveVerbs.Forward.ContainsKey(verb)) {
                int transitivity = Verbs.TransitiveVerbs.Forward.GetValueOrDefault(verb);
                vparameters = input.Split(" ")[1..(transitivity+1)];
                if (input.Split(" ").Length > transitivity + 1) {
                    flags = input.Split(" ")[transitivity + 1].Substring(1);
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
                        int stem;
                        int leaf;
                        // for some reason I can't chain TryParses in an if statement
                        if (int.TryParse(vparameters[1].Trim(), out stem)) {
                            if (int.TryParse(vparameters[2].Trim(), out leaf)) {
                                CurrentNode = tree.Traverse(new(vparameters[0], stem, leaf));
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
                    Console.WriteLine("PLACEHOLDER (GENERAL HELP)");
                    Console.WriteLine("Use -h [command] for more specific help");
                    if (parameters != null) {
                        switch (parameters[0]) {
                            case "save":
                                Console.WriteLine("Command 003: Save\nSaves your game.\nFlags:\n-h: Pulls up this screen.\n-q: Quits game.\nA location is saveable if you are in one of the following things:\n1. The primary town square of a town or city\n2. A save point, usually some sort of glowing tablet or statue\n3. Any location where a long rest is possible (e.g. oasis, camp, ship, etc)");
                                if (debug) {
                                    Console.WriteLine("Debug Information:\n - c: Saves to clipboard. (ONLY IF DEBUG IS ACTIVE)\nSaves are stored in JSON with base64 encryption.");
                                }
                                break;
                        }
                    }
                    break;
                case "info":
                    Console.WriteLine($"You are currently in {CurrentNode.Address.Item1}.");
                    Console.WriteLine($"Your stats are ");
                    break;
                case "save":
                    if (CurrentNode.Properties.Contains("saveable")) {
                        Console.WriteLine("Saving Game...");
                        string outcome = Functions.SaveLoad.ParseSave();
                        Console.WriteLine(outcome);
                        if (flags.Contains(char.Parse("q"))) {
                            Environment.Exit(0);
                        }
                        if (flags.Contains(char.Parse("c"))) {
                            // TODO: IMPLEMENT THIS AFTER IMPLEMENTING SAVE
                        }
                    }
                    else {
                        Console.WriteLine("You cannot save here! \nRun save -h to see when a node is saveable.");
                    }

                    break;
                default:
                    Console.WriteLine("Invalid command or invalid context of command; story commands can only be used in stories, battle commands only in battles, etc.");
                    break;
                }
            Console.WriteLine();
            Loop(tree, CurrentNode, PastNode);
        }
    }
}
