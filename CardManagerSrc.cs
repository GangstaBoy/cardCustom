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
    public bool IsProvocation
    {
        get
        {
            return Abilities.Exists(x => x.AbilityType == CardAbility.abilityType.PROVOCATION);
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



    public void GetDamage(int damage)
    {
        if (damage > 0)
        {
            if (Abilities.Exists(x => x.AbilityType == CardAbility.abilityType.HOLY_SHIELD))
            {
                Debug.Log("Shield is found");
                Abilities.RemoveAll(x => x.AbilityType == CardAbility.abilityType.HOLY_SHIELD);
            }
            else
            {
                Defense -= damage;
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
        DOUBLE_ATTACK_ON_ALLY_CARD
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
        CardManager.AllCards.Add(new Card("peasant", "Sprites/Cards/peasant", 1, 4, 0, 2, new List<CardAbility> { new CardAbility(CardAbility.abilityType.GOLD_REGENERATION, 1) }));
        CardManager.AllCards.Add(new Card("peasant archer", "Sprites/Cards/archer-peasant", 1, 4, 0, 3, new List<CardAbility> { new CardAbility(CardAbility.abilityType.RANGED), new CardAbility(CardAbility.abilityType.GOLD_REGENERATION, 1) }));
        CardManager.AllCards.Add(new Card("witch", "Sprites/Cards/witch", 2, 5, 0, 3, new List<CardAbility> { new CardAbility(CardAbility.abilityType.MANA_ON_CAST, 2), new CardAbility(CardAbility.abilityType.RANGED) }));
        CardManager.AllCards.Add(new Card("scarecrow", "Sprites/Cards/scarecrow", 0, 6, 0, 1, new List<CardAbility> { new CardAbility(CardAbility.abilityType.PROVOCATION) }));

        CardManager.AllCards.Add(new Card("hunter archer", "Sprites/Cards/archer-hunter", 2, 5, 0, 5, new List<CardAbility> { new CardAbility(CardAbility.abilityType.RANGED), new CardAbility(CardAbility.abilityType.INSTANT_ACTIVE) }));
        CardManager.AllCards.Add(new Card("lini hunter", "Sprites/Cards/lini-hunter", 3, 7, 0, 6, new List<CardAbility> { new CardAbility(CardAbility.abilityType.INSTANT_ACTIVE), new CardAbility(CardAbility.abilityType.DAMAGE_HERO_ON_CAST, 2) }));

        CardManager.AllCards.Add(new Card("assasin archer", "Sprites/Cards/archer-assasin", 5, 9, 0, 10, new List<CardAbility> { new CardAbility(CardAbility.abilityType.RANGED) }));
        CardManager.AllCards.Add(new Card("guild assasin", "Sprites/Cards/guild-assasin", 4, 2, 0, 6, new List<CardAbility> { new CardAbility(CardAbility.abilityType.INSTANT_ACTIVE), new CardAbility(CardAbility.abilityType.DAMAGE_HERO_ON_CAST, 3) }));

        CardManager.AllCards.Add(new Card("wolf", "Sprites/Cards/wolf", 3, 3, 0, 3, new List<CardAbility> { new CardAbility(CardAbility.abilityType.INSTANT_ACTIVE), new CardAbility(CardAbility.abilityType.REGENERATION_EACH_TURN, 1) }));
        CardManager.AllCards.Add(new Card("alfa wolf", "Sprites/Cards/alfa-wolf", 4, 8, 0, 9, new List<CardAbility> { new CardAbility(CardAbility.abilityType.INSTANT_ACTIVE), new CardAbility(CardAbility.abilityType.PROVOCATION, 1), new CardAbility(CardAbility.abilityType.LIFESTEAL, 2) }));
        CardManager.AllCards.Add(new Card("feral wolf", "Sprites/Cards/feral-wolf", 3, 1, 1, 0, new List<CardAbility> { new CardAbility(CardAbility.abilityType.INSTANT_ACTIVE) }));
        CardManager.AllCards.Add(new Card("giant rat", "Sprites/Cards/giant-rat", 2, 5, 0, 3, new List<CardAbility> { new CardAbility(CardAbility.abilityType.REGENERATION_EACH_TURN, 3) }));


        CardManager.AllCards.Add(new Card("warden", "Sprites/Cards/warden", 4, 6, 0, 5, new List<CardAbility> { new CardAbility(CardAbility.abilityType.REGENERATION_EACH_TURN, 1) }));
        CardManager.AllCards.Add(new Card("soldier archer", "Sprites/Cards/archer-soldier", 3, 6, 0, 5, new List<CardAbility> { new CardAbility(CardAbility.abilityType.RANGED) }));
        CardManager.AllCards.Add(new Card("general scarlett", "Sprites/Cards/scarlet-general", 9, 13, 0, 18, new List<CardAbility> { new CardAbility(CardAbility.abilityType.DECREASE_ATTACK_ON_ATTACK, 3) }));
        CardManager.AllCards.Add(new Card("guard commander", "Sprites/Cards/guard-commander", 7, 11, 0, 12, new List<CardAbility> { new CardAbility(CardAbility.abilityType.DECREASE_ATTACK_ON_ATTACK) }));

        CardManager.AllCards.Add(new Card("gnome warrior", "Sprites/Cards/gnome-warrior", 4, 18, 0, 13, new List<CardAbility> { new CardAbility(CardAbility.abilityType.REGENERATION_EACH_TURN, 4) }));
        CardManager.AllCards.Add(new Card("gnome healer", "Sprites/Cards/gnome-healer", 1, 8, 0, 7, new List<CardAbility> { new CardAbility(CardAbility.abilityType.REGENERATION_EACH_TURN, 1), new CardAbility(CardAbility.abilityType.HEAL_ALLY_FIELD_CARDS_EACH_TURN, 2) }));
        CardManager.AllCards.Add(new Card("forest gnome", "Sprites/Cards/forest-gnome", 1, 5, 0, 4, new List<CardAbility> { new CardAbility(CardAbility.abilityType.HEAL_ALLY_FIELD_CARDS_ON_CAST, 2) }));
        CardManager.AllCards.Add(new Card("clay golem", "Sprites/Cards/clay-golem", 3, 12, 0, 7, new List<CardAbility> { new CardAbility(CardAbility.abilityType.REGENERATION_EACH_TURN, 2) }));

        CardManager.AllCards.Add(new Card("beholder", "Sprites/Cards/beholder", 5, 17, 3, 10, new List<CardAbility> { new CardAbility(CardAbility.abilityType.LIFESTEAL, 3), new CardAbility(CardAbility.abilityType.DAMAGE_ON_CAST, 3) }));
        CardManager.AllCards.Add(new Card("ancient keeper", "Sprites/Cards/ancient-keeper", 3, 17, 5, 12, new List<CardAbility> { new CardAbility(CardAbility.abilityType.HERO_REGENERATION, 2), new CardAbility(CardAbility.abilityType.MANA_REGENERATION, 2) }));

        CardManager.AllCards.Add(new Card("crusader", "Sprites/Cards/crusader", 4, 8, 1, 7, new List<CardAbility> { new CardAbility(CardAbility.abilityType.HOLY_SHIELD) }));
        CardManager.AllCards.Add(new Card("divine mistress", "Sprites/Cards/divine-mistress", 5, 15, 3, 12, new List<CardAbility> { new CardAbility(CardAbility.abilityType.HOLY_SHIELD), new CardAbility(CardAbility.abilityType.REGENERATION_EACH_TURN, 1), new CardAbility(CardAbility.abilityType.DAMAGE_EACH_TURN, 1) }));
        CardManager.AllCards.Add(new Card("crusader commander", "Sprites/Cards/crusader-commander", 6, 12, 2, 9, new List<CardAbility> { new CardAbility(CardAbility.abilityType.HOLY_SHIELD), new CardAbility(CardAbility.abilityType.REGENERATION_EACH_TURN, 1) }));
        CardManager.AllCards.Add(new Card("crusader general", "Sprites/Cards/crusader-general", 6, 20, 4, 18, new List<CardAbility> { new CardAbility(CardAbility.abilityType.HOLY_SHIELD), new CardAbility(CardAbility.abilityType.REGENERATION_EACH_TURN, 3), new CardAbility(CardAbility.abilityType.PROVOCATION), new CardAbility(CardAbility.abilityType.DECREASE_ATTACK_ON_ATTACK) }));
        // CardManager.AllCards.Add(new Card("wild werewolf", "Sprites/Cards/wild-werewolf", 4, 7, 0, 5, new List<CardAbility> {new CardAbility(CardAbility.abilityType.INSTANT_ACTIVE, new CardAbility(CardAbility.abilityType.REGENERATION_EACH_TURN, new CardAbility(CardAbility.abilityType.DECREASE_ATTACK_ON_ATTACK}));


        // SPELLS    

        CardManager.AllCards.Add(new SpellCard("fireball", "Sprites/Cards/fireball", 2, 0, SpellCard.SpellType.DAMAGE_CARD, 7, SpellCard.TargetType.ENEMY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("magic missle", "Sprites/Cards/magic-missle", 1, 0, SpellCard.SpellType.DAMAGE_CARD, 4, SpellCard.TargetType.ENEMY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("firewall", "Sprites/Cards/firewall", 5, 0, SpellCard.SpellType.DAMAGE_ENEMY_FIELD_CARDS, 5, SpellCard.TargetType.NO_TARGET));
        CardManager.AllCards.Add(new SpellCard("healing wave", "Sprites/Cards/healing-wave 1", 2, 0, SpellCard.SpellType.HEAL_ALLY_FIELD_CARDS, 3, SpellCard.TargetType.NO_TARGET));
        CardManager.AllCards.Add(new SpellCard("mana potion", "Sprites/Cards/mana-potion", 0, 1, SpellCard.SpellType.ADD_MANA, 4, SpellCard.TargetType.NO_TARGET));
        CardManager.AllCards.Add(new SpellCard("lighting bolt", "Sprites/Cards/lightning", 7, 0, SpellCard.SpellType.DAMAGE_ENEMY_HERO, 5, SpellCard.TargetType.NO_TARGET));
        CardManager.AllCards.Add(new SpellCard("healing potion", "Sprites/Cards/healing-potion", 0, 1, SpellCard.SpellType.HEAL_ALLY_HERO, 4, SpellCard.TargetType.NO_TARGET));
        CardManager.AllCards.Add(new SpellCard("healing spell", "Sprites/Cards/healing-spell", 2, 0, SpellCard.SpellType.HEAL_CARD, 6, SpellCard.TargetType.ALLY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("decay", "Sprites/Cards/low_damage", 3, 0, SpellCard.SpellType.DEBUFF_CARD_DAMAGE, 4, SpellCard.TargetType.ENEMY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("provocation", "Sprites/Cards/provocation-spell", 4, 0, SpellCard.SpellType.PROVOCATION_ON_ALLY_CARD, 0, SpellCard.TargetType.ALLY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("damage buff", "Sprites/Cards/damage-buff-spell", 2, 0, SpellCard.SpellType.BUFF_CARD_DAMAGE, 2, SpellCard.TargetType.ALLY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("magic shield", "Sprites/Cards/magic-shield", 1, 0, SpellCard.SpellType.SHIELD_ON_ALLY_CARD, 2, SpellCard.TargetType.ALLY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("double attack", "Sprites/Cards/double-attack", 4, 0, SpellCard.SpellType.DOUBLE_ATTACK_ON_ALLY_CARD, 0, SpellCard.TargetType.ALLY_CARD_TARGET));


    }

}
