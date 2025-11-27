using ProjectXiantian.Definitions;

namespace ProjectXiantian.Commands.General {
    partial class GeneralCommands {
        public static GameContext Back(GameContext context) {
            List<TweeNode> AccessedNodes = new();
            if (context.CurrentNode.PreviousNodes == null) { AnsiConsole.WriteLine("Unable to go back any further; this is the start node!"); return context; }
            foreach (TweeNode node in context.CurrentNode.PreviousNodes) {
                if (node.Accessed == true) {
                    AccessedNodes.Add(node);
                }
            }
            if (AccessedNodes.Count == 0) { AnsiConsole.WriteLine("You've never been to any of the past nodes!"); }
            else if (AccessedNodes.Count == 1) { context.CurrentNode = AccessedNodes[0]; }
            else {
                AnsiConsole.WriteLine("Which node would you like to go back to?");
                int i = 0;
                foreach (TweeNode node in AccessedNodes) {
                    i++;
                    AnsiConsole.WriteLine("(1) The node in " + node.Address.Item1); //TEST THIS TOMORROW
                }
                try { context.CurrentNode = AccessedNodes[AnsiConsole.Ask<int>("")]; } catch { Console.WriteLine("Invalid input!"); }
            }
            return context;
        }
    }
}