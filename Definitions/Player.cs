using ProjectXiantian.Definitions;
public class Player {
    public int Strength;
    public int Constitution;
    public int Dexterity;
    public int Intelligence;
    public int Wisdom;
    public int Soul;
    public int Charisma;
    public int Luck;
    public int PowerLevel;
    public CultivationRealm CultivationRealm;
    public BodyRealm BodyRealm;
    public ProfessionRealm ForgingRealm;
    public ProfessionRealm TalismanRealm;
    public ProfessionRealm FormationRealm;
    public ProfessionRealm EnchanterRealm;
    public AlchemyRealm AlchemyRealm;
    public ComprehensionRealm ComprehensionRealm;
    public Dictionary<string, SkillsRealm> Skills = new();
    public Dictionary<string, ArtRealm> Art = new();
    public List<Item> Inventory = new();
    public Dictionary<Slot, EquippableItem> EquippedItems = new();
    public int PAT; // check notion for abbreviations
    public int PDF;
    public int HP;
    public int PRG;
    public int PSP;
    public int EVA;
    public int CRT;
    public int MAT;
    public int MDF;
    public int MRG;
    public int MSP;
    public int MCD;
}
