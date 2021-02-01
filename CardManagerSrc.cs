using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{

    public string Name;
    public Sprite Logo;
    public int Attack, Defense, Manacost, Goldcost, MaxDefense;
    public bool IsPlaced;

    public List<CardAbility> Abilities;

    public bool IsSpell;
    public bool IsAlive
    {
        get
        {
            return Defense > 0;

        }
    }
    public bool HasAbility
    {
        get
        {
            return Abilities.Count > 0;

        }
    }
    public bool Ranged
    {
        get
        {
            return Abilities.Exists(x => x.AbilityType == CardAbility.abilityType.RANGED);
        }
    }
    public int TimesTookDamage;
    public int TimesDealDamage;
    public Card(string name, string logoPath, int attack, int defense, int manacost, int goldcost,
                List<CardAbility> abilities)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        Defense = MaxDefense = defense;
        Manacost = manacost;
        Goldcost = goldcost;
        IsPlaced = false;
        TimesDealDamage = TimesTookDamage = 0;

        Abilities = new List<CardAbility>();

        if (abilities != null || abilities.Count > 0)
        {
            foreach (var ability in abilities)
            {
                Abilities.Add(ability);
            }
        }
    }

    public Card(Card card)
    {
        Name = card.Name;
        Logo = card.Logo;
        Attack = card.Attack;
        Defense = MaxDefense = card.Defense;
        Manacost = card.Manacost;
        Goldcost = card.Goldcost;
        IsPlaced = false;
        TimesDealDamage = TimesTookDamage = 0;

        Abilities = new List<CardAbility>();

        if (card.Abilities != null || card.Abilities.Count > 0)
        {
            foreach (var ability in card.Abilities)
            {
                Abilities.Add(ability);
            }
        }
    }


    public Card GetCopy()
    {
        return new Card(this);
    }
}

public class SpellCard : Card
{
    public enum SpellType
    {
        NO_SPELL,
        HEAL_ALLY_FIELD_CARDS,
        DAMAGE_ENEMY_FIELD_CARDS,
        HEAL_ALLY_HERO,
        DAMAGE_ENEMY_HERO,
        SHIELD_ON_ALLY_CARD,
        PROVOCATION_ON_ALLY_CARD,
        BUFF_CARD_DAMAGE,
        DEBUFF_CARD_DAMAGE,
        ADD_GOLD,
        ADD_MANA,
        HEAL_CARD,
        DAMAGE_CARD,
        DOUBLE_ATTACK_ON_ALLY_CARD,
        ARMOR_ON_CARD,
        REGENERATION_ON_CARD,
        REGENERATION_AURA_ON_CARD,
        FIRE_SHIELD_ON_CARD
    }
    public enum TargetType
    {
        NO_TARGET,
        ALLY_CARD_TARGET,
        ENEMY_CARD_TARGET
    }
    public SpellType Spell;
    public TargetType SpellTarget;
    public int SpellValue;

    public SpellCard(string name, string logoPath, int manacost, int goldcost, SpellType spellType = 0, int spellValue = 0, TargetType targetType = 0)
                    : base(name, logoPath, 0, 0, manacost, goldcost, new List<CardAbility>())
    {
        IsSpell = true;
        Spell = spellType;
        SpellValue = spellValue;
        SpellTarget = targetType;
    }

    public SpellCard(SpellCard card) : base(card)
    {
        IsSpell = true;
        Spell = card.Spell;
        SpellValue = card.SpellValue;
        SpellTarget = card.SpellTarget;
    }

    public new SpellCard GetCopy()
    {
        return new SpellCard(this);
    }
}

public static class CardManager
{
    public static List<Card> AllCards = new List<Card>();
}


public class CardManagerSrc : MonoBehaviour
{

