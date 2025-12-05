using System.Text.Json;
using System.Text.Json.Serialization;
using ProjectXiantian.Content;
using ProjectXiantian.Definitions;
namespace ProjectXiantian.Methods {
    public class InventoryConverter : JsonConverter<Dictionary<Item, int>> {
        public override void Write(Utf8JsonWriter writer, Dictionary<Item, int> orig, JsonSerializerOptions options) {
            writer.WriteStartObject();
            foreach (var pair in orig) {
                writer.WriteNumber(pair.Key.Id, pair.Value);
            }
            writer.WriteEndObject();
        }
        public override Dictionary<Item, int> Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options) {
            Dictionary<Item, int> inventory = new();
            reader.Read();
            while (reader.TokenType != JsonTokenType.EndObject) {
                string key = reader.GetString();
                reader.Read();
                int value = reader.GetInt32();
                inventory.Add(ItemMethods.GetItem(ItemMethods.ItemRecord, key), value);
                reader.Read();
            }
            return inventory;
        }
    }
    public class Save {
        public int FormatId { get; set; } = Misc.SaveFormatId;
        public Tuple<string, int, int> CurrentNodeAddress { get; set; }
        public Player player { get; set; }
    }
    partial class Functions {
        public static GameContext Load(GameContext context, int? filenum) {
            Save save;
            if (filenum is null || filenum > 5) {
                return null;
            }
            else {
                try {
                    using (StreamReader sr = new($"./Saves/save{filenum}.sav")) {
                        using (JsonDocument savejson = JsonDocument.Parse(sr.ReadToEnd())) {
                            save = savejson.RootElement.Deserialize<Save>();
                        }
                        if (Misc.SaveFormatId == save.FormatId) {
                            context.player = save.player;
                            context.CurrentNode = context.tree.Traverse(save.CurrentNodeAddress);
                        }
                        else {
                            // handle all update logic here
                        }
                        return context;
                    }
                }
                catch { AnsiConsole.WriteLine("No such file found!"); return context; }
            }
        }
        public static void Save(GameContext context, int filenum = 1) {
            if (filenum > 5) {
                Exceptions.E1();
            }
            else {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var save = new Save { CurrentNodeAddress = context.CurrentNode.Address, player = context.player };
                File.WriteAllText($"./Saves/save{filenum}.sav", JsonSerializer.Serialize(save, options));
                return;
            }
        }
    }
}
