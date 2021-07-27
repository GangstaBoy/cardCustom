using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private List<CardInstance> _hand = new List<CardInstance>();
    [SerializeField] private int _maxHandSize;
    public int currentHandSize { get => _hand.Count; }
    public int maxHandSize { get => _maxHandSize; }
    public bool canDraw { get => maxHandSize > currentHandSize; }

    public void AddCard(CardInstance card)
    {
        _hand.Add(card);
    }

    public void RemoveCard(CardInstance card)
    {
        _hand.Remove(card);
        Debug.Log("Destroying " + card.name);
        Destroy(card.gameObject);
    }

}
