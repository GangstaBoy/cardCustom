using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buff
{
    public string Name;
    public BuffType BuffType;
    public int? BuffValue;
    public string LogoPath;
    public BuffCategory BuffCategory;
    public bool AttackModifier;


    public Buff(string name, string logo, BuffType buffType, int? buffValue = null, BuffCategory buffCategory = BuffCategory.BUFF, bool attackModifier = false)
    {
        AttackModifier = attackModifier;
        Name = name;
        LogoPath = logo;
        BuffType = buffType;
        BuffCategory = buffCategory;
        if (buffValue != null) BuffValue = buffValue;
    }

    public Buff(Buff buff)
    {
        AttackModifier = buff.AttackModifier;
        Name = buff.Name;
        LogoPath = buff.LogoPath;
        BuffType = buff.BuffType;
        BuffCategory = buff.BuffCategory;
        if (buff.BuffValue != null) BuffValue = buff.BuffValue;
    }

    public Buff GetCopy()
    {
        return new Buff(this);
    }
}

public enum BuffCategory
{
    BUFF,
    DEBUFF,
    HERO_BUFF,
    HERO_DEBUFF
}

public enum BuffType
{
    ARMOR,
    DAMAGE_INCREASE,
    MAX_DEFENSE_INCREASE,
    HOLY_SHIELD,
    PROVOCATION,
    DOUBLE_ATTACK,
    SELF_HP_REGENERATION,
    NEARBY_ALLIES_REGENERATION,
    ALL_ALLIES_REGENERATION,
    FIRE_SHIELD,
    ARMOR_PACKAGE,
    SKELETON_SUMMONER,
    MANA_FLARE,
    GOLD_INCOME,
    BUFF_DAMAGE,
    WOLF_AURA,
    WOLF_AURA_CAST
}


public static class BuffsManager
{
    public static List<Buff> AllBuffs = new List<Buff>();
    public static void ShowAll()
    {
        foreach (var item in AllBuffs)
        {
            Debug.Log(item.Name);
        }
    }
    public static Buff GetBuff(string name)
    {
        foreach (var item in AllBuffs)
        {
            if (item.Name == name)
            {
                Debug.Log(item.Name);
                return item.GetCopy();
            }
        }
        Debug.LogError("No such buff found:" + name);
        return null;
    }
}




public class BuffsManagerStarter : MonoBehaviour
{
    void Awake()
    {
        //BuffsManager.AllBuffs.Add(new Buff("fire arms", "Sprites/Cards/double-attack"));
        BuffsManager.AllBuffs.Add(new Buff("magic shield", "Sprites/Cards/magic-shield", BuffType.HOLY_SHIELD, 1));
        BuffsManager.AllBuffs.Add(new Buff("provocation", "Sprites/Cards/provocation-spell", BuffType.PROVOCATION));
        BuffsManager.AllBuffs.Add(new Buff("double attack", "Sprites/Cards/double-attack", BuffType.DOUBLE_ATTACK));
        BuffsManager.AllBuffs.Add(new Buff("armor", "Sprites/Cards/armor", BuffType.ARMOR));
        BuffsManager.AllBuffs.Add(new Buff("regeneration", "Sprites/Cards/regeneration", BuffType.SELF_HP_REGENERATION));
        BuffsManager.AllBuffs.Add(new Buff("healing aura", "Sprites/Cards/heal-allies-buff", BuffType.ALL_ALLIES_REGENERATION));
        BuffsManager.AllBuffs.Add(new Buff("fire shield", "Sprites/Cards/fire-shield", BuffType.FIRE_SHIELD));
        BuffsManager.AllBuffs.Add(new Buff("buff damage", "Sprites/Cards/damage-buff-spell", BuffType.BUFF_DAMAGE, 1, BuffCategory.BUFF, true));
        BuffsManager.AllBuffs.Add(new Buff("wolf aura", "Sprites/Cards/wolf", BuffType.WOLF_AURA, 1, BuffCategory.BUFF));


        BuffsManager.AllBuffs.Add(new Buff("armor package", "Sprites/Cards/armor", BuffType.ARMOR_PACKAGE, 4, BuffCategory.HERO_BUFF));
        BuffsManager.AllBuffs.Add(new Buff("skeleton summoner", "Sprites/Cards/skeleton-summoner", BuffType.SKELETON_SUMMONER, 1, BuffCategory.HERO_BUFF));
        BuffsManager.AllBuffs.Add(new Buff("mana flare", "Sprites/Cards/mana-flare", BuffType.MANA_FLARE, 2, BuffCategory.HERO_BUFF));
        BuffsManager.AllBuffs.Add(new Buff("gold income", "Sprites/Cards/gold-income", BuffType.GOLD_INCOME, 3, BuffCategory.HERO_BUFF));

    }
}




