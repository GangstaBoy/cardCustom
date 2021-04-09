using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehaviour : MonoBehaviour
{
    [SerializeField] private int _attack, _maxHealth, _health;
    [SerializeField] private List<CardAbilitySO> _abilities;
    [SerializeField] private CardSO.CardType _type;

    public int Attack { get => _attack; }
    public int MaxHealth { get => _maxHealth; }
    public int Health { get => _health; }
    public List<CardAbilitySO> Abilities { get => _abilities; }
    public CardSO.CardType Type { get => _type; }
    private bool _isPlayable;
    private bool _isPlayed;

    public CardBehaviour(CardSO cardSO)
    {
        this._attack = cardSO.Attack;
        this._health = this._maxHealth = cardSO.MaxHealth;

    }



    void Play()
    {
        if (!_isPlayable) return;
    }


    void Start()
    {

    }

}
