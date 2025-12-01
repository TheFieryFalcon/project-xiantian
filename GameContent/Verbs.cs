using BidirectionalMap;
namespace ProjectXiantian.Content {
    class Verbs {
        public static BiMap<string, int> TransitiveVerbs = new();
        public static List<string> StateUnchangingVerbs = new();
        public static void Fill() {
            TransitiveVerbs.Add("teleport", 3);
            TransitiveVerbs.Add("choose", 1);
            TransitiveVerbs.Add("use", 1);
            TransitiveVerbs.Add("set", 3);
            StateUnchangingVerbs.Add("save");
            StateUnchangingVerbs.Add("undo");
            StateUnchangingVerbs.Add("help");
            StateUnchangingVerbs.Add("info");
        }
    }
}
