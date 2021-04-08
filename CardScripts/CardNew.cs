using System;
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
    public DropPlaceScriptNew Drop;

    void Start()
    {
        Camera MainCamera = Camera.allCameras[0];       //todo: fix this shit! Perhaps make a single Event System? 
        Drop = MainCamera.GetComponent<Opponent>().FieldTransform.GetComponent<DropPlaceScriptNew>();
        Drop.CardDropped += OnDrop;
    }

    public void Initialize(CardSO card, Opponent player)
    {
        _cardSO = card;
        _player = player;
        _cardDisplay.Initialize(_cardSO);
        if (_cardSO.Type == CardSO.CardType.CREATURE) _card = new CreatureCard(this);
        if (_cardSO.Type == CardSO.CardType.SPELL) _card = new SpellCardNew();
    }

    private void OnDrop(object sender, DropPlaceScriptNew.CardDroppedEventArgs e)
    {
        if (e.droppedObject == this.gameObject)
        {
            Play();
        }
    }
    public void ShowCard()
    {
        _cardDisplay.ShowCard();
    }

    public void Play()
    {
        if (_card.isPlayable)
        {
            _card.Play();
            Drop.CardDropped -= OnDrop;
            Destroy(gameObject, 0.01f);
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Play();
    }
}


