using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBars : MonoBehaviour
{
    public GameObject BuffBar;
    public GameObject DebuffBar;
    public List<BuffController> Buffs;
    public List<BuffController> Debuffs;
    public StatusBars(GameObject buffBar, GameObject debuffBar)
    {
        BuffBar = buffBar;
        DebuffBar = debuffBar;
    }
    public void InitializeStatusBars()
    {
        Buffs = new List<BuffController>();
        Debuffs = new List<BuffController>();
    }

    public void TriggerDamageDeal()
    {
        if (Buffs.Count > 0)
        {
            foreach (var buff in Buffs)
            {
                buff.OnDamageDeal();
            }
        }
        if (Debuffs.Count > 0)
        {
            foreach (var debuff in Debuffs)
            {
                debuff.OnDamageDeal();
            }
        }
    }
    public void Add(BuffController buffController, bool isBuff = true)
    {
        if (isBuff)
        {
            Buffs.Add(buffController);
            buffController.OnAdd();
        }
        else Debuffs.Add(buffController);
    }

    public void Remove(BuffController buffController, bool isBuff = true)
    {
        if (isBuff)
        {
            Buffs.Remove(buffController);
            buffController.OnRemove();
            Destroy(buffController.BuffBehaviour.gameObject);
        }
        else Debuffs.Remove(buffController);
        buffController.BuffGameobject.SetActive(false);
    }
    public void OnNewTurn()
    {
        foreach (var buff in Buffs)
        {
            buff.OnNewTurn();
        }
        foreach (var debuff in Debuffs)
        {
            debuff.OnNewTurn();
        }
    }
}
