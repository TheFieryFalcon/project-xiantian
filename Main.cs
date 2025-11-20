using System.ComponentModel;

namespace Advoxium.ProjectXiantian {
    class Entry {
        public static void Main() {
            // things that should be in this class:
            // 1: Input parser
            // 2: Help verb (and other non-context dependent verbs, like maybe map or something similar)
            // 3: Data (probably)
            var tree = TweeParser.Parse();
            Console.WriteLine("The node at START-0-00 says");
            Console.WriteLine(tree.Traverse(new Tuple<string, int, int>("START", 0, 0)).Content);
            Console.WriteLine("The node at START-2-00 says");
            Console.WriteLine(tree.Traverse(new Tuple<string, int, int>("START", 2, 0)).Content);
            Console.WriteLine("The node at START-1-01 says");
            Console.WriteLine(tree.Traverse(new Tuple<string, int, int>("START", 1, 1)).Content);
        }
    }
}
