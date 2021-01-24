using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckController : MonoBehaviour, IPointerClickHandler
{
    public bool isPlayerDeck;
    public List<Card> Cards = new List<Card>();

    public void customDeck()
    {
        SpellCard spellCard = (SpellCard)CardManager.AllCards.Find(x => x.Name == "mana potion");
        Card card = CardManager.AllCards.Find(x => x.Name == "guild assasin");
                if (spellCard.IsSpell) Cards.Add(((SpellCard)spellCard).GetCopy());
                else Cards.Add(spellCard.GetCopy());
        for(int i = 0; i<5; i++)
        {
                if (card.IsSpell) Cards.Add(((SpellCard)card).GetCopy());
                else Cards.Add(card.GetCopy());
        }
        
    }

    public void RandomizeDeck(int size)
    {

        for(int i = 0; i<size; i++)
        {
            var card = CardManager.AllCards[Random.Range(0, CardManager.AllCards.Count)];
            if (card.IsSpell) Cards.Add(((SpellCard)card).GetCopy());
            else Cards.Add(card.GetCopy());
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(isPlayerDeck) 
        {
            Debug.Log("Yey! Size is " + Cards.Count);
            for (int i = 0; i < Cards.Count; i++)
            {
                Debug.Log(Cards[i].Name);
            }
        }
    }

    public void GiveCardToHand(Transform hand, int count = 1)
    {
        if (count < 1) return;
        for (int i = 0; i < count; i++)
        {
            if(Cards.Count == 0)
            return;
        
        GameManagerScr.Instance.CreateCardPref(Cards[0], hand);
        Debug.Log("Card " + Cards[0].Name + " to " + hand );

        Cards.RemoveAt(0);
        }
    }
}