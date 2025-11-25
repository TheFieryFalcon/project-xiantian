using ProjectXiantian.Definitions;
public class Item(int Id, string Name, string Description, Rarity Rarity = Rarity.MORTAL, bool HasWeight = false, int Weight = 1, bool Sellable = false, int SellPrice = 0, Currency SellCurrency = Currency.COINS, int BuyPrice = 0, Currency BuyCurrency = Currency.COINS, bool Buyable = false) {
    public int Id { get; set; } = Id;
    public string Name { get; set; } = Name;
    public string Description { get; set; } = Description;
    public Rarity Rarity { get; set; } = Rarity;
    public bool Sellable { get; set; } = Sellable;
    public int SellPrice { get; set; } = SellPrice;
    public Currency SellCurrency { get; set; } = SellCurrency;
    public int BuyPrice { get; set; } = BuyPrice;
    public Currency BuyCurrency { get; set; } = BuyCurrency;
    public bool Buyable { get; set; } = Buyable;
    public int Weight { get; set; } = Weight;
    public bool HasWeight { get; set; } = HasWeight;
}
public class EquippableItem(int Id, string Name, string Description, List<Tuple<Attribute, int>> AttributeModifiers, Slot Slot, Quality Quality, Rarity Rarity = Rarity.MORTAL, bool HasWeight = false, int Weight = 1, bool Sellable = false, int SellPrice = 0, Currency SellCurrency = Currency.COINS, int BuyPrice = 0, Currency BuyCurrency = Currency.COINS, bool Buyable = false) : Item(Id, Name, Description, Rarity, HasWeight, Weight, Sellable, SellPrice, SellCurrency, BuyPrice, BuyCurrency, Buyable) { 
    List<Tuple<Attribute, int>> AttributeModifiers { get; set; } = new();
    public Slot? Slot { get; set; } = null;
    public int QualityVal { get; set; }
}
public class Material (int Id, string Name, string Description, int QualityVal, Rarity Rarity = Rarity.MORTAL, bool HasWeight = false, int Weight = 1, bool Sellable = false, int SellPrice = 0, Currency SellCurrency = Currency.COINS, int BuyPrice = 0, Currency BuyCurrency = Currency.COINS, bool Buyable = false) : Item(Id, Name, Description, Rarity, HasWeight, Weight, Sellable, SellPrice, SellCurrency, BuyPrice, BuyCurrency, Buyable) {
    public int QualityVal { get; set; }
}