namespace ProjectXiantian.Definitions {
    public abstract class Quest {
        public string Name { get; set; }
        public string Description { get; set; }
        // public NPC Giver { get; set; }

    }
    public class FetchQuest : Quest {
        public IItem Requirement { get; set; }
        public int Count { get; set; }
    }
    public static class Quests {
        
    }
}
