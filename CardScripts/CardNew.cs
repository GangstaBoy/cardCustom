using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ICard
{
    bool isPlayable { get; }
    void Play();
}

public class CardNew : MonoBehaviour, IPointerClickHandler
{
    public Opponent Owner { get => _player; }
    private CardSO _cardSO;
    private Opponent _player;
    [SerializeField] private CardDisplay _cardDisplay;
    private ICard _card;


    public void Initialize(CardSO card, Opponent player)
    {
        _cardSO = card;
        _player = player;
        _cardDisplay.Initialize(_cardSO);
        if (_cardSO.Type == CardSO.CardType.CREATURE) _card = new CreatureCard(this);
        if (_cardSO.Type == CardSO.CardType.SPELL) _card = new SpellCardNew();
    }

    public void ShowCard()
    {
        _cardDisplay.ShowCard();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_card.isPlayable)
        {
            _card.Play();
            Destroy(gameObject, 0.1f);
        }
    }
}


