using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController
{
    public Buff Buff;
    public BuffBehaviour BuffBehaviour;
    public GameObject BuffGameobject;
    CardController CardController;
    StatusBars StatusBars;
    public BuffController(Buff buff, BuffBehaviour buffBehaviour, GameObject buffGameobject)
    {
        this.Buff = buff;
        this.BuffBehaviour = buffBehaviour;
        this.BuffGameobject = buffGameobject;
        CardController = BuffGameobject.transform.parent.parent.GetComponent<CardController>();
        StatusBars = this.BuffGameobject.transform.parent.parent.GetComponent<StatusBars>();
    }

    public void OnAdd()
    {
        switch (Buff.BuffType)
        {
            case BuffType.HOLY_SHIELD:
                {
                    CardController.Info.Shield.SetActive(true);
                    break;
                }

            default:
                break;
        }
    }

    public void OnRemove()
    {
        switch (Buff.BuffType)
        {
            case BuffType.HOLY_SHIELD:
                {
                    if (!StatusBars.Buffs.Exists(x => x.Buff.BuffType == BuffType.HOLY_SHIELD))
                    {
                        CardController.Info.Shield.gameObject.SetActive(false);
                    }
                    break;
                }

            default:
                break;
        }
    }
}
