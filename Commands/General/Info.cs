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
                if (flags.Contains(char.Parse("i"))) {
                    Item item = ItemMethods.GetItem(context.ItemRecord, string.Join(" ", parameters));
                    if (item is not null) {
                        AnsiConsole.WriteLine(item.Name);
                        AnsiConsole.MarkupLine($"[italic]{item.Id}[/]");
                        AnsiConsole.WriteLine($"{item.Type}");
                        AnsiConsole.WriteLine($"Rarity: {item.Rarity.ToString()}");
                        if (item.IsEquippable == true) {
                            AnsiConsole.WriteLine($"Slot: {item.CEquippable.Slot}");
                        }
                        else if (item.IsMaterial == true) {
                            AnsiConsole.WriteLine($"Quality: {item.CMaterial.LQualityVal} {item.CMaterial.HQualityVal}");
                        }
                        if (item.Effects is not null) {
                            foreach (Effect effect in item.Effects) {
                                AnsiConsole.WriteLine();
                                AnsiConsole.WriteLine(effect.ToString());
                            }
                        }
                        if (item.IsBuyable == true) {
                            AnsiConsole.WriteLine($"Market Price: {item.CBuyable.BuyPrice} {item.CBuyable.BuyCurrency}");
                        }
                        else if (item.IsSellable == true) {
                            AnsiConsole.WriteLine($"Market Price: {item.CSellable.SellPrice} {item.CSellable.SellCurrency}");
                        }
                        AnsiConsole.WriteLine("Obtainment Methods:");

                    }
                    else {
                        AnsiConsole.WriteLine("No such item found!");
                    }
                }
                else {
                    AnsiConsole.WriteLine("Invalid flags!");
                }
                
            }
        }
    }
}
