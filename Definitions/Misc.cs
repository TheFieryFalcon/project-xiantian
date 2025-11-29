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
        public static string StoryTreePath = "./GameContent/ProjectXiantian.twee";
        public static void InvalidArgumentException() {
            AnsiConsole.WriteLine("Flag or verb must take (an) argument(s)!");
        }
    }
}
