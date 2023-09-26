using System;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public Enum effectName;

    private void OnEnable()
    {
        Invoke("DestroyThis", 2f);
    }

    public void DestroyThis()
    {
        ObjPool.ReturnObject(this, effectName);
    }
}
