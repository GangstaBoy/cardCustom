using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
public enum FieldType
{
    SELF_HAND,
    SELF_FIELD,
    ENEMY_HAND,
    ENEMY_FIELD
}
*/
public class DropPlaceScriptNew : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public class CardDroppedEventArgs : EventArgs
    {
        public GameObject droppedObject;
    }
    public event EventHandler<CardDroppedEventArgs> CardDropped;
    public FieldType type;
    private int _rotation;
    public void OnDrop(PointerEventData eventData)
    {
        if (type != FieldType.SELF_FIELD)
            return;

        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();
        card.DefaultParent = transform;
        CardDropped?.Invoke(this.gameObject, new CardDroppedEventArgs { droppedObject = card.gameObject });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || type == FieldType.ENEMY_HAND || type == FieldType.ENEMY_FIELD || type == FieldType.SELF_HAND)
            return;

        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();

        if (card)
            card.DefaultTempCardParent = transform;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || type == FieldType.ENEMY_HAND || type == FieldType.ENEMY_FIELD || type == FieldType.SELF_HAND)
            return;

        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();

        if (card && card.DefaultTempCardParent == transform)
            card.DefaultTempCardParent = card.DefaultParent;

    }
}
