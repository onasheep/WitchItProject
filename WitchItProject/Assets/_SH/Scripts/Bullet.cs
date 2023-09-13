using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Update()
    {
        Invoke("DestroyThis", 3f);
    }

    private void DestroyThis()
    {
        BulletPool.ReturnObject(this);
    }
}
