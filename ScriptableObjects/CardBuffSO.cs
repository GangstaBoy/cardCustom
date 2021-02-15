using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Card Buff", menuName = "Card Buff")]
public class CardBuffSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private BuffCategory _buffCategory;
    [SerializeField] private BuffType _buffType;
    [SerializeField] private int _buffValue;
    [SerializeField] private Sprite _artwork;

    public string Name { get => _name; }
    public string Description { get => _description; }
    public BuffCategory BuffCategory { get => _buffCategory; }
    public BuffType BuffType { get => _buffType; }
    public Sprite Artwork { get => _artwork; }
    public int BuffValue { get => _buffValue; }
}
