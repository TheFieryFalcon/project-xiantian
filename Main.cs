namespace ProjectXiantian {
    class Entry {
        public static TweeTree tree = TweeParser.Parse();
        public static TweeNode CurrentNode = tree.Root;
        public static void Main() {
            // THIS IS THE MAIN LOOP, it should contain:
            // 1: Input parser
            // 2: Help verb (and other non-context dependent verbs, like maybe map or something similar)
            // 3: Data (probably)
            Console.Write(CurrentNode.Content);
            Console.WriteLine("Make an action:");
            string input = Console.ReadLine();
            switch (input) {
                case "help":
                    Console.WriteLine("PLACEHOLDER");
                    break;
                case "info":
                    Console.WriteLine($"You are currently in {CurrentNode.Address.Item1}.");
                    Console.WriteLine($"Your stats are ");
                    break;
            }
        }
    }
}
