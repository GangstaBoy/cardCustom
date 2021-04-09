using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class CardMovement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public DropPlaceScriptNew Drop;
    Camera MainCamera;
    Vector3 offset; // stores distance between card center and click place
    public Transform DefaultParent, DefaultTempCardParent;
    GameObject TempCardGO;
    public bool IsDraggable { get => _isDraggable; }
    public int initialHandIndex { get => _initialHandIndex; }
    private int _initialHandIndex;
    private bool _isDraggable;
    [SerializeField] private CardNew _cardNew;


    void Awake()
    {
        MainCamera = Camera.allCameras[0]; // todo: fix
        TempCardGO = GameObject.Find("TempCardGO");
    }

    void Start()
    {
        CheckIfDraggable();
        Drop = MainCamera.GetComponent<Opponent>().FieldTransform.GetComponent<DropPlaceScriptNew>();
        Drop.CardEntered += OnEnter;
        Drop.CardLeft += OnLeave;
    }

    void OnEnter(object sender, DropPlaceScriptNew.DropEventArgs e)
    {
        if (e.fieldType != FieldType.SELF_FIELD)
            return;
        if (e.dropObject)
            DefaultTempCardParent = e.dropField.transform;
    }
    void OnLeave(object sender, DropPlaceScriptNew.DropEventArgs e)
    {
        if (e.fieldType != FieldType.SELF_FIELD)
            return;
        if (e.dropObject && DefaultTempCardParent == e.dropField.transform)
            DefaultTempCardParent = DefaultParent;
    }

    void Update()
    {
        //CheckIfDraggable();
    }

    void CheckIfDraggable()
    {
        _isDraggable = _cardNew.IsPlayable;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        CheckIfDraggable();

        if (!IsDraggable)
            return;

        offset = transform.position - MainCamera.ScreenToWorldPoint(eventData.position);
        DefaultParent = DefaultTempCardParent = transform.parent;

        _initialHandIndex = transform.GetSiblingIndex();

        //if (CC.Card.IsSpell || CC.Info.CanAttack)
        //    GameManagerScr.Instance.HighlightTargets(true, CC.Card);

        TempCardGO.transform.SetParent(DefaultParent);
        TempCardGO.transform.SetSiblingIndex(transform.GetSiblingIndex());


        transform.SetParent(DefaultParent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        CheckIfDraggable();
        if (!IsDraggable)
            return;

        Vector3 newPos = MainCamera.ScreenToWorldPoint(eventData.position);
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.position = newPos + offset;

        if (TempCardGO.transform.parent != DefaultTempCardParent)
        {
            TempCardGO.transform.SetParent(DefaultTempCardParent);
            if (DefaultTempCardParent.GetComponent<DropPlaceScriptNew>().type == FieldType.SELF_FIELD)
                TempCardGO.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        if (DefaultParent.GetComponent<DropPlaceScriptNew>().type != FieldType.SELF_FIELD)
            CheckPoisition();

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsDraggable)
            return;
        transform.SetParent(DefaultParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        transform.SetSiblingIndex(TempCardGO.transform.GetSiblingIndex());

        TempCardGO.transform.SetParent(GameObject.Find("Canvas").transform);
        TempCardGO.transform.localPosition = new Vector2(2340, 0);

        //GameManagerScr.Instance.HighlightTargets(false, CC.Card);
    }

    void CheckPoisition()
    {
        int NewIndex = DefaultTempCardParent.childCount;
        if (TempCardGO.transform.parent == DefaultParent) NewIndex = _initialHandIndex;
        else
            for (int i = 0; i < DefaultTempCardParent.childCount; i++)
            {
                if (transform.position.x < DefaultTempCardParent.GetChild(i).position.x)
                {
                    NewIndex = i;
                    /*
                    if (TempCardGO.transform.GetSiblingIndex() < NewIndex)
                        NewIndex--;
                    */
                    break;
                }
            }
        TempCardGO.transform.SetSiblingIndex(NewIndex);
    }

    public void MoveToField(Transform field)
    {
        transform.SetParent(GameObject.Find("Canvas").transform);
        transform.DOMove(field.position, .5f);
    }

    public void MoveToTarget(Transform target, bool returnRequired = true)
    {
        StartCoroutine(MoveToTargetCor(target, returnRequired));
    }

    IEnumerator MoveToTargetCor(Transform target, bool returnRequired = true)
    {
        Transform parent = transform.parent;
        int index = transform.GetSiblingIndex();
        Vector3 pos = transform.position;
        /*
        if(transform.parent.GetComponent<HorizontalLayoutGroup>())
            transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = false;
        */
        transform.SetParent(GameObject.Find("Canvas").transform);

        transform.DOMove(target.position, .25f);

        yield return new WaitForSeconds(.26f);
        if (transform.GetComponent<RectTransform>() != null && returnRequired)
            transform.DOMove(pos, .25f);

        yield return new WaitForSeconds(.26f);
        if (transform.GetComponent<RectTransform>() != null && returnRequired)
        {
            transform.SetParent(parent);
            transform.SetSiblingIndex(index);
        }

        /*
        if(transform.parent.GetComponent<HorizontalLayoutGroup>())
            transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = true;
        */

    }

}
