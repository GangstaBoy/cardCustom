using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CardEffectType
{
    DEAL_DAMAGE,
    RESTORE_HEALTH_INSTANT,
    MANA_BURN,
    REDUCE_LIFE
}
[System.Serializable]
public class EffectValues
{
    public string key;
    public string value;
}


