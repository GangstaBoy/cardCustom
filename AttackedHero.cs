using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackedHero : MonoBehaviour, IDropHandler
{
    public enum HeroType
    {
        ENEMY,
        PLAYER
    }
    public Image HeroIcon;
    public HeroType Type;
    public GameManagerScr GameManager;
    public List<Sprite> Icons;
    public Color TargetCol, NormalCol;

    void Start() 
    {
        HeroIcon.sprite = Icons[Random.Range(0, Icons.Count)];
        HeroIcon.preserveAspect = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(!GameManagerScr.Instance.IsPlayerTurn) return;

        CardController card = eventData.pointerDrag.GetComponent<CardController>();

        if(GameManagerScr.Instance.EnemyFieldCards.Count > 0 && !card.Card.Ranged) return;

        if (card && card.Info.CanAttack && Type == HeroType.ENEMY 
        && (!GameManagerScr.Instance.EnemyFieldCards.Exists(x => x.Card.IsProvocation)))
        {
            GameManagerScr.Instance.DamageHero(card, true);
        }

    }

    public void HighlightAsTarget (bool highlight)
    {
        GetComponent<Image>().color = highlight ? TargetCol : NormalCol;
    }
}
