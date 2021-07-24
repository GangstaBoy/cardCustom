using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum FieldType
{
    SELF_HAND,
    SELF_FIELD,
    ENEMY_HAND,
    ENEMY_FIELD
}

public class DropPlaceScript : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public FieldType type;
    public void OnDrop(PointerEventData eventData)
    {
        if (type != FieldType.SELF_FIELD)
            return;

        CardController card = eventData.pointerDrag.GetComponent<CardController>();

        if (card
        && GameManagerScr.Instance.IsPlayerTurn
        && GameManagerScr.Instance.CurrentGame.Player.Gold >= card.Card.Goldcost
        && GameManagerScr.Instance.CurrentGame.Player.Mana >= card.Card.Manacost
        && !card.Card.IsPlaced
        && card.Movement.IsDraggable)
        {
            if (card.Card.IsSpell)
            {
                card.OnCast();
            }
            else
            {
                card.Movement.DefaultParent = transform;
                card.OnCast();
            }
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || type == FieldType.ENEMY_HAND || type == FieldType.ENEMY_FIELD || type == FieldType.SELF_HAND)
            return;

        CardMovementScript card = eventData.pointerDrag.GetComponent<CardMovementScript>();

        if (card)
            card.DefaultTempCardParent = transform;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || type == FieldType.ENEMY_HAND || type == FieldType.ENEMY_FIELD || type == FieldType.SELF_HAND)
            return;

        CardMovementScript card = eventData.pointerDrag.GetComponent<CardMovementScript>();

        if (card && card.DefaultTempCardParent == transform)
            card.DefaultTempCardParent = card.DefaultParent;

    }
}
