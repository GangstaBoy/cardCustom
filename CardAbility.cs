using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAbility : MonoBehaviour
{
    public enum abilityType
    {
        NO_ABILITY,
        INSTANT_ACTIVE,
        DOUBLE_ATTACK,
        HOLY_SHIELD,
        PROVOCATION,
        REGENERATION_EACH_TURN,
        DECREASE_ATTACK_ON_ATTACK,
        RANGED,
        GOLD_REGENERATION,
        MANA_REGENERATION,
        HERO_REGENERATION,
        DAMAGE_ON_CAST,
        DAMAGE_EACH_TURN,
        DAMAGE_HERO_ON_CAST,
        MANA_ON_CAST,
        HEAL_ALLY_FIELD_CARDS_ON_CAST,
        HEAL_ALLY_FIELD_CARDS_EACH_TURN,
        LIFESTEAL

    }

    public CardController CardController;
    public int AbilityValue;
    public abilityType AbilityType;

    public CardAbility(abilityType ability, int abilityValue = 0)
    {
        AbilityType = ability;
        AbilityValue = abilityValue;
    }

    public void OnCast()
    {
        foreach (var ability in CardController.Card.Abilities)
        {
            switch (ability.AbilityType)
            {
                case abilityType.MANA_ON_CAST:
                    if (CardController.IsPlayerCard)
                    {
                        GameManagerScr.Instance.CurrentGame.Player.Mana += ability.AbilityValue;
                        GameManagerScr.Instance.PlayerHero.ShowHeroMPChangedEvent(GameManagerScr.Instance.PlayerHero, ability.AbilityValue);
                    }
                    else
                    {
                        GameManagerScr.Instance.CurrentGame.Enemy.Mana += ability.AbilityValue;
                        GameManagerScr.Instance.EnemyHero.ShowHeroMPChangedEvent(GameManagerScr.Instance.EnemyHero, ability.AbilityValue);
                    }
                    break;

                case abilityType.INSTANT_ACTIVE:
                    CardController.Info.CanAttack = true;
                    if (CardController.IsPlayerCard) CardController.Info.HighlightCard(true);
                    break;

                case abilityType.DOUBLE_ATTACK:
                    GameManagerScr.Instance.CreateBuffPref(CardController, BuffsManager.GetBuff("double attack"));
                    break;

                case abilityType.HOLY_SHIELD:
                    GameManagerScr.Instance.CreateBuffPref(CardController, BuffsManager.GetBuff("magic shield"));
                    break;

                case abilityType.PROVOCATION:
                    GameManagerScr.Instance.CreateBuffPref(CardController, BuffsManager.GetBuff("provocation"));
                    break;

                case abilityType.REGENERATION_EACH_TURN:
                    GameManagerScr.Instance.CreateBuffPref(CardController, BuffsManager.GetBuff("regeneration"), 2);
                    break;

                case abilityType.HEAL_ALLY_FIELD_CARDS_EACH_TURN:
                    GameManagerScr.Instance.CreateBuffPref(CardController, BuffsManager.GetBuff("healing aura"), 1);
                    break;

                case abilityType.HEAL_ALLY_FIELD_CARDS_ON_CAST:
                    if (CardController.IsPlayerCard && GameManagerScr.Instance.PlayerFieldCards.Count > 0)
                    {
                        for (int i = GameManagerScr.Instance.PlayerFieldCards.Count - 1; i >= 0; i--)
                        {
                            CardController.RegenCardHP(GameManagerScr.Instance.PlayerFieldCards[i], ability.AbilityValue);
                        }
                    }
                    else if (!CardController.IsPlayerCard && GameManagerScr.Instance.EnemyFieldCards.Count > 0)
                    {
                        for (int i = GameManagerScr.Instance.EnemyFieldCards.Count - 1; i >= 0; i--)
                        {
                            CardController.RegenCardHP(GameManagerScr.Instance.EnemyFieldCards[i], ability.AbilityValue);
                        }
                    }
                    break;

                case abilityType.DAMAGE_ON_CAST:
                    if (CardController.IsPlayerCard && GameManagerScr.Instance.EnemyFieldCards.Count > 0)
                    {
                        for (int i = GameManagerScr.Instance.EnemyFieldCards.Count - 1; i >= 0; i--)
                        {
                            Debug.Log("Damage to " + GameManagerScr.Instance.EnemyFieldCards[i].name);
                            CardController.GiveDamageTo(GameManagerScr.Instance.EnemyFieldCards[i], ability.AbilityValue);

                        }
                    }
                    else if (!CardController.IsPlayerCard && GameManagerScr.Instance.PlayerFieldCards.Count > 0)
                    {
                        for (int i = GameManagerScr.Instance.PlayerFieldCards.Count - 1; i >= 0; i--)
                        {
                            Debug.Log("Damage to " + GameManagerScr.Instance.PlayerFieldCards[i].name);
                            CardController.GiveDamageTo(GameManagerScr.Instance.PlayerFieldCards[i], ability.AbilityValue);

                        }
                    }
                    break;

                case abilityType.DAMAGE_HERO_ON_CAST:
                    if (CardController.IsPlayerCard)
                    {
                        GameManagerScr.Instance.CurrentGame.Enemy.GetDamage(ability.AbilityValue);
                        GameManagerScr.Instance.EnemyHero.ShowHeroHPChangedEvent(GameManagerScr.Instance.EnemyHero, ability.AbilityValue, true);
                        GameManagerScr.Instance.CheckResult();
                    }
                    else if (!CardController.IsPlayerCard)
                    {
                        GameManagerScr.Instance.CurrentGame.Player.GetDamage(ability.AbilityValue);
                        GameManagerScr.Instance.PlayerHero.ShowHeroHPChangedEvent(GameManagerScr.Instance.PlayerHero, ability.AbilityValue, true);
                        GameManagerScr.Instance.CheckResult();
                    }
                    break;

                default: break;
            }
        }
    }

    public void OnDamageDeal()
    {
        foreach (var ability in CardController.Card.Abilities)
        {
            switch (ability.AbilityType)
            {
                case abilityType.LIFESTEAL:
                    CardController.RegenCardHP(CardController, ability.AbilityValue);
                    break;

                default: break;
            }
        }
    }

    public void OnTakeDamage(CardController attacker = null)
    {
        foreach (var ability in CardController.Card.Abilities)
        {
            switch (ability.AbilityType)
            {
                case abilityType.DECREASE_ATTACK_ON_ATTACK:
                    if (attacker != null) attacker.Card.Attack--;
                    break;

                default: break;
            }
        }
    }

    public void OnNewTurn()
    {
        CardController.Card.TimesDealDamage = 0;
        CardController.Card.TimesTookDamage = 0;
        foreach (var ability in CardController.Card.Abilities)
        {
            switch (ability.AbilityType)
            {
                case abilityType.GOLD_REGENERATION:
                    if (CardController.IsPlayerCard)
                    {
                        GameManagerScr.Instance.CurrentGame.Player.Gold += ability.AbilityValue;
                        GameManagerScr.Instance.PlayerHero.ShowHeroGoldChangedEvent(GameManagerScr.Instance.PlayerHero, ability.AbilityValue);
                    }
                    else
                    {
                        GameManagerScr.Instance.CurrentGame.Enemy.Gold += ability.AbilityValue;
                        GameManagerScr.Instance.EnemyHero.ShowHeroGoldChangedEvent(GameManagerScr.Instance.EnemyHero, ability.AbilityValue);
                    }
                    break;

                case abilityType.MANA_REGENERATION:
                    if (CardController.IsPlayerCard)
                    {
                        GameManagerScr.Instance.CurrentGame.Player.Mana += ability.AbilityValue;
                        GameManagerScr.Instance.PlayerHero.ShowHeroMPChangedEvent(GameManagerScr.Instance.PlayerHero, ability.AbilityValue);
                    }
                    else
                    {
                        GameManagerScr.Instance.CurrentGame.Enemy.Mana += ability.AbilityValue;
                        GameManagerScr.Instance.EnemyHero.ShowHeroMPChangedEvent(GameManagerScr.Instance.EnemyHero, ability.AbilityValue);
                    }
                    break;

                case abilityType.HERO_REGENERATION:
                    if (CardController.IsPlayerCard)
                    {
                        GameManagerScr.Instance.CurrentGame.Player.HP += ability.AbilityValue;
                        GameManagerScr.Instance.PlayerHero.ShowHeroHPChangedEvent(GameManagerScr.Instance.PlayerHero, ability.AbilityValue, false);
                    }
                    else
                    {
                        GameManagerScr.Instance.CurrentGame.Enemy.HP += ability.AbilityValue;
                        GameManagerScr.Instance.EnemyHero.ShowHeroHPChangedEvent(GameManagerScr.Instance.EnemyHero, ability.AbilityValue, false);
                    }
                    break;

                case abilityType.DAMAGE_EACH_TURN:
                    if (CardController.IsPlayerCard && GameManagerScr.Instance.EnemyFieldCards.Count > 0)
                    {
                        for (int i = GameManagerScr.Instance.EnemyFieldCards.Count - 1; i >= 0; i--)
                        {
                            CardController.GiveDamageTo(GameManagerScr.Instance.EnemyFieldCards[i], ability.AbilityValue);

                        }
                    }
                    else if (!CardController.IsPlayerCard && GameManagerScr.Instance.PlayerFieldCards.Count > 0)
                    {
                        for (int i = GameManagerScr.Instance.PlayerFieldCards.Count - 1; i >= 0; i--)
                        {
                            CardController.GiveDamageTo(GameManagerScr.Instance.PlayerFieldCards[i], ability.AbilityValue);

                        }
                    }
                    break;

                default: break;
            }
        }
    }
}
