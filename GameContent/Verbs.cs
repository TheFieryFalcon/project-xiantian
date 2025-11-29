using BidirectionalMap;
namespace ProjectXiantian.GameContent {
    class Verbs {
        public static BiMap<string, int> TransitiveVerbs = new();
        public static List<string> StateUnchangingVerbs = new();
        public static void Fill() {
            TransitiveVerbs.Add("teleport", 3);
            TransitiveVerbs.Add("choose", 1);

            StateUnchangingVerbs.Add("save");
            StateUnchangingVerbs.Add("undo");
            StateUnchangingVerbs.Add("help");
            StateUnchangingVerbs.Add("info");
        }
    }
}
