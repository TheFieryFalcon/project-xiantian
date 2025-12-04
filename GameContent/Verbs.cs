using BidirectionalMap;
namespace ProjectXiantian.Content {
    class Verbs {
        public static List<string> StateUnchangingVerbs = new();
        public static void Fill() {
            StateUnchangingVerbs.Add("save");
            StateUnchangingVerbs.Add("undo");
            StateUnchangingVerbs.Add("help");
            StateUnchangingVerbs.Add("info");
        }
    }
}
