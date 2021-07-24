using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class Moveable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Camera MainCamera;
    Vector3 offset; // stores distance between center and click place
    public Transform DefaultParent, DefaultTempCardParent;
    GameObject TempCardGO;
    public bool IsDraggable { get => _isDraggable; }
    public int initialHandIndex { get => _initialHandIndex; }
    private int _initialHandIndex;
    private bool _isDraggable;
    private Transform _defaultParent;
    [SerializeField] private CardNew _cardNew;


    void Awake()
    {
        _defaultParent = transform.parent;
        MainCamera = Camera.allCameras[0]; // todo: fix
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = transform.position - MainCamera.ScreenToWorldPoint(eventData.position);
        _initialHandIndex = transform.GetSiblingIndex();
        transform.SetParent(_defaultParent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPos = MainCamera.ScreenToWorldPoint(eventData.position);
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.position = newPos + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_defaultParent);
        transform.SetSiblingIndex(_initialHandIndex);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        //transform.SetSiblingIndex(TempCardGO.transform.GetSiblingIndex());

    }
}
