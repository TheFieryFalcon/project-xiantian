using System.Text.Json;
using ProjectXiantian.Definitions;
namespace ProjectXiantian.Functions {
    public class Save {
        public Tuple<string, int, int> CurrentNodeAddress;
        public Player Player;
    }
    class SaveLoad {
        public static GameContext Load(GameContext context, int? filenum) {
            if (filenum is null || filenum > 5) {
                return null;
            }
            else {
                JsonElement root;
                using (StreamReader sr = new($"./Saves/save{filenum}.sav")) {
                    //using (JsonDocument save = JsonDocument.Parse(sr.ReadToEnd())) {
                      //  root = save.RootElement.Clone();
                    //}
                    //WIP
                    return context;
                }
            }
        }
        public static string Save(GameContext context, int filenum = 1) {
            if (filenum > 5) {
                return ("Parameter of -n must be a number between 1 and 5!");
            }
            else {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var save = new Save { CurrentNodeAddress = context.CurrentNode.Address, Player = context.player };
                File.WriteAllText($"./Saves/save{filenum}.sav", JsonSerializer.Serialize(save));
                return null;
            }
        }
    }
}
