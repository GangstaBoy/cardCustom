using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Effect", menuName = "Effect")]
public class CardEffectSO : ScriptableObject
{
    [SerializeField] private CardEffectType _cardEffectType;
    [SerializeField] private EffectValues[] _effectValues;

    public CardEffectType CardEffectType { get => _cardEffectType; }
    public EffectValues[] EffectValues { get => _effectValues; }
}