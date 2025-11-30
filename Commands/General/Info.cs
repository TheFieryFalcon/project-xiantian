using ProjectXiantian.Definitions;
using ProjectXiantian.Content;
namespace ProjectXiantian.Commands.General {
    partial class GeneralCommands {
        public static void Info(GameContext context, char[] flags, string[] parameters, bool debug) {
            Player p = context.player; //just a shorthand
            if (flags.Length == 0) {
                AnsiConsole.WriteLine($"{p.Name}    {p.Title}");
                AnsiConsole.WriteLine($"Realm: {p.CultivationRealm.ToString()}");
                AnsiConsole.WriteLine($"Power Lvl: {p.PowerLevel}");
                AnsiConsole.WriteLine($"Active Quest: Name");
                AnsiConsole.WriteLine($"Objective: Task");
                Table Table = new();
                Table.AddColumns("", "", "", "");
                Table.HideHeaders();
                Table.LeftAligned();
                Table.AddRow("Martial", "STR", "CON", "DEX");
                Table.AddRow("Stats:", p.Strength.ToString(), p.Constitution.ToString(), p.Dexterity.ToString());
                Table.AddEmptyRow();
                Table.AddRow("Qigong", "INT", "WIS", "SOU");
                Table.AddRow("Stats:", p.Intelligence.ToString(), p.Wisdom.ToString(), p.Soul.ToString());
                AnsiConsole.Write(Table);
                AnsiConsole.WriteLine("You can get more info by running with flags!");
            }
            else {
                if (flags.Contains(char.Parse("s"))) {
                    if (parameters.Length > 0 && (parameters[0] == "item" || parameters[0].StartsWith("item"))) {
                        Item item = ItemMethods.GetItem(context.ItemRecord, string.Join(" ", parameters[1..]));
                        if (item is not null) {
                            AnsiConsole.WriteLine(item.Name);
                            AnsiConsole.MarkupLine($"[italic]{item.Id}[/]");
                            AnsiConsole.WriteLine($"Rarity: {item.Rarity.ToString()}");
                            if (item.IsEquippable == true) {
                                AnsiConsole.WriteLine($"Slot: {item.CEquippable.Slot}");
                                AnsiConsole.WriteLine($"Effects: ");

                            }
                            AnsiConsole.WriteLine("Obtainment Methods:");

                        }
                    }
                }
            }
        }
    }
}
