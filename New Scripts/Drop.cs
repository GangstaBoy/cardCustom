using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Drop : MonoBehaviour, IDropHandler
{
    public class DropArgs : EventArgs
    {
        public GameObject dropObject;
    }
    public static event EventHandler<DropArgs> CardDropEvent;

    public void OnDrop(PointerEventData eventData)
    {
        CardDropEvent?.Invoke(this.gameObject, new DropArgs { dropObject = eventData.pointerDrag });
    }
}
