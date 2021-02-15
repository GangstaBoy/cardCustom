using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBehavior : MonoBehaviour
{
    private List<CardSO> _deck = new List<CardSO>();

    private void RandomizeDeck()
    {
        _deck.Add(Resources.Load<CardSO>("CardObjects/Peasant"));
        _deck.Add(Resources.Load<CardSO>("CardObjects/Wolf"));
        _deck.Add(Resources.Load<CardSO>("CardObjects/Magic Sparks"));
    }

    private void Awake()
    {
        RandomizeDeck();
    }

#nullable enable
    public CardSO? DrawCard()
    {

        if (_deck.Count == 0) return null;
        var card = _deck[0];
        _deck.RemoveAt(0);
        return card;
    }
#nullable disable
}
