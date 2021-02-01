using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController
{
    public Buff Buff;
    public BuffBehaviour BuffBehaviour;
    public GameObject BuffGameobject;
    AttackedHero Hero;
    CardController CardController;
    StatusBars StatusBars;
    public BuffController(Buff buff, BuffBehaviour buffBehaviour, GameObject buffGameobject, int? buffValue = null, bool isCard = true)
    {
        this.Buff = buff;
        if (buffValue != null) this.Buff.BuffValue = buffValue;
        this.BuffBehaviour = buffBehaviour;
        this.BuffGameobject = buffGameobject;
        if (isCard) CardController = BuffGameobject.transform.parent.parent.GetComponent<CardController>();
        else
        {
            Hero = BuffGameobject.transform.parent.parent.GetComponent<AttackedHero>();
        }
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
            case BuffType.FIRE_SHIELD:
                if (CardController.IsPlayerCard && GameManagerScr.Instance.EnemyFieldCards.Count > 0)
                {
                    for (int i = GameManagerScr.Instance.EnemyFieldCards.Count - 1; i >= 0; i--)
                    {
                        CardController.GiveDamageTo(GameManagerScr.Instance.EnemyFieldCards[i], Buff.BuffValue.Value);
                    }
                }
                else if (!CardController.IsPlayerCard && GameManagerScr.Instance.PlayerFieldCards.Count > 0)
                {
                    for (int i = GameManagerScr.Instance.PlayerFieldCards.Count - 1; i >= 0; i--)
                    {
                        CardController.GiveDamageTo(GameManagerScr.Instance.PlayerFieldCards[i], Buff.BuffValue.Value);
                    }
                }
                break;

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

            case BuffType.ARMOR_PACKAGE:
                if (Hero.Type == AttackedHero.HeroType.PLAYER && GameManagerScr.Instance.PlayerFieldCards.Count > 0)
                {
                    GameManagerScr.Instance.CreateBuffPref(GameManagerScr.Instance.PlayerFieldCards[Random.Range(0, GameManagerScr.Instance.PlayerFieldCards.Count)].BuffFactory, BuffsManager.GetBuff("armor"), Buff.BuffValue.Value, false);
                }
                else if (Hero.Type == AttackedHero.HeroType.ENEMY && GameManagerScr.Instance.EnemyFieldCards.Count > 0)
                {
                    GameManagerScr.Instance.CreateBuffPref(GameManagerScr.Instance.EnemyFieldCards[Random.Range(0, GameManagerScr.Instance.EnemyFieldCards.Count)].BuffFactory, BuffsManager.GetBuff("armor"), Buff.BuffValue.Value, false);
                }
                break;

            case BuffType.SKELETON_SUMMONER:
                if (Hero.Type == AttackedHero.HeroType.PLAYER)
                {
                    GameManagerScr.Instance.PlayerDeck.SpawnCardOfNameToHand(GameManagerScr.Instance.PlayerHand, "skeleton", 1);
                }
                else if (Hero.Type == AttackedHero.HeroType.ENEMY)
                {
                    GameManagerScr.Instance.EnemyDeck.SpawnCardOfNameToHand(GameManagerScr.Instance.EnemyHand, "skeleton", 1);
                }
                break;

            case BuffType.GOLD_INCOME:
                if (Hero.Type == AttackedHero.HeroType.PLAYER)
                {
                    GameManagerScr.Instance.CurrentGame.Player.Gold += Buff.BuffValue.Value;
                    GameManagerScr.Instance.PlayerHero.ShowHeroGoldChangedEvent(GameManagerScr.Instance.PlayerHero, Buff.BuffValue.Value);

                }
                else if (Hero.Type == AttackedHero.HeroType.ENEMY)
                {
                    GameManagerScr.Instance.CurrentGame.Enemy.Gold += Buff.BuffValue.Value;
                    GameManagerScr.Instance.EnemyHero.ShowHeroGoldChangedEvent(GameManagerScr.Instance.EnemyHero, Buff.BuffValue.Value);
                }
                break;

            case BuffType.MANA_FLARE:
                if (Hero.Type == AttackedHero.HeroType.PLAYER)
                {
                    GameManagerScr.Instance.CurrentGame.Player.Mana += Buff.BuffValue.Value;
                    GameManagerScr.Instance.PlayerHero.ShowHeroMPChangedEvent(GameManagerScr.Instance.PlayerHero, Buff.BuffValue.Value);

                }
                else if (Hero.Type == AttackedHero.HeroType.ENEMY)
                {
                    GameManagerScr.Instance.CurrentGame.Enemy.Mana += Buff.BuffValue.Value;
                    GameManagerScr.Instance.EnemyHero.ShowHeroMPChangedEvent(GameManagerScr.Instance.EnemyHero, Buff.BuffValue.Value);
                }
                break;



            default:
                break;
        }
    }

}
