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
        public static string StoryTreePath = "./Content/ProjectXiantian.twee";

    }
    public class Exceptions {
        public static void E1() {
            AnsiConsole.WriteLine("Invalid parameters passed to verb. Please refer to help for more information. Error 1");
        }
        public static void E2() {
            AnsiConsole.WriteLine("Something was null that shouldn't be. Report this to the developers! Error 2");
        }
        public static void E3() {
            AnsiConsole.WriteLine("Debug mode is not enabled!");
        }
        public static void E4() {
            AnsiConsole.WriteLine("Invalid command or invalid context of command; story commands can only be used in stories, battle commands only in battles, etc.");
        }
    }
}
