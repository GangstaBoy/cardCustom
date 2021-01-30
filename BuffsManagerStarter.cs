using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buff : MonoBehaviour
{
    public string Name;
    //public List<KeyValuePair<BuffType, int>> BuffEffects;
    public string LogoPath;

    public Buff(string name/*, List<KeyValuePair<BuffType, int>> buffEffects*/, string logo)
    {
        Name = name;
        //BuffEffects = buffEffects;
        LogoPath = logo;
    }

    public Buff(Buff buff)
    {
        Name = buff.Name;
        //BuffEffects = buff.BuffEffects;
        LogoPath = buff.LogoPath;
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
    SELF_REGENERATION,
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
        BuffsManager.AllBuffs.Add(new Buff("fire arms"/*, new List<KeyValuePair<BuffType, int>> { new KeyValuePair<BuffType, int>(BuffType.DAMAGE_INCREASE, 5) }*/, "Sprites/Cards/double-attack"));
        BuffsManager.AllBuffs.Add(new Buff("magic shield"/*, new List<KeyValuePair<BuffType, int>> { new KeyValuePair<BuffType, int>(BuffType.DAMAGE_INCREASE, 5) }*/, "Sprites/Cards/magic-shield"));
    }
}




