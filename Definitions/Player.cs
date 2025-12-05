using ProjectXiantian.Definitions;
using System.Text.Json.Serialization;
using ProjectXiantian.Methods;
public class Player {
    public string Name { get; set; } = "temp";
    public string Title { get; set; } = "Supreme Venerable";

    public int Strength { get; set; } = 0;
    public int Constitution { get; set; } = 0;
    public int Dexterity { get; set; } = 0;
    public int Intelligence { get; set; } = 0;
    public int Wisdom { get; set; } = 0;
    public int Soul { get; set; } = 0;
    public int Charisma { get; set; } = 0;
    public int Luck { get; set; } = 0;
    public int PowerLevel { get; set; } = 0;

    public CultivationRealm CultivationRealm { get; set; } = 0;
    public BodyRealm BodyRealm { get; set; } = 0;
    public ProfessionRealm ForgingRealm { get; set; } = 0;
    public ProfessionRealm TalismanRealm { get; set; } = 0;
    public ProfessionRealm FormationRealm { get; set; } = 0;
    public ProfessionRealm EnchanterRealm { get; set; } = 0;
    public AlchemyRealm AlchemyRealm { get; set; } = 0;
    public ComprehensionRealm ComprehensionRealm { get; set; } = 0;

    public Dictionary<string, SkillsRealm> Skills { get; set; } = new();
    public Dictionary<string, ArtRealm> Art { get; set; } = new();
    [JsonConverter(typeof(InventoryConverter))]
    public Dictionary<Item, int> Inventory { get; set; } = new();
    public Dictionary<Slot, Item> EquippedItems { get; set; } = new();

    public int PAT { get; set; } = 0;
    public int PDF { get; set; } = 0;
    public int HP { get; set; } = 0;
    public int PRG { get; set; } = 0;
    public int PSP { get; set; } = 0;
    public int EVA { get; set; } = 0;
    public int CRT { get; set; } = 0;
    public int MAT { get; set; } = 0;
    public int MDF { get; set; } = 0;
    public int MRG { get; set; } = 0;
    public int MSP { get; set; } = 0;
    public int MCD { get; set; } = 0;
}