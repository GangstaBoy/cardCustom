using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    public Buff Buff;
    public BuffBehaviour BuffBehaviour;
    public GameObject BuffGameobject;
    public BuffController(Buff buff, BuffBehaviour buffBehaviour, GameObject buffGameobject)
    {
        this.Buff = buff;
        this.BuffBehaviour = buffBehaviour;
        this.BuffGameobject = buffGameobject;
    }
}
