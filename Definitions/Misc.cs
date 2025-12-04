namespace ProjectXiantian.Definitions {
    public class GameContext(TweeTree tree, TweeNode currentNode, Player player) {
        public TweeNode CurrentNode { get; set; } = currentNode;
        public TweeTree tree { get; set; } = tree;
        public Player player { get; set; } = player;
        public TweeNode PastNode { get; set; } = null;
        public List<Tuple<string, string, Item>> ItemRecord = null;
    }
    public class Misc {
        public static int SaveFormatId = 1; //UPDATE EVERY FORMAT CHANGE
        public static string StoryTreePath = "./GameContent/lang/en_US/ProjectXiantian.twee";
        public static string SplitCamelCase(string input) { // thanks stack overflow
            return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }
    }
    public class Exceptions {
        public static void E1() {
            AnsiConsole.WriteLine("Invalid parameters passed to verb. Please refer to help for more information. Error 1");
        }
        public static void E2() {
            AnsiConsole.WriteLine("Something was null that shouldn't be. Report this to the developers! Error 2");
        }
        public static void E3() {
            AnsiConsole.WriteLine("Debug mode is not enabled! Error 3");
        }
        public static void E4() {
            AnsiConsole.WriteLine("Invalid command or invalid context of command; story commands can only be used in stories, battle commands only in battles, etc. Error 4");
        }
    }
}
