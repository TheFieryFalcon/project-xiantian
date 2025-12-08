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
        public Quality quality { get; set; }
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
        public void DisplayItem(bool debug) {
            AnsiConsole.WriteLine(Name);
            AnsiConsole.WriteLine(Description);
            if (debug) {
                AnsiConsole.MarkupLine($"[italic]{Id}[/]");
            }
            AnsiConsole.WriteLine($"{Type}");
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine($"Rarity: {km.lang[Rarity.ToLocString()]}");
            if (IsEquippable == true) {
                AnsiConsole.WriteLine($"Slot: {km.lang[CEquippable.Slot.ToLocString()]}");
                AnsiConsole.WriteLine("Stats:");
                int ilevel = 0;
                if (CEquippable != null) {
                    foreach (Tuple<PAttribute, int> item in CEquippable.AttributeModifiers) {
                        AnsiConsole.WriteLine($"{Math.Sign(item.Item2) switch { 0 => "-", 1 => "+" }}{item.Item2} {km.lang[item.Item1.ToLocString()]}");
                        ilevel += item.Item2;
                    }
                    AnsiConsole.WriteLine($"Item Level: {ilevel}");
                }
                else {
                    AnsiConsole.WriteLine("This item is useless!");
                }
            }
            else if (IsMaterial == true) {
                AnsiConsole.WriteLine($"Quality: {CMaterial.LQualityVal} {CMaterial.HQualityVal}");
            }
            if (IsBuyable == true) {
                AnsiConsole.WriteLine($"Market Price: {CBuyable.BuyPrice} {km.lang[CBuyable.BuyCurrency.ToLocString()]}");
            }
            else if (IsSellable == true) {
                AnsiConsole.WriteLine($"Market Price: {CSellable.SellPrice} {km.lang[CSellable.SellCurrency.ToLocString()]}");
            }
            if (Effects is not null) {
                foreach (Effect effect in Effects) {
                    AnsiConsole.WriteLine();
                    AnsiConsole.WriteLine(effect.ToString());
                }
            }
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("Obtainment Methods:");
            if (IsBuyable == true) {
                AnsiConsole.WriteLine("Buying from NPCs");
            }
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
    // consequent cannot **actually** be null, it's just that antecedent should go before consequent
    public class Effect(ConditionalStatement antecedent = null, ConditionalStatement consequent = null, EventHandler handler = null, string name = null) {
        public string Name { get; set; } = name;
        public ConditionalStatement Antecedent { get; set; } = antecedent;
        public EventHandler Event = handler;
        public ConditionalStatement Consequent { get; set; } = consequent;
        public bool IsActive => handler == null;
        override public string ToString() {
            string result = IsActive switch { true => "Active Effect", false => "Passive Effect" };
            if (Name != null) {
                result += Name;
            }
            result += ":";
            if (handler != null) {
                result += "\nOn ";
                result += Misc.SplitCamelCase(handler.Method.Name) + ","; // probably localize handlers instead because holy jank
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
