using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellTarget : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (!GameManagerScr.Instance.IsPlayerTurn) return;

        CardController spell = eventData.pointerDrag.GetComponent<CardController>(),
                        target = GetComponent<CardController>();

        if (spell && spell.Card.IsSpell && spell.IsPlayerCard && target.Card.IsPlaced && GameManagerScr.Instance.CurrentGame.Player.Mana >= spell.Card.Manacost && GameManagerScr.Instance.CurrentGame.Player.Gold >= spell.Card.Goldcost)
        {
            var spellCard = (SpellCard)spell.Card;

            if ((spellCard.SpellTarget == SpellCard.TargetType.ENEMY_CARD_TARGET && !target.IsPlayerCard)
            || (spellCard.SpellTarget == SpellCard.TargetType.ALLY_CARD_TARGET && target.IsPlayerCard))
            {
                GameManagerScr.Instance.ReduceManaAndGold(true, spell.Card.Manacost, spell.Card.Goldcost);
                spell.UseSpell(target);
            }

        }
    }
}
