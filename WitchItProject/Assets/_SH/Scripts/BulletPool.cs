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
        Bullet newObj = Instantiate(bulletPrefab).GetComponent<Bullet>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static Bullet GetObject()
    {
        if (instance.bulletQueue.Count > 0)
        {
            Bullet obj = instance.bulletQueue.Dequeue();
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(null);
            return obj;
        }
        else
        {
            Bullet newObj = instance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public static void ReturnObject(Bullet obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(instance.transform);
        instance.bulletQueue.Enqueue(obj);
    }
}
