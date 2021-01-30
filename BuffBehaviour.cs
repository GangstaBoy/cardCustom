using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffBehaviour : MonoBehaviour
{
    public Image Logo;
    public void Init(Buff buff)
    {
        Logo.sprite = Resources.Load<Sprite>(buff.LogoPath);
    }

    void Refresh()
    {
        //Logo.sprite = Resources.Load<Sprite>(Buff.LogoPath);
    }
}
