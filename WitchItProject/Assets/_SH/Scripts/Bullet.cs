using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rigid;

    private Vector3 fireDirection;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Invoke("DestroyThis", 3f);
    }
    private void Update()
    {
        ShootThis();
        fireDirection = transform.up;
    }
    private void ShootThis()
    {
        rigid.AddForce(fireDirection * -50);
    }

    public void DestroyThis()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        BulletPool.ReturnObject(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        DestroyThis();
    }
}
