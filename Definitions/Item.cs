using ProjectXiantian.Definitions;
public interface IItem {
    string Id { get; set; }
    string Name { get; set; }
    string Description { get; set; }
    Rarity Rarity { get; set; }
}
public class Buyable(int buyPrice, Currency currency) {
    int BuyPrice { get; set; } = buyPrice;
    Currency BuyCurrency { get; set; } = currency;
}
public class Sellable(int sellPrice, Currency currency) {
    int SellPrice { get; set; } = sellPrice;
    Currency SellCurrency { get; set; } = currency;
}
public class Equippable(List<Tuple<PAttribute, int>> AttributeModifiers, Slot slot) {
    List<Tuple<PAttribute, int>> AttributeModifiers { get; set; }
    Slot? Slot { get; set; }
    int QualityVal { get; set; }
}
public class Material {
    int QualityVal { get; set; }
}
public class Item : IItem {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Rarity Rarity { get; set; }
    // C stands for Component
    public Equippable? CEquippable { get; set; }
    public Sellable? CSellable {  get; set; }
    public Buyable? CBuyable { get; set; }
    public Material? CMaterial { get; set; }

    public bool IsEquippable => CEquippable != null;
    public bool IsSellable => CSellable != null;
    public bool IsBuyable => CBuyable != null;
    public bool IsMaterial => CMaterial != null;
}

public class ItemFactory {
    public static Item CreateItem(string Name, string Description, Rarity Rarity = Rarity.MORTAL, bool IsMaterial = false, bool IsBuyable = false, bool IsSellable = false, int BuyPrice = 0, int SellPrice = 0, Currency Currency = Currency.NONE) {
        Item result = new Item {
            Name = Name,
            Description = Description,
            Rarity = Rarity
        };
        if (Rarity == Rarity.QUEST) {
            result.Id = $"item.quest.{Name.ToLower().Replace(" ", "_")}";
        }
        else {
            result.Id = $"item.{Name.ToLower().Replace(" ", "_")}";
        }
        if (IsBuyable == true) {
            result.CBuyable = new Buyable(BuyPrice, Currency);
        }
        if (IsSellable == true) {
            result.CSellable = new Sellable(SellPrice, Currency);
        }
        if (IsMaterial == true) {
            result.CMaterial = new Material();
        }
        return result;
    }
    public static Item CreateGear(string Name, string Description, List<Tuple<PAttribute, int>> AttributeModifiers, Slot Slot, Rarity Rarity = Rarity.MORTAL, bool IsBuyable = false, bool IsSellable = false, int BuyPrice = 0, int SellPrice = 0, Currency Currency = Currency.NONE) {
        Item result = CreateItem(Name, Description, Rarity, false, IsBuyable, IsSellable, BuyPrice, SellPrice, Currency);
        result.CEquippable = new Equippable(AttributeModifiers, Slot);
        return result;
    }
}