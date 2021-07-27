using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Buff", menuName = "Buff")]
public class BuffSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private BuffCategory _buffCategory;
    [SerializeField] private BuffType _buffType;
    [SerializeField] private Sprite _artwork;

    public string Name { get => _name; }
    public string Description { get => _description; }
    public BuffCategory BuffCategory { get => _buffCategory; }
    public BuffType BuffType { get => _buffType; }
    public Sprite Artwork { get => _artwork; }
}
