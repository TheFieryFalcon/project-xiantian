using ProjectXiantian.Definitions;

public class Player {
    public int Strength = 0;
    public int Constitution = 0;
    public int Dexterity = 0;
    public int Intelligence = 0;
    public int Wisdom = 0;
    public int Soul = 0;
    public int Charisma = 0;
    public int Luck = 0;
    public int PowerLevel = 0;
    public CultivationRealm CultivationRealm = 0;
    public BodyRealm BodyRealm = 0;
    public ProfessionRealm ForgingRealm = 0;
    public ProfessionRealm TalismanRealm = 0;
    public ProfessionRealm FormationRealm = 0;
    public ProfessionRealm EnchanterRealm = 0;
    public AlchemyRealm AlchemyRealm = 0;
    public ComprehensionRealm ComprehensionRealm = 0;
    public Dictionary<string, SkillsRealm> Skills = new();
    public Dictionary<string, ArtRealm> Art = new();
    public List<Item> Inventory = new();
    public Dictionary<Slot, EquippableItem> EquippedItems = new();
    public int PAT = 0; // check notion for abbreviations
    public int PDF = 0;
    public int HP = 0;
    public int PRG = 0;
    public int PSP = 0;
    public int EVA = 0;
    public int CRT = 0;
    public int MAT = 0;
    public int MDF = 0;
    public int MRG = 0;
    public int MSP = 0;
    public int MCD = 0;
}