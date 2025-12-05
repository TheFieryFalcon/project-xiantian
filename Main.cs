global using Spectre.Console;
using ProjectXiantian.Definitions;
using ProjectXiantian.Methods;
using ProjectXiantian.Commands.General;
using ProjectXiantian.Commands.ContextParsers;
using Baksteen.Extensions.DeepCopy;
using ProjectXiantian.Content;
using ProjectXiantian.GameContent.Lang;

namespace ProjectXiantian {
    class Entry {
        public static bool debug = true; //CHANGE BEFORE RELEASE
        public static Stack<GameContext> History = new();
        public static void Main() {
            AnsiConsole.WriteLine("Loading localization..."); // implement some kind of actual language checker before release
            km.lang = km.LoadLangFile("en_US");
            AnsiConsole.WriteLine("Building tree...");
            TweeTree tree = TweeParser.Parse();
            TweeNode CurrentNode = tree.Root;
            GameContext context = new(tree, CurrentNode, new());
            AnsiConsole.WriteLine("Loading verbs...");
            Verbs.Fill();
            AnsiConsole.WriteLine("Loading items...");
            ItemMethods.ItemRecord = ItemMethods.Fill();
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
                Loop(context, 0, "");
            }
        }

        public static void Loop(GameContext context, int i, string PastCommand) {
            // THIS IS THE MAIN LOOP, it should contain:
            // 1: Input parser
            // 2: Handler for general verbs
            // 3: Redirect for other contexts
            
            if (context.CurrentNode is null || context is null) {
                Exceptions.E2();
                context.CurrentNode = context.tree.Root;
            }
            if (i == 0 || (context.CurrentNode != context.PastNode && context.PastNode != null)) {
                if (i != 0) {
                    AnsiConsole.WriteLine();
                }
                AnsiConsole.Write(context.CurrentNode.Content);
            }
            context.PastNode = DeepCopyObjectExtensions.DeepCopy(context.CurrentNode);
            GameContext temp = DeepCopyObjectExtensions.DeepCopy(context);
            AnsiConsole.WriteLine();

            string input = AnsiConsole.Ask<string>("Make an action:");
            string verb = input.Split(" ")[0];
            char[] flags = [];
            string[] parameters = [];
            string[] vparameters = [];
            List<string> ptemp = new();
            bool flag = false;
            foreach (string param in input.Split(" ")[1..]) {
                // first
                if (param.Length == 0) {
                    continue;
                }
                // third, resets temp list
                else if (param.StartsWith(char.Parse("-"))) {
                    if (flag) {
                        Console.WriteLine("Command cannot have two flag blocks!");
                        Loop(context, i + 1, "invalid");
                    }
                    flag = true;
                    ptemp = new();
                    flags = param.ToCharArray()[1..];
                }
                // second, fourth
                else {
                    ptemp.Add(param);
                }
            }
            // fifth
            if (flag) {
                parameters = ptemp.ToArray();
            }
            else {
                vparameters = ptemp.ToArray();
            }
            
            // moved from above
            if (!Verbs.StateUnchangingVerbs.Contains(verb)) {
                History.Push(temp);
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
                    GameContext lcontext = GeneralCommands.Load(context, flags, parameters);
                    // reset everything
                    if (lcontext != context) {
                        AnsiConsole.Clear();
                        AnsiConsole.WriteLine($"Loaded save slot {parameters[0]} successfully!\n");
                        context = lcontext;
                        i = 0;
                        debug = false;
                        History.Clear();
                    }
                    break;
                case "undo":
                    // it's a lot easier for me to do this one here
                    if (History.Count > 0) {
                        context = DeepCopyObjectExtensions.DeepCopy(History.Pop());
                    }
                    else {
                        AnsiConsole.WriteLine("Unable to undo any further!");
                    }
                    break;
                case "get":
                    context = GeneralCommands.Get(context, vparameters, flags, parameters, debug);
                    break;
                default:
                    switch (context.CurrentNode.Type) {
                        case NodeType.STORY:

                            break;
                        case NodeType.LOCATION:
                            context = Location.ParseLocation(context, verb, vparameters, flags, parameters);
                            break;
                        case NodeType.BATTLE:

                            break;
                        default:
                            Exceptions.E4();
                            break;
                    }
                    
                    break;
            }
            Loop(context, i + 1, verb);
        }
    }
}
