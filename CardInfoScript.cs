using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfoScript : MonoBehaviour
{
    public CardController CC;
    public Image Logo;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Attack;
    public TextMeshProUGUI Defense;
    public TextMeshProUGUI Manacost;
    public TextMeshProUGUI Goldcost;
    public GameObject HideObj, HighlightedObj;
    public bool CanAttack;
    public Color NormalCol, TargetCol, SpellTargetCol; 

    public void ShowCardInfo()
    {
        Logo.sprite = CC.Card.Logo;
        Logo.preserveAspect = true;
        Name.text = CC.Card.Name;
        HideObj.SetActive(false);
        HighlightedObj.SetActive(false);
        CanAttack = false;

        if(CC.Card.IsSpell)
        {
            Attack.gameObject.SetActive(false);
            Defense.gameObject.SetActive(false);
        }

        Refresh();
    }

    public void Refresh()
    {
        Attack.text = CC.Card.Attack.ToString();
        Defense.text = CC.Card.Defense.ToString();
        Manacost.text = CC.Card.Manacost.ToString();
        Goldcost.text = CC.Card.Goldcost.ToString();
    }

    public void HideCardInfo()
    {
        CanAttack = false;
        HideObj.SetActive(true);
        Manacost.text = "";
        Goldcost.text = "";
    }

    public void HighlightCard(bool highlight) 
    {
        if(HighlightedObj) HighlightedObj.SetActive(highlight);
    }

    public void CheckIfCanBePlayed(int CurrentMana, int CurrentGold)
    {
        GetComponent<CanvasGroup>().alpha = (CurrentMana >= CC.Card.Manacost && CurrentGold >= CC.Card.Goldcost) ? 1 : .5f;
    }

    public void HighlightAsTarget (bool highlight)
    {
        GetComponent<Image>().color = highlight ? TargetCol : NormalCol;
    }

    public void HighlightAsSpellTarget (bool highlight)
    {
        GetComponent<Image>().color = highlight ? SpellTargetCol : NormalCol;
    }



}
