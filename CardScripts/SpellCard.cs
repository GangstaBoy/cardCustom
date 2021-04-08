using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCardNew : ICard
{
    public bool isPlayable
    {
        get
        {
            return true;
        }

    }
    public void Play()
    {
        if (!isPlayable) return;
        Debug.Log(this + " was played like a spell");
    }
}
