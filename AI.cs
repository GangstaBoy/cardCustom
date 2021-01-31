using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public void MakeTurn()
    {
        StartCoroutine(EnemyTurn(GameManagerScr.Instance.EnemyHandCards));
    }
    IEnumerator EnemyTurn(List<CardController> cards)
    {
        yield return new WaitForSeconds(.51f);
        int j = 0;
        while (j < cards.Count && !GameManagerScr.Instance.IsPlayerTurn)
        {
            var card = cards[j];
            if (GameManagerScr.Instance.CurrentGame.Enemy.Mana >= card.Card.Manacost && GameManagerScr.Instance.CurrentGame.Enemy.Gold >= card.Card.Goldcost)
            {
                Debug.Log("Playing start: " + card.Card.Name);
                if (card.Card.IsSpell)
                {
                    if (!CastSpell(card)) j++;
                    yield return new WaitForSeconds(.51f);
                }
                else
                {
                    card.GetComponent<CardMovementScript>().MoveToField(GameManagerScr.Instance.EnemyField);
                    yield return new WaitForSeconds(.51f);
                    card.transform.SetParent(GameManagerScr.Instance.EnemyField);
                    card.OnCast();
                }
            }
            else
            {
                j++;
                Debug.Log("Cannot play " + card.Card.Name + ". Manacost: " + card.Card.Manacost + " and Goldcost: " + card.Card.Goldcost);
            }
        }

        yield return new WaitForSeconds(.51f);

        while (GameManagerScr.Instance.EnemyFieldCards.Exists(x => x.Info.CanAttack))
        {
            var activeCard = GameManagerScr.Instance.EnemyFieldCards.FindAll(x => x.Info.CanAttack)[0];
            bool hasProvocation = GameManagerScr.Instance.PlayerFieldCards.Exists(x => x.IsProvocation);

            if (GameManagerScr.Instance.PlayerFieldCards.Count == 0)
            {
                activeCard.Info.CanAttack = false;
                activeCard.Movement.MoveToTarget(GameManagerScr.Instance.PlayerHero.transform);
                yield return new WaitForSeconds(.75f);
                GameManagerScr.Instance.DamageHero(activeCard, false);
            }
            else
            {
                CardController enemy;
                if (hasProvocation)
                {
                    enemy = GameManagerScr.Instance.PlayerFieldCards.Find(x => x.IsProvocation);
                }
                else
                {
                    enemy = GameManagerScr.Instance.PlayerFieldCards[Random.Range(0, GameManagerScr.Instance.PlayerFieldCards.Count)];
                }

                if (activeCard.Card.Ranged && !hasProvocation)
                {
                    int i = Random.Range(0, 2);
                    if (i == 0)
                    {
                        activeCard.Movement.MoveToTarget(GameManagerScr.Instance.PlayerHero.transform);
                        yield return new WaitForSeconds(.75f);
                        GameManagerScr.Instance.DamageHero(activeCard, false);
                    }
                    else
                    {
                        activeCard.Movement.MoveToTarget(enemy.transform);
                        yield return new WaitForSeconds(1.03f);
                        GameManagerScr.Instance.CardsFight(activeCard, enemy);
                    }
                }
                else
                {
                    activeCard.Movement.MoveToTarget(enemy.transform);
                    yield return new WaitForSeconds(1.03f);
                    GameManagerScr.Instance.CardsFight(activeCard, enemy);
                }
            }
            yield return new WaitForSeconds(.1f);
        }
        yield return new WaitForSeconds(.25f);
        GameManagerScr.Instance.ChangeTurn();
    }

    bool CastSpell(CardController card)
    {
        switch (((SpellCard)card.Card).SpellTarget)
        {
            case SpellCard.TargetType.NO_TARGET:
                switch (((SpellCard)card.Card).Spell)
                {
                    case SpellCard.SpellType.HEAL_ALLY_FIELD_CARDS:
                        if (GameManagerScr.Instance.EnemyFieldCards.Count > 1
                        && GameManagerScr.Instance.EnemyFieldCards.FindAll(x => x.Card.Defense < x.Card.MaxDefense).Count > 0)
                        {
                            StartCoroutine(CastCard(card));
                            return true;
                        }
                        else return false;

                    case SpellCard.SpellType.DAMAGE_ENEMY_FIELD_CARDS:
                        if (GameManagerScr.Instance.PlayerFieldCards.Count > 1)
                        {
                            StartCoroutine(CastCard(card));
                            return true;
                        }
                        else return false;

                    case SpellCard.SpellType.HEAL_ALLY_HERO:
                        StartCoroutine(CastCard(card));
                        return true;

                    case SpellCard.SpellType.DAMAGE_ENEMY_HERO:
                        StartCoroutine(CastCard(card));
                        return true;

                    case SpellCard.SpellType.ADD_GOLD:
                        StartCoroutine(CastCard(card));
                        return true;

                    case SpellCard.SpellType.ADD_MANA:
                        StartCoroutine(CastCard(card));
                        return true;
                }
                return false;

            case SpellCard.TargetType.ALLY_CARD_TARGET:
                switch (((SpellCard)card.Card).Spell)
                {
                    case SpellCard.SpellType.BUFF_CARD_DAMAGE:
                        if (GameManagerScr.Instance.EnemyFieldCards.Count > 0)
                        {
                            StartCoroutine(CastCard(card, GameManagerScr.Instance.EnemyFieldCards[Random.Range(0, GameManagerScr.Instance.EnemyFieldCards.Count)]));
                            return true;
                        }
                        else return false;

                    case SpellCard.SpellType.HEAL_CARD:
                        if (GameManagerScr.Instance.EnemyFieldCards.Count > 0
                        && GameManagerScr.Instance.EnemyFieldCards.FindAll(x => x.Card.Defense < x.Card.MaxDefense).Count > 0)
                        {
                            StartCoroutine(CastCard(card, GameManagerScr.Instance.EnemyFieldCards.Find(x => x.Card.Defense < x.Card.MaxDefense)));
                            return true;
                        }
                        else return false;

                    case SpellCard.SpellType.PROVOCATION_ON_ALLY_CARD:
                        if (GameManagerScr.Instance.EnemyFieldCards.Count > 0)
                        {
                            StartCoroutine(CastCard(card, GameManagerScr.Instance.EnemyFieldCards[Random.Range(0, GameManagerScr.Instance.EnemyFieldCards.Count)]));
                            return true;
                        }
                        else return false;

                    case SpellCard.SpellType.DOUBLE_ATTACK_ON_ALLY_CARD:
                        if (GameManagerScr.Instance.EnemyFieldCards.Count > 0)
                        {
                            StartCoroutine(CastCard(card, GameManagerScr.Instance.EnemyFieldCards[Random.Range(0, GameManagerScr.Instance.EnemyFieldCards.Count)]));
                            return true;
                        }
                        else return false;

                    case SpellCard.SpellType.ARMOR_ON_CARD:
                        if (GameManagerScr.Instance.EnemyFieldCards.Count > 0)
                        {
                            StartCoroutine(CastCard(card, GameManagerScr.Instance.EnemyFieldCards[Random.Range(0, GameManagerScr.Instance.EnemyFieldCards.Count)]));
                            return true;
                        }
                        else return false;

                    case SpellCard.SpellType.SHIELD_ON_ALLY_CARD:
                        if (GameManagerScr.Instance.EnemyFieldCards.Count > 0)
                        {
                            StartCoroutine(CastCard(card, GameManagerScr.Instance.EnemyFieldCards[Random.Range(0, GameManagerScr.Instance.EnemyFieldCards.Count)]));
                            return true;
                        }
                        else return false;
                }
                return false;

            case SpellCard.TargetType.ENEMY_CARD_TARGET:
                switch (((SpellCard)card.Card).Spell)
                {
                    case SpellCard.SpellType.DEBUFF_CARD_DAMAGE:
                        if (GameManagerScr.Instance.PlayerFieldCards.Count > 0)
                        {
                            StartCoroutine(CastCard(card, GameManagerScr.Instance.PlayerFieldCards[Random.Range(0, GameManagerScr.Instance.PlayerFieldCards.Count)]));
                            return true;
                        }
                        else return false;

                    case SpellCard.SpellType.DAMAGE_CARD:
                        if (GameManagerScr.Instance.PlayerFieldCards.Count > 0)
                        {
                            StartCoroutine(CastCard(card, GameManagerScr.Instance.PlayerFieldCards[Random.Range(0, GameManagerScr.Instance.PlayerFieldCards.Count)]));
                            return true;
                        }
                        else return false;
                }
                return false;

            default:
                return false;
        }
    }

    IEnumerator CastCard(CardController spell, CardController target = null)
    {
        if (((SpellCard)spell.Card).SpellTarget == SpellCard.TargetType.NO_TARGET)
        {
            spell.Info.ShowCardInfo();
            spell.GetComponent<CardMovementScript>().MoveToField(GameManagerScr.Instance.EnemyField);
            yield return new WaitForSeconds(.51f);
            GameManagerScr.Instance.ReduceManaAndGold(false, spell.Card.Manacost, spell.Card.Goldcost);
            Debug.Log("Spell using " + spell.Card.Name);
            spell.UseSpell(null);
        }
        else
        {
            spell.Info.ShowCardInfo();
            spell.GetComponent<CardMovementScript>().MoveToTarget(target.transform, false);

            yield return new WaitForSeconds(.51f);
            GameManagerScr.Instance.EnemyHandCards.Remove(spell);
            GameManagerScr.Instance.ReduceManaAndGold(false, spell.Card.Manacost, spell.Card.Goldcost);

            spell.Card.IsPlaced = true;
            Debug.Log("Using spell:" + spell.Card.Name + " on target " + target.Card.Name);
            spell.UseSpell(target);
        }
    }
}
