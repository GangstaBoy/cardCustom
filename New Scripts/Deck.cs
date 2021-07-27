using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private List<GameCardSO> _deck = new List<GameCardSO>();
    private GameCardSO[] _cards;



    private void Awake()
    {
        _cards = Resources.LoadAll<GameCardSO>("GameCardObjects");
        RandomizeDeck();
    }
    public void RandomizeDeck()
    {
        for (int i = 0; i < 10; i++)
        {
            _deck.Add(_cards[Random.Range(0, _cards.Length)]);
        }
    }

#nullable enable
    public GameCardSO? DrawCard()
    {

        if (_deck.Count == 0) return null;
        var card = _deck[0];
        _deck.RemoveAt(0);
        return card;
    }
#nullable disable

}
