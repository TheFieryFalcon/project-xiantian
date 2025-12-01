using System.Text.Json;
using ProjectXiantian.Definitions;
namespace ProjectXiantian.Methods {
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
