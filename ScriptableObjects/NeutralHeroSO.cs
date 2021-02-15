using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Neutral", menuName = "Neutral")]
public class NeutralHeroSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _artwork;
    [SerializeField] private int _maxHealth;
    [SerializeField] private List<CardSO> _deck;

    public string Name { get => _name; }
    public string Description { get => _description; }
    public Sprite Artwork { get => _artwork; }
    public int MaxHealth { get => _maxHealth; }
    public List<CardSO> Deck { get => _deck; }

}
