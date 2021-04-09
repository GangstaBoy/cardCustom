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
    public class DropEventArgs : EventArgs
    {
        public GameObject dropObject;
        public GameObject dropField;
        public FieldType fieldType;
    }
    public event EventHandler<DropEventArgs> CardDropped;
    public event EventHandler<DropEventArgs> CardEntered;
    public event EventHandler<DropEventArgs> CardLeft;
    public FieldType type;
    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggable = eventData.pointerDrag;
        CardDropped?.Invoke(this.gameObject, new DropEventArgs { dropObject = draggable, dropField = this.gameObject, fieldType = this.type });
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        GameObject draggable = eventData.pointerDrag;
        CardEntered?.Invoke(this.gameObject, new DropEventArgs { dropObject = draggable, dropField = this.gameObject, fieldType = this.type });
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        GameObject draggable = eventData.pointerDrag;
        CardLeft?.Invoke(this.gameObject, new DropEventArgs { dropObject = draggable, dropField = this.gameObject, fieldType = this.type });
    }
}
