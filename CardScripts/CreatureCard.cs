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
    private CardNew _parent;

    public CreatureCard(CardNew parent)
    {
        _parent = parent;
    }

    public void Play()
    {
        if (!isPlayable) return;
        var card = Object.Instantiate(Resources.Load("Prefabs/CreaturePref"), _parent.Owner.FieldTransform);
        Debug.Log(this + " was played like a creature.");
    }
}
