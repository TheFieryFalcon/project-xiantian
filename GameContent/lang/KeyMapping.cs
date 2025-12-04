using Hjson;
using System.Text.Json;
namespace ProjectXiantian.GameContent.Lang {
    class km { // because I'm tired of typing km twice every time
        public static Dictionary<string, string> lang = null;
        public static Dictionary<string, string> LoadLangFile(string language) {
            string path = $"./GameContent/lang/{language}/{language}.hjson";
            string defaultpath = $"./GameContent/lang/en_US/en_US.hjson";
            if (!File.Exists(path)) {
                path = defaultpath;
            }
            return JsonSerializer.Deserialize<Dictionary<string, string>>(HjsonValue.Load(path).ToString());
        }
        public static string GetKey(string name) {
            // in case of renaming
            return name.ToLowerInvariant() switch
            {
                _ => name.ToLowerInvariant()
            };
        }
    }
}
