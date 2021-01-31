using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController
{
    public Buff Buff;
    public BuffBehaviour BuffBehaviour;
    public GameObject BuffGameobject;
    CardController CardController;
    StatusBars StatusBars;
    public BuffController(Buff buff, BuffBehaviour buffBehaviour, GameObject buffGameobject, int? buffValue = null)
    {
        this.Buff = buff;
        if (buffValue != null) this.Buff.BuffValue = buffValue;
        this.BuffBehaviour = buffBehaviour;
        this.BuffGameobject = buffGameobject;
        CardController = BuffGameobject.transform.parent.parent.GetComponent<CardController>();
        StatusBars = this.BuffGameobject.transform.parent.parent.GetComponent<StatusBars>();
    }

    public void OnAdd()
    {
        switch (Buff.BuffType)
        {
            case BuffType.HOLY_SHIELD:
                {
                    CardController.Info.Shield.SetActive(true);
                    break;
                }

            case BuffType.PROVOCATION:
                {
                    CardController.Info.Provocation.SetActive(true);
                    break;
                }

            default:
                break;
        }
    }

    public void OnDamageDeal()
    {
        switch (Buff.BuffType)
        {
            case BuffType.DOUBLE_ATTACK:
                if (CardController.Card.TimesDealDamage == 1)
                {
                    CardController.Info.CanAttack = true;
                    if (CardController.IsPlayerCard) CardController.Info.HighlightCard(true);
                }
                break;

            default:
                break;
        }
    }

    public void OnRemove()
    {
        switch (Buff.BuffType)
        {
            case BuffType.HOLY_SHIELD:
                if (!StatusBars.Buffs.Exists(x => x.Buff.BuffType == BuffType.HOLY_SHIELD))
                {
                    CardController.Info.Shield.gameObject.SetActive(false);
                }
                break;

            case BuffType.PROVOCATION:
                if (!StatusBars.Buffs.Exists(x => x.Buff.BuffType == BuffType.PROVOCATION))
                {
                    CardController.Info.Provocation.gameObject.SetActive(false);
                }
                break;

            default:
                break;
        }
    }

    public void OnNewTurn()
    {
        switch (Buff.BuffType)
        {
            case BuffType.SELF_HP_REGENERATION:
                int regenAmount = Buff.BuffValue == null ? 0 : Buff.BuffValue.Value;
                CardController.RegenCardHP(CardController, regenAmount);
                break;

            case BuffType.ALL_ALLIES_REGENERATION:
                if (CardController.IsPlayerCard && GameManagerScr.Instance.PlayerFieldCards.Count > 0)
                {
                    for (int i = GameManagerScr.Instance.PlayerFieldCards.Count - 1; i >= 0; i--)
                    {
                        CardController.RegenCardHP(GameManagerScr.Instance.PlayerFieldCards[i], Buff.BuffValue.Value);
                    }
                }
                else if (!CardController.IsPlayerCard && GameManagerScr.Instance.EnemyFieldCards.Count > 0)
                {
                    for (int i = GameManagerScr.Instance.EnemyFieldCards.Count - 1; i >= 0; i--)
                    {
                        CardController.RegenCardHP(GameManagerScr.Instance.EnemyFieldCards[i], Buff.BuffValue.Value);
                    }
                }
                break;


            default:
                break;
        }
    }

}
