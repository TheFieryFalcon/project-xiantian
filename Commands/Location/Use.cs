using ProjectXiantian.Content;
using ProjectXiantian.Definitions;

namespace ProjectXiantian.Commands.Location {
    partial class LocationCommands {
        public static GameContext Use(GameContext context, string[] vparameters, char[] flags, string[] parameters) {
            Item item = ItemMethods.GetItem(ItemMethods.ItemRecord, vparameters[0]);
            if (item is null) {
                AnsiConsole.WriteLine("No such item found! You can use item ID or name in any command which needs an item. 1004");
            }
            foreach (Effect effect in item.Effects) {
                int i = 0;
                try {i = ConditionalStatement.Evaluate(context, effect.Antecedent).Item2; } catch { Console.WriteLine("This item cannot be used! 1002"); return context; }
                if (i == 0) {
                    AnsiConsole.WriteLine("You do not meet the requirements for using this item. 1003");
                    return context;
                }
                else if (i == 1) {
                    var temp = ConditionalStatement.Evaluate(context, effect.Consequent);
                    i = temp.Item2;
                    if (i != 4) {
                        AnsiConsole.WriteLine($"Assignment conditional parser exited with exit code {i}. Please report to developer! 1000");
                        return context;
                    }
                    else {
                        context = temp.Item1;
                        return context;
                    }
                }
                else {
                    AnsiConsole.WriteLine($"Non-assignment conditional parser exited with exit code {i}. Please report to developer! 1001");
                    return context;
                    
                }
            }
            AnsiConsole.WriteLine("This item cannot be used! 1002");
            return context;

        }
    }
}
