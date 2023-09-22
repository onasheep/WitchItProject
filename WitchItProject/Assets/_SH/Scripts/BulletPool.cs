using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance;

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject hitPrefab;
    [SerializeField]
    private GameObject footfallPrefab;

    Queue<Bullet> bulletQueue = new Queue<Bullet>();
    Queue<Effect> hitQueue = new Queue<Effect>();
    Queue<Effect> footfallQueue = new Queue<Effect>();

    private void Awake()
    {
        instance = this;

        //bulletPrefab = (GameObject)Resources.Load("Bullet");
        bulletPrefab = (GameObject)Resources.Load("Projectile");
        hitPrefab = (GameObject)Resources.Load("Hit");
        footfallPrefab = (GameObject)Resources.Load("Footfall");

        Initialize(30);
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            bulletQueue.Enqueue(CreateNewBullet());
            hitQueue.Enqueue(CreateNewEffect(hitPrefab));
            footfallQueue.Enqueue(CreateNewEffect(footfallPrefab));
        }
    }

    private Bullet CreateNewBullet()
    {
        Bullet newObj_ = Instantiate(bulletPrefab).GetComponent<Bullet>();
        newObj_.gameObject.SetActive(false);
        newObj_.transform.SetParent(transform);
        newObj_.transform.localPosition = Vector3.zero;
        return newObj_;
    }
    public static Bullet GetBullet()
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
            Bullet newObj_ = instance.CreateNewBullet();
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


    private Effect CreateNewEffect(GameObject targetPrefab_)
    {
        Effect newObj_ = Instantiate(targetPrefab_).GetComponent<Effect>();
        newObj_.gameObject.SetActive(false);
        newObj_.transform.SetParent(transform);
        newObj_.transform.localPosition = Vector3.zero;
        return newObj_;
    }
    public static Effect GetEffect(GameObject targetPrefab_)
    {
        if (targetPrefab_ == instance.hitPrefab)
        {
            if (instance.hitQueue.Count > 0)
            {
                Effect obj_ = instance.hitQueue.Dequeue();
                obj_.gameObject.SetActive(true);
                obj_.transform.SetParent(null);
                return obj_;
            }
            else
            {
                Effect newObj_ = instance.CreateNewEffect(targetPrefab_);
                newObj_.gameObject.SetActive(true);
                newObj_.transform.SetParent(null);
                return newObj_;
            }
        }
        else if (targetPrefab_ == instance.footfallPrefab)
        {
            if (instance.footfallQueue.Count > 0)
            {
                Effect obj_ = instance.footfallQueue.Dequeue();
                obj_.gameObject.SetActive(true);
                obj_.transform.SetParent(null);
                return obj_;
            }
            else
            {
                Effect newObj_ = instance.CreateNewEffect(targetPrefab_);
                newObj_.gameObject.SetActive(true);
                newObj_.transform.SetParent(null);
                return newObj_;
            }
        }
        else
        {
            return null;
        }
    }
    public static void ReturnObject(Effect obj_)
    {
        if (obj_.gameObject == instance.hitPrefab)
        {
            obj_.gameObject.SetActive(false);
            obj_.transform.SetParent(instance.transform);
            obj_.transform.localPosition = Vector3.zero;
            instance.hitQueue.Enqueue(obj_);
        }
        else if (obj_.gameObject == instance.footfallPrefab)
        {
            obj_.gameObject.SetActive(false);
            obj_.transform.SetParent(instance.transform);
            obj_.transform.localPosition = Vector3.zero;
            instance.footfallQueue.Enqueue(obj_);
        }
    }

    public static GameObject GetPrefab(int idx_)
    {
        switch (idx_)
        {
            case 0:
                return instance.hitPrefab;
            case 1:
                return instance.footfallPrefab;
            default:
                return null;
        }
    }
}
