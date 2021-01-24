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
        DAMAGE_EACH_TURN_SMALL
    }

    public CardController CardController;
    public GameObject Shield, Provocation;
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
                case abilityType.INSTANT_ACTIVE:
                    CardController.Info.CanAttack = true;
                    if(CardController.IsPlayerCard) CardController.Info.HighlightCard(true);
                break;

                case abilityType.HOLY_SHIELD:
                    Shield.SetActive(true);
                break;

                case abilityType.PROVOCATION:
                    Provocation.SetActive(true);
                break;

                case abilityType.DAMAGE_ON_CAST:
                    if(CardController.IsPlayerCard && GameManagerScr.Instance.EnemyFieldCards.Count > 0)
                    {
                        for (int i = GameManagerScr.Instance.EnemyFieldCards.Count - 1; i >= 0; i--)
                        {
                            Debug.Log("Damage to " + GameManagerScr.Instance.EnemyFieldCards[i].name);
                            CardController.GiveDamageTo(GameManagerScr.Instance.EnemyFieldCards[i], ability.AbilityValue);
                            
                        }
                    }
                    else if(!CardController.IsPlayerCard && GameManagerScr.Instance.PlayerFieldCards.Count > 0)
                    {
                        for (int i = GameManagerScr.Instance.PlayerFieldCards.Count - 1; i >= 0; i--)
                        {
                            Debug.Log("Damage to " + GameManagerScr.Instance.EnemyFieldCards[i].name);
                            CardController.GiveDamageTo(GameManagerScr.Instance.PlayerFieldCards[i], ability.AbilityValue);
                            
                        }
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
                case abilityType.DOUBLE_ATTACK:
                    if(CardController.Card.TimesDealDamage == 1)
                    {
                        CardController.Info.CanAttack = true;

                        if (CardController.IsPlayerCard) CardController.Info.HighlightCard(true);
                    }
                break;
                
                default: break;
            }
        }
    }

    public void OnTakeDamage(CardController attacker = null)
    {
        Shield.SetActive(false);
        foreach (var ability in CardController.Card.Abilities)
        {
            switch (ability.AbilityType)
            {
                case abilityType.HOLY_SHIELD:
                    Shield.SetActive(true);
                break;
                
                case abilityType.DECREASE_ATTACK_ON_ATTACK:
                    if(attacker != null) attacker.Card.Attack--;
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
                case abilityType.REGENERATION_EACH_TURN:
                    if(CardController.Card.Defense < CardController.Card.MaxDefense) CardController.Card.Defense = Mathf.Min(CardController.Card.Defense+ability.AbilityValue, CardController.Card.MaxDefense);
                    CardController.Info.Refresh();
                break;

                case abilityType.GOLD_REGENERATION:
                    if(CardController.IsPlayerCard) GameManagerScr.Instance.CurrentGame.Player.Gold+=ability.AbilityValue;
                    else GameManagerScr.Instance.CurrentGame.Enemy.Gold+=ability.AbilityValue;
                break;

                case abilityType.MANA_REGENERATION:
                    if(CardController.IsPlayerCard) GameManagerScr.Instance.CurrentGame.Player.Mana+=ability.AbilityValue;
                    else GameManagerScr.Instance.CurrentGame.Enemy.Mana+=ability.AbilityValue;
                break;

                case abilityType.HERO_REGENERATION:
                    if(CardController.IsPlayerCard) GameManagerScr.Instance.CurrentGame.Player.HP+=ability.AbilityValue;
                    else GameManagerScr.Instance.CurrentGame.Enemy.HP+=ability.AbilityValue;
                break;
                
                default: break;
            }
        }
    }
}