    public void Awake()
    {
        CardManager.AllCards.Add(new Card("peasant", "Sprites/Cards/peasant", 1, 4, 0, 1, new List<CardAbility> { new CardAbility(CardAbility.abilityType.GOLD_REGENERATION, 1) }));
        CardManager.AllCards.Add(new Card("peasant archer", "Sprites/Cards/archer-peasant", 1, 4, 0, 2, new List<CardAbility> { new CardAbility(CardAbility.abilityType.RANGED), new CardAbility(CardAbility.abilityType.GOLD_REGENERATION, 1) }));
        CardManager.AllCards.Add(new Card("witch", "Sprites/Cards/witch", 2, 5, 0, 3, new List<CardAbility> { new CardAbility(CardAbility.abilityType.MANA_ON_CAST, 2), new CardAbility(CardAbility.abilityType.RANGED) }));
        CardManager.AllCards.Add(new Card("scarecrow", "Sprites/Cards/scarecrow", 0, 6, 0, 1, new List<CardAbility> { new CardAbility(CardAbility.abilityType.PROVOCATION) }));

        CardManager.AllCards.Add(new Card("skeleton", "Sprites/Cards/skeleton", 1, 3, 0, 0, new List<CardAbility> { new CardAbility(CardAbility.abilityType.NO_ABILITY) }));

        CardManager.AllCards.Add(new Card("hunter archer", "Sprites/Cards/archer-hunter", 2, 5, 0, 4, new List<CardAbility> { new CardAbility(CardAbility.abilityType.RANGED), new CardAbility(CardAbility.abilityType.INSTANT_ACTIVE) }));
        CardManager.AllCards.Add(new Card("lini hunter", "Sprites/Cards/lini-hunter", 3, 7, 0, 5, new List<CardAbility> { new CardAbility(CardAbility.abilityType.INSTANT_ACTIVE), new CardAbility(CardAbility.abilityType.DAMAGE_HERO_ON_CAST, 2) }));

        CardManager.AllCards.Add(new Card("assasin archer", "Sprites/Cards/archer-assasin", 5, 9, 0, 10, new List<CardAbility> { new CardAbility(CardAbility.abilityType.RANGED) }));
        CardManager.AllCards.Add(new Card("guild assasin", "Sprites/Cards/guild-assasin", 4, 2, 0, 5, new List<CardAbility> { new CardAbility(CardAbility.abilityType.INSTANT_ACTIVE), new CardAbility(CardAbility.abilityType.DAMAGE_HERO_ON_CAST, 3) }));

        CardManager.AllCards.Add(new Card("wolf", "Sprites/Cards/wolf", 3, 3, 0, 2, new List<CardAbility> { new CardAbility(CardAbility.abilityType.INSTANT_ACTIVE), new CardAbility(CardAbility.abilityType.REGENERATION_EACH_TURN, 1) }));
        CardManager.AllCards.Add(new Card("alfa wolf", "Sprites/Cards/alfa-wolf", 4, 8, 0, 7, new List<CardAbility> { new CardAbility(CardAbility.abilityType.INSTANT_ACTIVE), new CardAbility(CardAbility.abilityType.PROVOCATION), new CardAbility(CardAbility.abilityType.LIFESTEAL, 2) }));
        CardManager.AllCards.Add(new Card("feral wolf", "Sprites/Cards/feral-wolf", 3, 1, 2, 0, new List<CardAbility> { new CardAbility(CardAbility.abilityType.INSTANT_ACTIVE) }));
        CardManager.AllCards.Add(new Card("giant rat", "Sprites/Cards/giant-rat", 2, 5, 0, 4, new List<CardAbility> { new CardAbility(CardAbility.abilityType.REGENERATION_EACH_TURN, 2) }));


        CardManager.AllCards.Add(new Card("warden", "Sprites/Cards/warden", 4, 6, 0, 6, new List<CardAbility> { new CardAbility(CardAbility.abilityType.ARMOR_ON_CAST, 2) }));
        CardManager.AllCards.Add(new Card("soldier archer", "Sprites/Cards/archer-soldier", 3, 5, 0, 5, new List<CardAbility> { new CardAbility(CardAbility.abilityType.RANGED), new CardAbility(CardAbility.abilityType.ARMOR_ON_CAST, 1) }));
        CardManager.AllCards.Add(new Card("general scarlett", "Sprites/Cards/scarlet-general", 9, 13, 0, 20, new List<CardAbility> { new CardAbility(CardAbility.abilityType.INSTANT_ACTIVE), new CardAbility(CardAbility.abilityType.ARMOR_ON_CAST, 5) }));
        CardManager.AllCards.Add(new Card("guard commander", "Sprites/Cards/guard-commander", 5, 10, 0, 12, new List<CardAbility> { new CardAbility(CardAbility.abilityType.ARMOR_ON_CAST, 7) }));

        CardManager.AllCards.Add(new Card("gnome warrior", "Sprites/Cards/gnome-warrior", 4, 18, 0, 14, new List<CardAbility> { new CardAbility(CardAbility.abilityType.REGENERATION_EACH_TURN, 4), new CardAbility(CardAbility.abilityType.ARMOR_ON_CAST, 3) }));
        CardManager.AllCards.Add(new Card("gnome healer", "Sprites/Cards/gnome-healer", 1, 8, 0, 6, new List<CardAbility> { new CardAbility(CardAbility.abilityType.REGENERATION_EACH_TURN, 1), new CardAbility(CardAbility.abilityType.HEAL_ALLY_FIELD_CARDS_EACH_TURN, 2) }));
        CardManager.AllCards.Add(new Card("forest gnome", "Sprites/Cards/forest-gnome", 1, 5, 0, 3, new List<CardAbility> { new CardAbility(CardAbility.abilityType.HEAL_ALLY_FIELD_CARDS_ON_CAST, 2) }));
        CardManager.AllCards.Add(new Card("clay golem", "Sprites/Cards/clay-golem", 3, 12, 0, 6, new List<CardAbility> { new CardAbility(CardAbility.abilityType.ARMOR_ON_CAST, 5) }));

        CardManager.AllCards.Add(new Card("beholder", "Sprites/Cards/beholder", 5, 15, 3, 15, new List<CardAbility> { new CardAbility(CardAbility.abilityType.LIFESTEAL, 3), new CardAbility(CardAbility.abilityType.DAMAGE_ON_CAST, 3) }));
        CardManager.AllCards.Add(new Card("ancient keeper", "Sprites/Cards/ancient-keeper", 3, 20, 6, 20, new List<CardAbility> { new CardAbility(CardAbility.abilityType.HERO_REGENERATION, 2), new CardAbility(CardAbility.abilityType.MANA_REGENERATION, 2) }));

        CardManager.AllCards.Add(new Card("crusader", "Sprites/Cards/crusader", 3, 6, 1, 6, new List<CardAbility> { new CardAbility(CardAbility.abilityType.HOLY_SHIELD) }));
        CardManager.AllCards.Add(new Card("divine mistress", "Sprites/Cards/divine-mistress", 5, 15, 3, 14, new List<CardAbility> { new CardAbility(CardAbility.abilityType.HOLY_SHIELD), new CardAbility(CardAbility.abilityType.DAMAGE_EACH_TURN, 1) }));
        CardManager.AllCards.Add(new Card("crusader commander", "Sprites/Cards/crusader-commander", 6, 12, 3, 14, new List<CardAbility> { new CardAbility(CardAbility.abilityType.ARMOR_ON_CAST, 5), new CardAbility(CardAbility.abilityType.HOLY_SHIELD) }));
        CardManager.AllCards.Add(new Card("crusader general", "Sprites/Cards/crusader-general", 6, 20, 5, 25, new List<CardAbility> { new CardAbility(CardAbility.abilityType.ARMOR_ON_CAST, 8), new CardAbility(CardAbility.abilityType.HOLY_SHIELD), new CardAbility(CardAbility.abilityType.REGENERATION_EACH_TURN, 2), new CardAbility(CardAbility.abilityType.PROVOCATION) }));
        // CardManager.AllCards.Add(new Card("wild werewolf", "Sprites/Cards/wild-werewolf", 4, 7, 0, 5, new List<CardAbility> {new CardAbility(CardAbility.abilityType.INSTANT_ACTIVE, new CardAbility(CardAbility.abilityType.REGENERATION_EACH_TURN, }));


        // SPELLS    

        CardManager.AllCards.Add(new SpellCard("fireball", "Sprites/Cards/fireball", 3, 0, SpellCard.SpellType.DAMAGE_CARD, 7, SpellCard.TargetType.ENEMY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("magic missle", "Sprites/Cards/magic-missle", 1, 0, SpellCard.SpellType.DAMAGE_CARD, 4, SpellCard.TargetType.ENEMY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("firewall", "Sprites/Cards/firewall", 5, 0, SpellCard.SpellType.DAMAGE_ENEMY_FIELD_CARDS, 5, SpellCard.TargetType.NO_TARGET));
        CardManager.AllCards.Add(new SpellCard("healing wave", "Sprites/Cards/healing-wave 1", 4, 0, SpellCard.SpellType.HEAL_ALLY_FIELD_CARDS, 3, SpellCard.TargetType.NO_TARGET));
        CardManager.AllCards.Add(new SpellCard("mana potion", "Sprites/Cards/mana-potion", 0, 1, SpellCard.SpellType.ADD_MANA, 4, SpellCard.TargetType.NO_TARGET));
        CardManager.AllCards.Add(new SpellCard("lighting bolt", "Sprites/Cards/lightning", 6, 0, SpellCard.SpellType.DAMAGE_ENEMY_HERO, 3, SpellCard.TargetType.NO_TARGET));
        CardManager.AllCards.Add(new SpellCard("healing potion", "Sprites/Cards/healing-potion", 0, 1, SpellCard.SpellType.HEAL_ALLY_HERO, 4, SpellCard.TargetType.NO_TARGET));
        CardManager.AllCards.Add(new SpellCard("healing spell", "Sprites/Cards/healing-spell", 2, 0, SpellCard.SpellType.HEAL_CARD, 6, SpellCard.TargetType.ALLY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("decay", "Sprites/Cards/low_damage", 5, 0, SpellCard.SpellType.DEBUFF_CARD_DAMAGE, 4, SpellCard.TargetType.ENEMY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("provocation", "Sprites/Cards/provocation-spell", 5, 0, SpellCard.SpellType.PROVOCATION_ON_ALLY_CARD, 0, SpellCard.TargetType.ALLY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("damage buff", "Sprites/Cards/damage-buff-spell", 4, 0, SpellCard.SpellType.BUFF_CARD_DAMAGE, 2, SpellCard.TargetType.ALLY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("magic shield", "Sprites/Cards/magic-shield", 2, 0, SpellCard.SpellType.SHIELD_ON_ALLY_CARD, 2, SpellCard.TargetType.ALLY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("double attack", "Sprites/Cards/double-attack", 6, 0, SpellCard.SpellType.DOUBLE_ATTACK_ON_ALLY_CARD, 0, SpellCard.TargetType.ALLY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("armor equipment", "Sprites/Cards/armor-equipment", 0, 2, SpellCard.SpellType.ARMOR_ON_CARD, 6, SpellCard.TargetType.ALLY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("regeneration", "Sprites/Cards/regeneration", 4, 0, SpellCard.SpellType.REGENERATION_ON_CARD, 2, SpellCard.TargetType.ALLY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("healing aura", "Sprites/Cards/heal-allies-buff", 5, 0, SpellCard.SpellType.REGENERATION_AURA_ON_CARD, 1, SpellCard.TargetType.ALLY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("fire shield", "Sprites/Cards/fire-shield", 4, 0, SpellCard.SpellType.FIRE_SHIELD_ON_CARD, 1, SpellCard.TargetType.ALLY_CARD_TARGET));


    }

}
