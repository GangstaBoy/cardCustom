using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffFactory : MonoBehaviour
{
    public StatusBars StatusBars;

    public void Init(Buff buff, GameObject buffGameobject)
    {
        BuffBehaviour buffBehaviour = buffGameobject.GetComponent<BuffBehaviour>();
        buffBehaviour.Init(buff);
        BuffController buffController = new BuffController(buff, buffBehaviour, buffGameobject);
        StatusBars.Add(buffController);
        return;
    }

    public void RemoveBuff(BuffController buff)
    {
        StatusBars.Remove(buff);
    }

}

