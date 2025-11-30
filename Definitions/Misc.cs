using System.Linq.Expressions;
using System.Reflection;
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
}
