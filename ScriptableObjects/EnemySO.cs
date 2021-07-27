using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemySO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _artwork;
    [SerializeField] private string _description;
    [SerializeField] private int _health, _mana, _stamina, _cardsDrawPerTurn;

    public string Name { get => _name; }
    public Sprite Artwork { get => _artwork; }
    public string Description { get => _description; }
    public int Health { get => _health; }
    public int Mana { get => _mana; }
    public int Stamina { get => _stamina; }
    public int CardsDrawPerTurn { get => _cardsDrawPerTurn; }
}
