using ProjectXiantian.Definitions;
using System.Reflection;

namespace ProjectXiantian.GameContent {
    internal class Items {
        static readonly PAttribute PAT = PAttribute.PAT; // physical attack
        static readonly PAttribute PDF = PAttribute.PDF; // physical defense
        static readonly PAttribute HP = PAttribute.HP;
        static readonly PAttribute PRG = PAttribute.PRG; // hp regen
        static readonly PAttribute PSP = PAttribute.PSP; // physical speed
        static readonly PAttribute EVA = PAttribute.EVA; // evasion (cf. pokemon)
        static readonly PAttribute CRT = PAttribute.CRT; // crit chance
        static readonly PAttribute MAT = PAttribute.MAT; // magical attack
        static readonly PAttribute MDF = PAttribute.MDF; // magical defense
        static readonly PAttribute MRG = PAttribute.MRG; // mp regen
        static readonly PAttribute MSP = PAttribute.MSP; // cast speed
        static readonly PAttribute CDG = PAttribute.CDG; // crit damage
       
        public static Item TestItem { get; set; } = ItemFactory.CreateItem("Test Item", "This item is for testing only", Rarity.INCOMPREHENSIBLE, false, true, true, 1000000, 1000000, Currency.SPIRIT_STONES);
        public static Item TestSword { get; set; } = ItemFactory.CreateGear("Test Sword", "Legendarily sharp sword of developing", [new(PAT, 1000)], Slot.HAND);

    }
    class ItemMethods {
        public static List<Tuple<string, string, Item>> Fill() {
            List<Tuple<string, string, Item>> result = new(); // id, name, item
            foreach (PropertyInfo property in typeof(Items).GetProperties()) {
                if (property.PropertyType == typeof(Item)) {
                    Item item = new();
                    item = (Item)property.GetValue(item);
                    result.Add(new(item.Id, item.Name, item));
                }
            }
            return result;
        }
        public static Item GetItem(List<Tuple<string, string, Item>> record, string input) {
            foreach (Tuple<string, string, Item> entry in record) {
                if (entry.Item1 == input || entry.Item2 == input) {
                    return entry.Item3;
                }
            }
            return null;
        }
    }
}
