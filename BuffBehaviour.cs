using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffBehaviour : MonoBehaviour
{
    public Image Logo;
    public GameObject ValueToken;
    public TextMeshProUGUI ValueText;
    Buff Buff;
    public void Init(Buff buff)
    {
        Logo.sprite = Resources.Load<Sprite>(buff.LogoPath);
        Buff = buff;
        Refresh();
    }

    void Update()
    {
        if (Buff != null) Refresh();
    }

    public void Refresh()
    {
        if (Buff.BuffValue != null)
        {
            ValueToken.SetActive(true);
            ValueText.text = Buff.BuffValue.ToString();
        }
    }
}
