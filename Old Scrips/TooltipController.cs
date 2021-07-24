using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TooltipController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltipPrefab;
    GameObject tooltip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip = GameObject.Instantiate(tooltipPrefab, this.transform.Find("TooltipPlaceholder"), false);
        TMP_Text description = tooltip.transform.Find("TooltipText").GetComponent<TMP_Text>();
        BuffBehaviour buffBehavior = this.gameObject.GetComponent<BuffBehaviour>();     // todo: make generic
        description.text = buffBehavior.BuffDescription;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
        GameObject.Destroy(tooltip.gameObject);
    }
}
