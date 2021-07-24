using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Game Card", menuName = " Game Card")]
public class GameCardSO : ScriptableObject
{
    public enum GameCardType        //todo: move to a different location
    {
        ATTACK,
        SPELL,
        ACTION,
        ARTIFACT
    }
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private int _manacost, _staminacost;
    [SerializeField] private Sprite _artwork;
    [SerializeField] private GameCardType _type;
    [SerializeField] private CardEffectSO[] _cardEffectSOs;
    public string Name { get => _name; }
    public string Description { get => _description; }
    public int Manacost { get => _manacost; }
    public int Staminacost { get => _staminacost; }
    public Sprite Artwork { get => _artwork; }
    public GameCardType Type { get => _type; }
    public CardEffectSO[] CardEffectSOs { get => _cardEffectSOs; }
}
