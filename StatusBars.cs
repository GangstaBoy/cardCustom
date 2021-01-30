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
    public void Add(BuffController buffController, bool isBuff = true)
    {
        if (isBuff) Buffs.Add(buffController);
        else Debuffs.Add(buffController);
    }

    public void Remove(BuffController buffController, bool isBuff = true)
    {
        if (isBuff) Buffs.Remove(buffController);
        else Debuffs.Remove(buffController);
        buffController.BuffGameobject.SetActive(false);
    }
}
