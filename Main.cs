global using Spectre.Console;
using ProjectXiantian.Definitions;
using ProjectXiantian.Methods;
using ProjectXiantian.Commands.General;
namespace ProjectXiantian {
    class Entry {
        public static bool debug = true; //CHANGE BEFORE RELEASE
        public static void Main() {
            TweeTree tree = TweeParser.Parse();
            TweeNode CurrentNode = tree.Root;
            GameContext context = new(tree, CurrentNode, new());
            Verbs.Fill();
            if (!Directory.Exists("./Saves"))
            {
                Directory.CreateDirectory("./Saves");
            }
            DirectoryInfo di = new DirectoryInfo("./Saves");
            if (di.GetFiles().Length != 0) {
                if (di.GetFiles().Length == 1 && di.GetFiles()[0].Length > 2) {
                    context = Functions.Load(context, int.Parse(di.GetFiles()[0].Name[^5..^4]));
                }
                else {
                    int? filenum = null;
                    AnsiConsole.WriteLine("Multiple save games detected. Which one would you like to load?");
                    string SaveExists(bool FileExists) => FileExists switch { false => "EMPTY", true => "EXISTS"};
                    int j = 0;

                    for (int i = 1; i < 6; i++) { 
                        AnsiConsole.Write($"Save {i}: {SaveExists(File.Exists($"./Saves/save{i}.sav"))}");
                        if (File.Exists($"./Saves/save{i}.sav")) { AnsiConsole.WriteLine($" Last Saved: {di.GetFiles()[j].LastWriteTime}"); j++;} else { AnsiConsole.WriteLine(); }
                        
                    }
                    while (filenum == null) {
                        filenum = AnsiConsole.Ask<int>("");
                    }
                    Functions.Load(context, filenum);
                }
            }
            if (context == null) {
                throw new NullReferenceException("Something went wrong while loading the game!");
            }
            else {
                AnsiConsole.Clear();
                if (di.GetFiles().Length == 0) {
                    string input = AnsiConsole.Ask<string>("No previous save detected! Would you like to play a brief tutorial?\n(Type \"yes\" or \"no\" below to enter inputs)");
                    if (input == "yes") {
                        
                    }
                }
                Loop(context);
            }
        }

        public static void Loop(GameContext context) {
            // THIS IS THE MAIN LOOP, it should contain:
            // 1: Input parser
            // 2: Handler for general verbs
            // 3: Redirect for other contexts

            // EX: I travelled to 1, then 2, and I am now in 3; player was in realm 4, then 5, then 6
            GameContext temp = new(context.tree, context.PastNode, context.PastPlayer); // Address is 2, player is in realm 5
            if (context.PastContext is not null && context.PastContext.PastContext is not null) {
                temp.PastContext = context.PastContext.PastContext; // Address is 1, player is in realm 4
            }
            context.PastContext = temp;
            context.PastNode = context.CurrentNode; // Address is 3
            context.PastPlayer = context.player; // Player is in realm 6
            if (context.CurrentNode == null) {
                AnsiConsole.WriteLine("Something has gone very wrong: Current node of context is null! Resetting context...");
                context.CurrentNode = context.tree.Root;
            }
            if (context.PastNode == null || context.CurrentNode != context.PastNode) {
                AnsiConsole.Write(context.CurrentNode.Content);
                context.CurrentNode.Accessed = true; // do not display, this does not actually persist, this is just for the back command
            }
            AnsiConsole.WriteLine();
            string input = AnsiConsole.Ask<string>("Make an action:");
            string verb = input.Split(" ")[0];
            char[] flags = [];
            string[] parameters = [];
            string[] vparameters = [];
            if (input.Split(" ").Length > 1 && !Verbs.TransitiveVerbs.Forward.ContainsKey(verb)) { 
                flags = input.Split(" ")[1][1..].ToCharArray();
                if (input.Split(" ").Length > 2) {
                    parameters = input.Split(" ")[2..];
                }
            }
            else if (Verbs.TransitiveVerbs.Forward.ContainsKey(verb)) {
                int transitivity = Verbs.TransitiveVerbs.Forward.GetValueOrDefault(verb);
                vparameters = input.Split(" ")[1..(transitivity+1)];
                if (input.Split(" ").Length > transitivity + 1) {
                    flags = input.Split(" ")[transitivity + 1][1..].ToCharArray();
                    if (input.Split(" ").Length > transitivity + 2) {
                        parameters = input.Split(" ")[(transitivity+2)..];
                    }
                }

            }
            switch (verb) {
                case "debug":
                    debug = GeneralCommands.Debug(debug);
                    break;
                case "teleport":
                    context = GeneralCommands.Teleport(context, vparameters, debug);
                    break;
                case "help":
                    GeneralCommands.Help(parameters, debug);
                    break;
                case "info":
                    GeneralCommands.Info(context, flags, parameters, debug);
                    break;
                case "save":
                    GeneralCommands.Save(context, flags, parameters);
                    break;
                case "load":
                    context = GeneralCommands.Load(context, flags, parameters);
                    break;
                case "back":
                    context = GeneralCommands.Back(context);
                    break;
                case "undo":
                    context = GeneralCommands.Undo(context);
                    break;
                default:
                    AnsiConsole.WriteLine("Invalid command or invalid context of command; story commands can only be used in stories, battle commands only in battles, etc.");
                    break;
                }
            Loop(context);
        }
    }
}
