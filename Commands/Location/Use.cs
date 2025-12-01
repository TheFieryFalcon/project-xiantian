using ProjectXiantian.Content;
using ProjectXiantian.Definitions;

namespace ProjectXiantian.Commands.Location {
    partial class LocationCommands {
        public static GameContext Use(GameContext context, string[] vparameters, char[] flags, string[] parameters) {
            Item item = ItemMethods.GetItem(context.ItemRecord, vparameters[0]);
            if (item is null) {
                AnsiConsole.WriteLine("No such item found! You can use item ID or name in any command which needs an item.");
            }
            foreach (Effect effect in item.Effects) {
                int i = ConditionalStatement.Evaluate(context, effect.Antecedent).Item2;
                if (i == 0) {
                    AnsiConsole.WriteLine("You do not meet the requirements for using this item.");
                    return context;
                }
                else if (i == 1) {
                    var temp = ConditionalStatement.Evaluate(context, effect.Consequent);
                    i = temp.Item2;
                    if (i != 4) {
                        AnsiConsole.WriteLine($"Assignment conditional parser exited with exit code {i}. Please report to developer!");
                        return context;
                    }
                    else {
                        context = temp.Item1;
                        return context;
                    }
                }
                else {
                    AnsiConsole.WriteLine($"Non-assignment conditional parser exited with exit code {i}. Please report to developer!");
                    return context;
                    
                }
            }
            AnsiConsole.WriteLine("This item cannot be used!");
            return context;

        }
    }
}
