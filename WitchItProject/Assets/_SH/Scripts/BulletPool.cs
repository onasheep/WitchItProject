using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance;

    [SerializeField]
    private GameObject bulletPrefab;

    Queue<Bullet> bulletQueue = new Queue<Bullet>();

    private void Awake()
    {
        instance = this;

        bulletPrefab = (GameObject)Resources.Load("Bullet");

        Initialize(30);
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            bulletQueue.Enqueue(CreateNewObject());
        }
    }

    private Bullet CreateNewObject()
    {
        Bullet newObj_ = Instantiate(bulletPrefab).GetComponent<Bullet>();
        newObj_.gameObject.SetActive(false);
        newObj_.transform.SetParent(transform);
        newObj_.transform.localPosition = Vector3.zero;
        return newObj_;
    }

    public static Bullet GetObject()
    {
        if (instance.bulletQueue.Count > 0)
        {
            Bullet obj_ = instance.bulletQueue.Dequeue();
            obj_.gameObject.SetActive(true);
            obj_.transform.SetParent(null);
            return obj_;
        }
        else
        {
            Bullet newObj_ = instance.CreateNewObject();
            newObj_.gameObject.SetActive(true);
            newObj_.transform.SetParent(null);
            return newObj_;
        }
    }

    public static void ReturnObject(Bullet obj_)
    {
        obj_.gameObject.SetActive(false);
        obj_.transform.SetParent(instance.transform);
        obj_.transform.localPosition = Vector3.zero;
        instance.bulletQueue.Enqueue(obj_);
    }
}
