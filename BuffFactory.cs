using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffFactory : MonoBehaviour
{
    public StatusBars StatusBars;

    public void Init(Buff buff, GameObject baseGameObject, int? buffValue = null, bool isCard = true)
    {
        var existingBuff = StatusBars.Buffs.Find(x => x.Buff.Name == buff.Name);
        if (existingBuff != null)
        {
            int? addedValue = buffValue == null ? buff.BuffValue : buffValue;
            if (addedValue == null) addedValue = 1;     // todo: strange

            existingBuff.Buff.BuffValue += addedValue;
        }
        else
        {
            GameObject buffGameobject = Instantiate(baseGameObject, StatusBars.BuffBar.transform, false);
            BuffBehaviour buffBehaviour = buffGameobject.GetComponent<BuffBehaviour>();
            buffBehaviour.Init(buff);
            BuffController buffController = new BuffController(buff, buffBehaviour, buffGameobject, buffValue, isCard);
            StatusBars.Add(buffController);
        }
        return;
    }

    public void RemoveBuff(BuffController buff)
    {
        StatusBars.Remove(buff);
    }

}

