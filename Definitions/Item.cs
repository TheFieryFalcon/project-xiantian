using ProjectXiantian.Content;
using ProjectXiantian.GameContent.Lang;

namespace ProjectXiantian.Definitions {
    public interface IItem {
        string Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        Rarity Rarity { get; set; }
    }
    public class Buyable(int buyPrice, Currency currency) {
        public int BuyPrice { get; set; } = buyPrice;
        public Currency BuyCurrency { get; set; } = currency;
    }
    public class Sellable(int sellPrice, Currency currency) {
        public int SellPrice { get; set; } = sellPrice;
        public Currency SellCurrency { get; set; } = currency;
    }
    public class Equippable(List<Tuple<PAttribute, int>> AttributeModifiers, Slot slot) {
        public List<Tuple<PAttribute, int>> AttributeModifiers { get; set; }
        public Slot Slot { get; set; }
        public int QualityVal { get; set; }
    }
    public class Material(int LQualityVal, int HQualityVal) {
        public int LQualityVal { get; set; } = LQualityVal;
        public int HQualityVal { get; set; } = HQualityVal;
    }
    public class Item : IItem {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool Consumable { get; set; }
        public Rarity Rarity { get; set; }
        public Effect[] Effects { get; set; }
        // C stands for Component
        public Equippable? CEquippable { get; set; }
        public Sellable? CSellable { get; set; }
        public Buyable? CBuyable { get; set; }
        public Material? CMaterial { get; set; }

        public bool IsEquippable => CEquippable != null;
        public bool IsSellable => CSellable != null;
        public bool IsBuyable => CBuyable != null;
        public bool IsMaterial => CMaterial != null;
        public void DisplayItem() {
            AnsiConsole.WriteLine(Name);
            AnsiConsole.WriteLine(Description);
            AnsiConsole.MarkupLine($"[italic]{Id}[/]");
            AnsiConsole.WriteLine($"{Type}");
            AnsiConsole.WriteLine($"Rarity: {km.lang[Rarity.ToString()]}");
            if (IsEquippable == true) {
                AnsiConsole.WriteLine($"Slot: {km.lang[CEquippable.Slot.ToString()]}");
            }
            else if (IsMaterial == true) {
                AnsiConsole.WriteLine($"Quality: {CMaterial.LQualityVal} {CMaterial.HQualityVal}");
            }
            if (Effects is not null) {
                foreach (Effect effect in Effects) {
                    AnsiConsole.WriteLine();
                    AnsiConsole.WriteLine(effect.ToString());
                }
            }
            if (IsBuyable == true) {
                AnsiConsole.WriteLine($"Market Price: {CBuyable.BuyPrice} {km.lang[CBuyable.BuyCurrency.ToString()]}");
            }
            else if (IsSellable == true) {
                AnsiConsole.WriteLine($"Market Price: {CSellable.SellPrice} {km.lang[CSellable.SellCurrency.ToString()]}");
            }
            AnsiConsole.WriteLine("Obtainment Methods:");
        }
        protected virtual void OnItemConsumed(EventArgs e) {

        }
    }

    public class ItemFactory {
        public static Item CreateItem(string id, Rarity Rarity = Rarity.MORTAL, bool IsMaterial = false, bool IsBuyable = false, bool IsSellable = false, int BuyPrice = 0, int SellPrice = 0, Currency Currency = Currency.NONE, bool Consumable = false, Effect[] effects = null, string Type = "Item") {
            Item result = new Item{ Rarity = Rarity};
            if (Rarity == Rarity.QUEST) {
                result.Id = $"item.quest.{id.ToLower().Replace(" ", "_")}";
            }
            else {
                result.Id = $"item.{id.ToLower().Replace(" ", "_")}";
            }
            result.Type = Type;
            result.Name = km.lang[km.GetKey(result.Id) + ".name"];
            result.Description = km.lang[km.GetKey(result.Id) + ".description"];
            result.Consumable = Consumable;
            if (IsBuyable == true) {
                result.CBuyable = new Buyable(BuyPrice, Currency);
            }
            if (IsSellable == true) {
                result.CSellable = new Sellable(SellPrice, Currency);
            }
            if (Consumable == true) {
                result.Type = string.Concat("Consumable " + Type);
            }
            if (effects != null) {
                result.Effects = effects;
            }
            return result;
        }
        public static Item CreateGear(string id, List<Tuple<PAttribute, int>> AttributeModifiers, Slot Slot, Rarity Rarity = Rarity.MORTAL, bool IsBuyable = false, bool IsSellable = false, int BuyPrice = 0, int SellPrice = 0, Currency Currency = Currency.NONE, Effect[] effects = null) {
            Item result = CreateItem(id, Rarity, false, IsBuyable, IsSellable, BuyPrice, SellPrice, Currency, false, effects, "Equipment");
            result.CEquippable = new Equippable(AttributeModifiers, Slot);
            return result;
        }
        public static Item CreateMaterial(string id, int LQualityVal, int HQualityVal, Rarity Rarity = Rarity.MORTAL, bool IsBuyable = false, bool IsSellable = false, int BuyPrice = 0, int SellPrice = 0, Currency Currency = Currency.NONE) {
            Item result = CreateItem(id, Rarity, false, IsBuyable, IsSellable, BuyPrice, SellPrice, Currency, Type:"Material");
            result.CMaterial = new Material(LQualityVal, HQualityVal);
            return result;
        }
    }

    public class Effect(ConditionalStatement a, ConditionalStatement c, EventHandler handler = null, string name = null) {
        public string Name { get; set; } = name;
        public ConditionalStatement Antecedent { get; set; } = a;
        public EventHandler Event = handler;
        public ConditionalStatement Consequent { get; set; } = c;
        public bool IsActive => handler == null;
        override public string ToString() {
            string result = IsActive switch { true => "Active Effect", false => "Passive Effect" };
            if (Name != null) {
                result += Name;
            }
            result += ":";
            if (handler != null) {
                result += "\nOn ";
                result += Misc.SplitCamelCase(handler.Method.Name) + ","; // change this tomorrow
            }
            result += $"\nIf {Antecedent.ToString()},";
            result += $"\n{Consequent.ToString()}";
            // Active Effect:
            // On Attack,
            // If Strength > 10 and Defense > 10,
            // HP increases by 5
            // Passive Effect:
            // If Strength > 10 and Defense > 10,
            // Regeneration decreases by 2
            return result;
        }
    }
}
