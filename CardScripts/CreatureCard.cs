using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCard : ICard
{

    public bool isPlayable
    {
        get
        {
            return true;
        }

    }
    private CardNew _sourceCard;

    public CreatureCard(CardNew sourceCard)
    {
        _sourceCard = sourceCard;
    }

    public void Play()
    {
        if (!isPlayable) return;
        //var creature = Object.Instantiate(Resources.Load("Prefabs/CreaturePref"), _sourceCard.Owner.FieldTransform);
        Debug.Log(this + " was played like a creature.");
    }
}
