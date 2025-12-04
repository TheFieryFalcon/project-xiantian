using ProjectXiantian.Content;
using ProjectXiantian.Definitions;

namespace ProjectXiantian.Commands.General {
    partial class GeneralCommands {
        public static void Get(GameContext context, string[] vparameters, char[] flags, string[] parameters, bool debug) {
            if (debug == false) {
                Exceptions.E3();
                return;
            }
            Item wanted = ItemMethods.GetItem(context.ItemRecord, vparameters[0]);
            if (wanted == null) {
                Console.WriteLine("Item not found! 1004");
            }
        }
    }
}