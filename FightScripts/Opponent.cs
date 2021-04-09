using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    private List<CardNew> _hand = new List<CardNew>();
    [SerializeField] private Transform _handTransform;
    [SerializeField] private Transform _fieldTransform;
    [SerializeField] private DeckBehavior _deck;
    [SerializeField] private GameObject cardPrefTemplate;
    private bool isPlayer;

    public Transform FieldTransform { get => _fieldTransform; }
    public bool IsPlayer { get => isPlayer; }

#nullable enable
    public void TakeCard(DeckBehavior deck)
    {
        CardSO? cardSO = deck.DrawCard();
        if (cardSO == null) return;
        var card = Object.Instantiate(cardPrefTemplate, _handTransform).GetComponent<CardNew>();
        card.Initialize(cardSO, this);
        _hand.Add(card);
    }
#nullable disable
    public void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            TakeCard(_deck);
        }
        foreach (var item in _hand)
        {
            item.CardDisplay.ShowCard();
        }
    }
}
