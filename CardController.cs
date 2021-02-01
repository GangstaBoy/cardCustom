using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardController : MonoBehaviour
{
    public CardAbility AbilityController;
    public Card Card;
    public bool IsPlayerCard;
    public CardInfoScript Info;
    public CardMovementScript Movement;
    public BuffFactory BuffFactory;
    public StatusBars StatusBars;
    public bool IsProvocation
    {
        get
        {
            return StatusBars.Buffs.Exists(x => x.Buff.BuffType == BuffType.PROVOCATION);
        }
    }

    GameManagerScr GameManager;

    public void Init(Card card, bool isPlayerCard)
    {

        Card = card;
        GameManager = GameManagerScr.Instance;
        IsPlayerCard = isPlayerCard;
        Info.Shield.SetActive(false);
        Info.Provocation.SetActive(false);

        if (isPlayerCard)
        {
            Info.ShowCardInfo();
            GetComponent<AttackedCard>().enabled = false;
        }
        else Info.HideCardInfo();
    }

    public void OnCast()
    {
        //if (Card.IsSpell && ((SpellCard)Card).SpellTarget != SpellCard.TargetType.NO_TARGET) return;
        StatusBars.InitializeStatusBars();
        if (IsPlayerCard)
        {
            GameManager.PlayerHandCards.Remove(this);
            GameManager.PlayerFieldCards.Add(this);
            GameManager.ReduceManaAndGold(true, Card.Manacost, Card.Goldcost);

        }
        else
        {
            GameManager.EnemyHandCards.Remove(this);
            GameManager.EnemyFieldCards.Add(this);
            GameManager.ReduceManaAndGold(false, Card.Manacost, Card.Goldcost);
            Info.ShowCardInfo();
        }

        Card.IsPlaced = true;
        if (Card.HasAbility) AbilityController.OnCast();
        if (Card.IsSpell) UseSpell(null);
    }

    public void OnTakeDamage(CardController attacker = null)
    {
        CheckIfAlive();
        AbilityController.OnTakeDamage(attacker);
    }

    public void OnDamageDeal()
    {
        Card.TimesDealDamage++;
        Info.CanAttack = false;
        Info.HighlightCard(false);

        if (Card.HasAbility) AbilityController.OnDamageDeal();
        StatusBars.TriggerDamageDeal();
    }

    public void UseSpell(CardController target)
    {
        var spellCard = (SpellCard)Card;

        switch (spellCard.Spell)
        {
            case SpellCard.SpellType.ADD_GOLD:
                if (IsPlayerCard)
                {
                    GameManagerScr.Instance.CurrentGame.Player.Gold += spellCard.SpellValue;
                    GameManagerScr.Instance.PlayerHero.ShowHeroGoldChangedEvent(GameManagerScr.Instance.PlayerHero, spellCard.SpellValue);
                }
                else
                {
                    GameManagerScr.Instance.CurrentGame.Enemy.Gold += spellCard.SpellValue;
                    GameManagerScr.Instance.EnemyHero.ShowHeroGoldChangedEvent(GameManagerScr.Instance.EnemyHero, spellCard.SpellValue);
                }
                break;

            case SpellCard.SpellType.ADD_MANA:
                if (IsPlayerCard)
                {
                    GameManagerScr.Instance.CurrentGame.Player.Mana += spellCard.SpellValue;
                    GameManagerScr.Instance.PlayerHero.ShowHeroMPChangedEvent(GameManagerScr.Instance.PlayerHero, spellCard.SpellValue);
                }
                else
                {
                    GameManagerScr.Instance.CurrentGame.Enemy.Mana += spellCard.SpellValue;
                    GameManagerScr.Instance.EnemyHero.ShowHeroMPChangedEvent(GameManagerScr.Instance.EnemyHero, spellCard.SpellValue);
                }
                break;

            case SpellCard.SpellType.BUFF_CARD_DAMAGE:
                target.Card.Attack += spellCard.SpellValue;
                break;

            case SpellCard.SpellType.DAMAGE_ENEMY_FIELD_CARDS:
                var enemyCards = IsPlayerCard ?
                    new List<CardController>(GameManager.EnemyFieldCards) :
                    new List<CardController>(GameManager.PlayerFieldCards);

                foreach (var card in enemyCards)
                {
                    GiveDamageTo(card, spellCard.SpellValue);
                }
                break;

            case SpellCard.SpellType.DAMAGE_ENEMY_HERO:
                if (IsPlayerCard)
                {
                    GameManagerScr.Instance.CurrentGame.Enemy.HP -= spellCard.SpellValue;
                    GameManagerScr.Instance.EnemyHero.ShowHeroHPChangedEvent(GameManagerScr.Instance.EnemyHero, spellCard.SpellValue, true);
                }
                else
                {
                    GameManagerScr.Instance.CurrentGame.Player.HP -= spellCard.SpellValue;
                    GameManagerScr.Instance.PlayerHero.ShowHeroHPChangedEvent(GameManagerScr.Instance.PlayerHero, spellCard.SpellValue, true);
                }
                GameManager.CheckResult();
                break;

            case SpellCard.SpellType.DEBUFF_CARD_DAMAGE:
                target.Card.Attack = Mathf.Clamp(target.Card.Attack - spellCard.SpellValue, 0, int.MaxValue);
                break;

            case SpellCard.SpellType.HEAL_ALLY_FIELD_CARDS:
                var allyCards = IsPlayerCard ? GameManager.PlayerFieldCards : GameManager.EnemyFieldCards;

                foreach (var card in allyCards)
                {
                    card.RegenCardHP(card, spellCard.SpellValue);
                }

                break;

            case SpellCard.SpellType.HEAL_ALLY_HERO:
                if (IsPlayerCard)
                {
                    GameManagerScr.Instance.CurrentGame.Player.HP += spellCard.SpellValue;
                    GameManagerScr.Instance.PlayerHero.ShowHeroHPChangedEvent(GameManagerScr.Instance.PlayerHero, spellCard.SpellValue, false);
                }
                else
                {
                    GameManagerScr.Instance.CurrentGame.Enemy.HP += spellCard.SpellValue;
                    GameManagerScr.Instance.EnemyHero.ShowHeroHPChangedEvent(GameManagerScr.Instance.EnemyHero, spellCard.SpellValue, false);
                }
                break;


            case SpellCard.SpellType.PROVOCATION_ON_ALLY_CARD:
                if (!target.StatusBars.Buffs.Exists(x => x.Buff.BuffType == BuffType.PROVOCATION))
                    GameManager.CreateBuffPref(target.BuffFactory, BuffsManager.GetBuff("provocation"));
                break;

            case SpellCard.SpellType.REGENERATION_ON_CARD:
                GameManager.CreateBuffPref(target.BuffFactory, BuffsManager.GetBuff("regeneration"), spellCard.SpellValue);
                break;

            case SpellCard.SpellType.REGENERATION_AURA_ON_CARD:
                GameManager.CreateBuffPref(target.BuffFactory, BuffsManager.GetBuff("healing aura"), spellCard.SpellValue);
                break;

            case SpellCard.SpellType.FIRE_SHIELD_ON_CARD:
                GameManager.CreateBuffPref(target.BuffFactory, BuffsManager.GetBuff("fire shield"), spellCard.SpellValue);
                break;

            case SpellCard.SpellType.ARMOR_ON_CARD:
                GameManager.CreateBuffPref(target.BuffFactory, BuffsManager.GetBuff("armor"), spellCard.SpellValue);
                break;

            case SpellCard.SpellType.DOUBLE_ATTACK_ON_ALLY_CARD:
                if (!target.StatusBars.Buffs.Exists(x => x.Buff.BuffType == BuffType.DOUBLE_ATTACK))
                    GameManager.CreateBuffPref(target.BuffFactory, BuffsManager.GetBuff("double attack"));
                break;

            case SpellCard.SpellType.SHIELD_ON_ALLY_CARD:
                GameManager.CreateBuffPref(target.BuffFactory, BuffsManager.GetBuff("magic shield"));
                break;

            case SpellCard.SpellType.DAMAGE_CARD:
                GiveDamageTo(target, spellCard.SpellValue);
                break;

            case SpellCard.SpellType.HEAL_CARD:
                target.RegenCardHP(target, spellCard.SpellValue);
                break;

            default:
                break;
        }

        if (target != null)
        {
            target.CheckIfAlive();
        }
        DestroyCard();
    }


    public void GiveDamageTo(CardController card, int damage)
    {
        if (!card.StatusBars.Buffs.Exists(x => x.Buff.BuffType == BuffType.HOLY_SHIELD)) // fix here not to check twice
            ShowCardHPChangedEvent(card, damage, true);
        card.GetDamage(damage);

        card.OnTakeDamage();
        card.CheckIfAlive();
    }

    public void GetDamage(int damage, bool isPure = false)
    {
        if (damage > 0)
        {
            if (StatusBars.Buffs.Exists(x => x.Buff.BuffType == BuffType.HOLY_SHIELD))
            {
                var shield = StatusBars.Buffs.Find(x => x.Buff.BuffType == BuffType.HOLY_SHIELD);
                if (shield.Buff.BuffValue == 1) StatusBars.Remove(StatusBars.Buffs.Find(x => x.Buff.BuffType == BuffType.HOLY_SHIELD));
                else shield.Buff.BuffValue--;
            }
            else if (StatusBars.Buffs.Exists(x => x.Buff.BuffType == BuffType.ARMOR) && !isPure)
            {
                var armor = StatusBars.Buffs.Find(x => x.Buff.BuffType == BuffType.ARMOR);
                if (armor.Buff.BuffValue < damage)
                {
                    StatusBars.Remove(StatusBars.Buffs.Find(x => x.Buff.BuffType == BuffType.ARMOR));
                    this.GetDamage(damage - armor.Buff.BuffValue.GetValueOrDefault());
                }
                else armor.Buff.BuffValue -= damage;
            }
            else
            {
                Card.Defense -= damage;
            }
        }
    }

    public void RegenCardHP(CardController card, int regenAmount)
    {
        if (card.Card.Defense < card.Card.MaxDefense)
        {
            card.Card.Defense = Mathf.Min(card.Card.Defense + regenAmount, card.Card.MaxDefense);
            card.Info.Refresh();
            ShowCardHPChangedEvent(card, regenAmount, false);
        }
    }

    void ShowCardHPChangedEvent(CardController card, int damageAmount, bool damage)
    {

        Transform startPoint = card.transform.Find("DamageStartPoint");
        TextMeshProUGUI tooltipBase = damage ? GameObject.Find("DamageTooltip").GetComponent<TextMeshProUGUI>() : GameObject.Find("HPRegenTooltip").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI tooltip = Instantiate(tooltipBase, startPoint, false);
        tooltip.text = damage ? "-" + damageAmount.ToString() + "!" : "+" + damageAmount.ToString();
        tooltip.transform.SetParent(startPoint);
        Destroy(tooltip, 0.75f);
    }

    public void CheckIfAlive()
    {
        if (Card.IsAlive) Info.Refresh();
        else DestroyCard();
    }

    void DestroyCard()
    {
        Movement.OnEndDrag(null);

        RemoveCardFromList(GameManager.EnemyFieldCards);
        RemoveCardFromList(GameManager.EnemyHandCards);
        RemoveCardFromList(GameManager.PlayerFieldCards);
        RemoveCardFromList(GameManager.PlayerHandCards);

        Destroy(gameObject);
    }

    void RemoveCardFromList(List<CardController> list)
    {
        if (list.Exists(x => x == this)) list.Remove(this);
    }
}
