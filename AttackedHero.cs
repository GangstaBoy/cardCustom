using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

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
        if (!GameManagerScr.Instance.IsPlayerTurn) return;

        CardController card = eventData.pointerDrag.GetComponent<CardController>();

        if (GameManagerScr.Instance.EnemyFieldCards.Count > 0 && !card.Card.Ranged) return;

        if (card && card.Info.CanAttack && Type == HeroType.ENEMY
        && (!GameManagerScr.Instance.EnemyFieldCards.Exists(x => x.Card.IsProvocation)))
        {
            GameManagerScr.Instance.DamageHero(card, true);
        }

    }

    public void HighlightAsTarget(bool highlight)
    {
        GetComponent<Image>().color = highlight ? TargetCol : NormalCol;
    }

    public void ShowHeroHPChangedEvent(AttackedHero hero, int damageAmount, bool damage)
    {
        Transform startPoint = hero.transform.Find("Health").Find("HPTooltipBase");
        startPoint.DetachChildren();
        TextMeshProUGUI tooltipBase = damage ? GameObject.Find("DamageTooltip").GetComponent<TextMeshProUGUI>() : GameObject.Find("HPRegenTooltip").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI tooltip = Instantiate(tooltipBase, startPoint, false);
        tooltip.text = damage ? "-" + damageAmount.ToString() + "!" : "+" + damageAmount.ToString();
        tooltip.fontSize = 40;
        //Debug.Log("Inside: " + startPoint.name + " " + tooltipBase + " " + tooltip + ". Text: " + tooltip.text);
        tooltip.transform.SetParent(startPoint);
        Destroy(tooltip.gameObject, 1);
    }

    public void ShowHeroMPChangedEvent(AttackedHero hero, int amount)
    {
        Transform startPoint = hero.transform.Find("Mana").Find("ManaTooltipBase");
        startPoint.DetachChildren();
        TextMeshProUGUI tooltipBase = GameObject.Find("MPRegenTooltip").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI tooltip = Instantiate(tooltipBase, startPoint, false);
        tooltip.text = "+" + amount.ToString();
        tooltip.fontSize = 40;
        //Debug.Log("Inside: " + startPoint.name + " " + tooltipBase + " " + tooltip + ". Text: " + tooltip.text);
        tooltip.transform.SetParent(startPoint);
        Destroy(tooltip.gameObject, 1);
    }

    public void ShowHeroGoldChangedEvent(AttackedHero hero, int amount)
    {
        Transform startPoint = hero.transform.Find("Gold").Find("GoldTooltipBase");
        startPoint.DetachChildren();
        TextMeshProUGUI tooltipBase = GameObject.Find("GoldRegenTooltip").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI tooltip = Instantiate(tooltipBase, startPoint, false);
        tooltip.fontSize = 40;
        tooltip.text = "+" + amount.ToString();
        //Debug.Log("Inside: " + startPoint.name + " " + tooltipBase + " " + tooltip + ". Text: " + tooltip.text);
        tooltip.transform.SetParent(startPoint);
        Destroy(tooltip.gameObject, 1);
    }
}
