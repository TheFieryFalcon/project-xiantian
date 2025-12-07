using ProjectXiantian.Definitions;
using ProjectXiantian.Content;
using ProjectXiantian.GameContent.Lang;
namespace ProjectXiantian.Commands.General {
    partial class GeneralCommands {
        public static void Info(GameContext context, char[] flags, string[] parameters, bool debug) {
            Player p = context.player; //just a shorthand
            if (flags.Length == 0) {
                AnsiConsole.WriteLine($"{p.Name}    {p.Title}");
                AnsiConsole.WriteLine($"Realm: {km.lang[p.CultivationRealm.ToString()]}");
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
                    if (parameters.Length == 0) {
                        Exceptions.E1();
                        return;
                    }
                    else if (parameters.Length == 1) {
                        switch (parameters[0]) {
                            case "item":
                                for (int i = 0; i < 50; i++) {
                                    // TODO: implement later
                                }
                                break;
                            default:
                                Exceptions.E1();
                                return;
                        }
                    }
                    Item item = ItemMethods.GetItem(ItemMethods.ItemRecord, string.Join(" ", parameters[1..]));
                    if (item is not null) {
                        item.DisplayItem();
                    }
                    else {
                        AnsiConsole.WriteLine("No such item found! 50b");
                    }
                }
                else if (flags.Contains(char.Parse("e"))) {
                    Table table = new();
                    table.AddColumns("Name:", "Description:", "#");
                    table.Centered();
                    table.Width = 40;
                    table.Border = TableBorder.None;
                    KeyValuePair<Item, int>[] page;
                    try { page = context.player.Inventory.ToArray()[..20]; } catch { page = context.player.Inventory.ToArray(); };
                    if (flags.Contains(char.Parse("p"))) {
                        try { int.Parse(parameters[0]); } catch { Exceptions.E1(); return; }
                        Range selpage = ((int.Parse(parameters[0]) - 1) * 20)..;
                        try { page = context.player.Inventory.ToArray()[selpage]; } catch { AnsiConsole.WriteLine("Your inventory does not have that many items! 51"); return; }
                    }
                    else if (parameters.Length > 0) {
                        page = Array.FindAll(context.player.Inventory.ToArray(), x => x.Key.Name.Contains(string.Join(" ", parameters)));
                        if (page.Length > 20) {
                            AnsiConsole.WriteLine("Too many items in search query! 50a");
                            return;
                        }
                        else if (page.Length == 0) {
                            AnsiConsole.WriteLine("No search result found! Note that unlike with -s you can only search for names, not IDs. 50c");
                            return;
                        }
                    }
                    foreach (KeyValuePair<Item, int> itemstack in page) {
                        if (itemstack.Key.Description.Length < 40) {
                            table.AddRow(itemstack.Key.Name, itemstack.Key.Description, itemstack.Value.ToString());
                        }
                        else {
                            table.AddRow(itemstack.Key.Name, itemstack.Key.Description[..40] + "...", itemstack.Value.ToString());
                        }
                    }
                    AnsiConsole.Write(table);
                    if (!flags.Contains(char.Parse("p")) && parameters.Length == 0) {
                        AnsiConsole.Write(new Align(new Text($"Page 1 / {Math.Ceiling((decimal)(context.player.Inventory.Count / 20))}"), HorizontalAlignment.Center));
                        AnsiConsole.WriteLine();
                    }
                    else if (parameters.Length > 0) {
                        // do nothing LMAO get trolled
                        return;
                    }
                    else {
                        AnsiConsole.Write(new Align(new Text($"Page {int.Parse(parameters[0])} / {Math.Ceiling((decimal)(context.player.Inventory.Count / 20))}"), HorizontalAlignment.Center));
                        AnsiConsole.WriteLine();
                    }
                    
                }
                else {
                    Exceptions.E1();
                }
            }
        }
    }
}
