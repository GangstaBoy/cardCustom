using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero", menuName = "Hero")]
public class HeroSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _artwork;

    public string Name { get => _name; }
    public string Description { get => _description; }
    public Sprite Artwork { get => _artwork; }
}
