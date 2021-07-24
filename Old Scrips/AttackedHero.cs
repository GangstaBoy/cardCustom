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
    public enum Hero
    {
        NECROMANCER,
        MAGE_QUEEN,
        DRUID_KING,
        CASTLE_KING
    }
    public Image HeroIcon;
    public HeroType Type;
    public GameManagerScr GameManager;
    public List<Sprite> Icons;
    public Color TargetCol, NormalCol;
    public BuffFactory BuffFactory;

    void Awake()
    {
        //Hero hero = (Hero)Random.Range(0, 3);
        BuffFactory.StatusBars.InitializeStatusBars();
    }

    public void InitializeHero(Hero hero, bool randomize = false)
    {
        if (randomize) hero = (Hero)Random.Range(0, 3);
        switch (hero)
        {
            case Hero.CASTLE_KING:
                HeroIcon.sprite = Icons.Find(x => x.name == "HERO_CASTLE_KING");
                GameManagerScr.Instance.CreateBuffPref(BuffFactory, BuffsManager.GetBuff("armor package"), 3, false);
                break;
            case Hero.NECROMANCER:
                HeroIcon.sprite = Icons.Find(x => x.name == "HERO_NECROMANCER");
                GameManagerScr.Instance.CreateBuffPref(BuffFactory, BuffsManager.GetBuff("skeleton summoner"), 1, false);
                break;
            case Hero.DRUID_KING:
                HeroIcon.sprite = Icons.Find(x => x.name == "HERO_DRUID_KING");
                GameManagerScr.Instance.CreateBuffPref(BuffFactory, BuffsManager.GetBuff("gold income"), 3, false);
                break;
            case Hero.MAGE_QUEEN:
                HeroIcon.sprite = Icons.Find(x => x.name == "HERO_MAGE_QUEEN");
                GameManagerScr.Instance.CreateBuffPref(BuffFactory, BuffsManager.GetBuff("mana flare"), 2, false);
                break;
            default:
                break;
        }
        HeroIcon.preserveAspect = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!GameManagerScr.Instance.IsPlayerTurn) return;

        CardController card = eventData.pointerDrag.GetComponent<CardController>();

        if (GameManagerScr.Instance.EnemyFieldCards.Count > 0 && !card.Card.Ranged) return;

        if (card && card.Info.CanAttack && Type == HeroType.ENEMY
        && (!GameManagerScr.Instance.EnemyFieldCards.Exists(x => x.IsProvocation)))
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
