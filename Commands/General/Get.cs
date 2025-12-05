using ProjectXiantian.Content;
using ProjectXiantian.Definitions;

namespace ProjectXiantian.Commands.General {
    partial class GeneralCommands {
        public static GameContext Get(GameContext context, string[] vparameters, char[] flags, string[] parameters, bool debug) {
            if (debug == false) {
                Exceptions.E3();
                return context;
            }
            if (vparameters == null) {
                Exceptions.E1();
                return context;
            }
            Item wanted = ItemMethods.GetItem(ItemMethods.ItemRecord, vparameters[0]);
            if (wanted == null) {
                Console.WriteLine("Item not found! 1004");
                return context;
            }
            int i = 0;
            try { i = int.Parse(parameters[1]); } catch { i = 1; }
            if (!context.player.Inventory.ContainsKey(wanted)) { 
                context.player.Inventory.Add(wanted, int.Parse(parameters[1])); 
            } 
            else { 
                context.player.Inventory[wanted] += i; 
            }
            Console.WriteLine($"Successfully given player {i} {wanted.Name}");
            return context;
        }
    }
}