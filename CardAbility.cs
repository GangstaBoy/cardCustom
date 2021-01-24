using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAbility : MonoBehaviour
{

    public CardController CardController;
    public GameObject Shield, Provocation;

    public void OnCast()
    {
        foreach (var ability in CardController.Card.Abilities)
        {
            switch (ability)
            {
                case Card.abilityType.INSTANT_ACTIVE:
                    CardController.Info.CanAttack = true;
                    if(CardController.IsPlayerCard) CardController.Info.HighlightCard(true);
                break;

                case Card.abilityType.HOLY_SHIELD:
                    Shield.SetActive(true);
                break;

                case Card.abilityType.PROVOCATION:
                    Provocation.SetActive(true);
                break;

                case Card.abilityType.DAMAGE_ON_CAST:
                    Debug.Log("Damage on Cast");
                    if(CardController.IsPlayerCard && GameManagerScr.Instance.EnemyFieldCards.Count > 0)
                    {
                        for (int i = GameManagerScr.Instance.EnemyFieldCards.Count - 1; i < 0; i--)
                        {
                            Debug.Log("Damage to " + GameManagerScr.Instance.EnemyFieldCards[i].name);
                            CardController.GiveDamageTo(GameManagerScr.Instance.EnemyFieldCards[i], 2);
                            
                        }
                    }
                    else if(!CardController.IsPlayerCard && GameManagerScr.Instance.PlayerFieldCards.Count > 0)
                    {
                        for (int i = GameManagerScr.Instance.PlayerFieldCards.Count - 1; i < 0; i--)
                        {
                            Debug.Log("Damage to " + GameManagerScr.Instance.EnemyFieldCards[i].name);
                            CardController.GiveDamageTo(GameManagerScr.Instance.PlayerFieldCards[i], 2);
                            
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
            switch (ability)
            {
                case Card.abilityType.DOUBLE_ATTACK:
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
            switch (ability)
            {
                case Card.abilityType.HOLY_SHIELD:
                    Shield.SetActive(true);
                break;
                
                case Card.abilityType.DECREASE_ATTACK_ON_ATTACK:
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
            switch (ability)
            {
                case Card.abilityType.REGENERATION_EACH_TURN_BASIC:
                    if(CardController.Card.Defense < CardController.Card.MaxDefense) CardController.Card.Defense = Mathf.Min(CardController.Card.Defense+1, CardController.Card.MaxDefense);
                    CardController.Info.Refresh();
                break;

                case Card.abilityType.REGENERATION_EACH_TURN_MODERATE:
                    if(CardController.Card.Defense < CardController.Card.MaxDefense) CardController.Card.Defense = Mathf.Min(CardController.Card.Defense+2, CardController.Card.MaxDefense);
                    CardController.Info.Refresh();
                break;

                case Card.abilityType.TOLL:
                    if(CardController.IsPlayerCard) GameManagerScr.Instance.CurrentGame.Player.Gold++;
                    else GameManagerScr.Instance.CurrentGame.Enemy.Gold++;
                break;

                case Card.abilityType.MANA_REGENERATION_BASIC:
                    if(CardController.IsPlayerCard) GameManagerScr.Instance.CurrentGame.Player.Mana++;
                    else GameManagerScr.Instance.CurrentGame.Enemy.Mana++;
                break;

                case Card.abilityType.MANA_REGENERATION_MODERATE:
                    if(CardController.IsPlayerCard) GameManagerScr.Instance.CurrentGame.Player.Mana += 2;
                    else GameManagerScr.Instance.CurrentGame.Enemy.Mana += 2;
                break;

                case Card.abilityType.HERO_REGENERATION_BASIC:
                    if(CardController.IsPlayerCard) GameManagerScr.Instance.CurrentGame.Player.HP++;
                    else GameManagerScr.Instance.CurrentGame.Enemy.HP++;
                break;

                case Card.abilityType.HERO_REGENERATION_MODERATE:
                    if(CardController.IsPlayerCard) GameManagerScr.Instance.CurrentGame.Player.HP += 2;
                    else GameManagerScr.Instance.CurrentGame.Enemy.HP += 2;
                break;
                
                default: break;
            }
        }
    }
}
