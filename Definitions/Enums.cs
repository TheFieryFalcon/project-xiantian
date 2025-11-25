namespace ProjectXiantian.Definitions {
    public enum CultivationRealm {
        MORTAL,
        SPIRIT_EMBRYO,
        QI_GATHERING,
        QI_CONDENSATION,
        QI_CRYSTALLIZATION,
        CORE_FORMATION,
        SOUL_FORGING,
        SEVEN_TREASURES,
        EIGHT_TRIGRAMS
    }
    public enum BodyRealm {
        MORTAL_BODY,
        WOOD_BODY,
        STEEL_BODY
    }
    public enum ProfessionRealm {
        NOT_UNLOCKED,
        NOVICE,
        BEGINNER,
        APPRENTICE,
        JOURNEYMAN,
        EXPERT,
        MASTER,
        GRANDMASTER
    }
    public enum SkillsRealm {
        NOT_UNLOCKED,
        NOVICE,
        BEGINNER,
        APPRENTICE,
        JOURNEYMAN,
        EXPERT,
        MASTER,
        GRANDMASTER
    }
    public enum AlchemyRealm { // because alchemists are snobbish bastards
        FIFTH_RANK,
        FOURTH_RANK,
        THIRD_RANK,
        SECOND_RANK,
        FIRST_RANK,
        BRONZE_MASTER,
        SILVER_MASTER,
        GOLD_MASTER,
        GRANDMASTER
    }
    public enum ArtRealm {
        NOT_UNLOCKED,
        STUDYING,
        MINOR_COMPLETION,
        MAJOR_COMPLETION,
        COMPREHENSION,
        DIDACTIC
    }
    public enum ComprehensionRealm {
        NONE,
        CLUELESS,
        STUDYING,
        REMEMBERING,
        UNDERSTANDING,
        FOLLOWING,
        TEACHING,
        REALIZATION,
        ENLIGHTENMENT,
        RETURN_TO_NATURE,
        HEART_LIKE_STILL_WATER,
        ABANDONMENT_OF_THREE_POISONS,
        OVERCOMING_DUKKHA,
        ONE_WITH_DAO,
        DAO_WITH_ONE,
        ALL_DAOS_WITH_ONE
    }
    public enum Stats {
        STRENGTH,
        CONSTITUTION,
        DEXTERITY,
        INTELLIGENCE,
        WISDOM,
        SOUL,
        CHARISMA,
        LUCK
    }
    public enum Attributes {
        PAT,
        PDF,
        PCD,
        HP,
        PRG,
        PSP,
        EVA,
        CRT,
        MAT,
        MDF,
        MRG,
        MSP,
        CDG
    }
    public enum Rarity {
        QUEST,
        MORTAL,
        LOW_RARE,
        MID_RARE,
        HIGH_RARE,
        LOW_MYSTERIOUS,
        MID_MYSTERIOUS,
        HIGH_MYSTERIOUS,
        LOW_YELLOW,
        MID_YELLOW,
        HIGH_YELLOW,
        LOW_EARTH,
        MID_EARTH,
        HIGH_EARTH,
        LOW_HEAVEN,
        MID_HEAVEN,
        HIGH_HEAVEN,
        LOW_FAIRY,
        MID_FAIRY,
        HIGH_FAIRY,
        LOW_DAO,
        MID_DAO,
        HIGH_DAO
    }
    public enum Quality {
        TRASH,
        USABLE,
        LOW_BASIC,
        MID_BASIC,
        HIGH_BASIC,
        LOW_SIMPLE,
        MID_SIMPLE,
        HIGH_SIMPLE,
        LOW_AVERAGE,
        MID_AVERAGE,
        HIGH_AVERAGE,
        LOW_COMPLEX,
        MID_COMPLEX,
        HIGH_COMPLEX,
        LOW_ADVANCED,
        MID_ADVANCED,
        HIGH_ADVANCED,
        LOW_INTRICATE,
        MID_INTRICATE,
        HIGH_INTRICATE,
        LOW_ARTIFACT,
        MID_ARTIFACT,
        HIGH_ARTIFACT,
        LOW_MAGNUM_OPUS,
        MID_MAGNUM_OPUS,
        HIGH_MAGNUM_OPUS,
        INCOMPREHENSIBLE
    }
    public enum Slot {
        HEAD,
        CHEST,
        ARMS,
        LEGS,
        BOOTS,
        RING,
        NECKLACE,
        TALISMAN,
        RUNE
    }
    public enum Currency {
        COINS, // 8 bronze = 1 silver, 12 silver = 1 gold, 30 gold = 1 platinum; will be used until Qi Condensation realm or so
        SPIRIT_STONES, // factors of 1000, Fragment => Mortal => Earth => Sky => Celestial => Dao
        QI_STONES, // factional currency
        MARTIAL_STONES, // factional currency
    }
    public static class DefFunctions {
        public static Quality ValueToQuality(int QualityVal) => QualityVal switch
        {
            < 10 => Quality.TRASH,
            < 20 => Quality.USABLE,

            // BASIC (5 apart)
            < 25 => Quality.LOW_BASIC, // 20 + 5
            < 30 => Quality.MID_BASIC, // 25 + 5
            < 35 => Quality.HIGH_BASIC, // 30 + 5

            // SIMPLE (10 apart)
            < 45 => Quality.LOW_SIMPLE, // 35 + 10
            < 55 => Quality.MID_SIMPLE, // 45 + 10
            < 65 => Quality.HIGH_SIMPLE, // 55 + 10

            // AVERAGE (25 apart):
            < 90 => Quality.LOW_AVERAGE, // 65 + 25
            < 115 => Quality.MID_AVERAGE, // 90 + 25
            < 165 => Quality.HIGH_AVERAGE, // 115 + 50

            // COMPLEX (50 apart):
            < 215 => Quality.LOW_COMPLEX, // 165 + 50
            < 265 => Quality.MID_COMPLEX, // 215 + 50
            < 365 => Quality.HIGH_COMPLEX, // 265 + 100

            // ADVANCED (100 apart):
            < 465 => Quality.LOW_ADVANCED, // 365 + 100
            < 565 => Quality.MID_ADVANCED, // 465 + 100
            < 865 => Quality.HIGH_ADVANCED, // 565 + 300 (big jump)

            // INTRICATE (300 apart):
            < 1165 => Quality.LOW_INTRICATE, // 865 + 300
            < 1465 => Quality.MID_INTRICATE, // 1165 + 300
            < 1965 => Quality.HIGH_INTRICATE, // 1465 + 500

            // ARTIFACT (500 apart):
            < 2465 => Quality.LOW_ARTIFACT, // 1965 + 500
            < 2965 => Quality.MID_ARTIFACT, // 2465 + 500
            < 4165 => Quality.HIGH_ARTIFACT, // 2965 + 1200 (another big jump)

            // MAGNUM OPUS (1200 apart for all):
            < 5365 => Quality.LOW_MAGNUM_OPUS, // 4165 + 1200
            < 6565 => Quality.MID_MAGNUM_OPUS, // 5365 + 1200
            < 9999 => Quality.HIGH_MAGNUM_OPUS,
            _ => Quality.INCOMPREHENSIBLE // anything higher than 10000 is max quality
        };
    }
}
