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

    public Buff(string name, string logo, BuffType buffType, int? buffValue = null)
    {
        Name = name;
        LogoPath = logo;
        BuffType = buffType;
        if (buffValue != null) BuffValue = buffValue;
    }

    public Buff(Buff buff)
    {
        Name = buff.Name;
        LogoPath = buff.LogoPath;
        BuffType = buff.BuffType;
        if (buff.BuffValue != null) BuffValue = buff.BuffValue;
    }

    public Buff GetCopy()
    {
        return new Buff(this);
    }
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
    ALL_ALLIES_REGENERATION
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
    }
}




