using BidirectionalMap;
namespace ProjectXiantian.Definitions {
    class Verbs {
        public static BiMap<string, int> TransitiveVerbs = new();
        public static void Fill() {
            TransitiveVerbs.Add("teleport", 3);
        }
    }
}
