using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("DestroyThis", 2f);
    }

    public void DestroyThis()
    {
        BulletPool.ReturnObject(this);
    }
}
