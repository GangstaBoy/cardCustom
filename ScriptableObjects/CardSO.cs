using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardSO : ScriptableObject
{
    public enum CardType
    {
        CREATURE,
        SPELL
    }
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private int _attack, _manacost, _goldcost, _maxHealth;
    [SerializeField] private Sprite _artwork;
    [SerializeField] private List<CardAbilitySO> _abilities;
    [SerializeField] private CardType _type;


    public string Name { get => _name; }
    public string Description { get => _description; }
    public int Attack { get => _attack; }
    public int Manacost { get => _manacost; }
    public int Goldcost { get => _goldcost; }
    public int MaxHealth { get => _maxHealth; }
    public List<CardAbilitySO> Abilities { get => _abilities; }
    public Sprite Artwork { get => _artwork; }
    public CardType Type { get => _type; }
}
