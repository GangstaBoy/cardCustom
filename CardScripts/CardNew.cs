using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class CardNew : MonoBehaviour, IPointerClickHandler
{
    public Opponent Owner { get => _player; }
    public CardDisplay CardDisplay { get => _cardDisplay; }
    public DropPlaceScriptNew Drop;
    public CardMovement Movement { get => _movement; }
    public bool IsPlayable { get => _isPlayable; }
    private CardSO _cardSO;
    private Opponent _player;
    [SerializeField] private CardDisplay _cardDisplay;
    [SerializeField] private CardMovement _movement;
    private bool _isPlayable = true;


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
        CardDisplay.Initialize(_cardSO);
    }

    private void OnDrop(object sender, DropPlaceScriptNew.DropEventArgs e)
    {
        if (e.dropObject == this.gameObject && IsPlayable && e.fieldType == FieldType.SELF_FIELD)
        {
            this.Movement.DefaultParent = e.dropField.transform;
            Play();
        }
    }

    public void Play()
    {
        if (true)
        {
            Drop.CardDropped -= OnDrop;
            //Destroy(gameObject, 0.01f);
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {

        Play();
    }
}


