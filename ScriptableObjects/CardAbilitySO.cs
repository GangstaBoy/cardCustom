using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Ability", menuName = "Ability")]
public class CardAbilitySO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private CardAbility.abilityType _abilityType;
    [SerializeField] private int _abilityValue = 0;
    [SerializeField] private Sprite _artwork;

    public string Name { get => _name; }
    public string Description { get => _description; }
    public CardAbility.abilityType AbilityType { get => _abilityType; }
    public int AbilityValue { get => _abilityValue; }
    public Sprite Artwork { get => _artwork; }
}
