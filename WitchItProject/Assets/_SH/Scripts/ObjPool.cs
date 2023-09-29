using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjPool : MonoBehaviour
{
    public static ObjPool instance;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private GameObject hitPrefab;
    [SerializeField]
    private GameObject footfallPrefab;
    [SerializeField]
    private GameObject metamorPrefab;
    [SerializeField]
    private GameObject possesPrefab;

    Queue<Bullet> bulletQueue = new Queue<Bullet>();

    Queue<Effect> hitQueue = new Queue<Effect>();
    Queue<Effect> footfallQueue = new Queue<Effect>();
    Queue<Effect> metamorQueue = new Queue<Effect>();
    Queue<Effect> possesQueue = new Queue<Effect>();

    public enum EffectNames
    {
        Hit,
        Footfall,
        Metamor,
        Posses
    }

    private void Awake()
    {
        instance = this;

        //bulletPrefab = (GameObject)Resources.Load("Bullet");
        bulletPrefab = (GameObject)Resources.Load("Projectile");

        hitPrefab = (GameObject)Resources.Load("Hit");
        footfallPrefab = (GameObject)Resources.Load("Footfall");
        metamorPrefab = (GameObject)Resources.Load("Metamorphosis");
        possesPrefab = (GameObject)Resources.Load("Possesion");

        Initialize(10);
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            bulletQueue.Enqueue(CreateNewBullet());

            hitQueue.Enqueue(CreateNewEffect(EffectNames.Hit));
            footfallQueue.Enqueue(CreateNewEffect(EffectNames.Footfall));
            metamorQueue.Enqueue(CreateNewEffect(EffectNames.Metamor));
            possesQueue.Enqueue(CreateNewEffect(EffectNames.Posses));
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

    private Effect CreateNewEffect(Enum effectName_)
    {
        Effect newObj_;

        switch (effectName_)
        {
            case EffectNames.Hit:
                newObj_ = Instantiate(hitPrefab).GetComponent<Effect>();
                break;

            case EffectNames.Footfall:
                newObj_ = Instantiate(footfallPrefab).GetComponent<Effect>();
                break;

            case EffectNames.Metamor:
                newObj_ = Instantiate(metamorPrefab).GetComponent<Effect>();
                break;

            case EffectNames.Posses:
                newObj_ = Instantiate(possesPrefab).GetComponent<Effect>();
                break;

            default:
                newObj_ = null;
                break;
        }

        newObj_.effectName = effectName_;

        newObj_.gameObject.SetActive(false);
        newObj_.transform.SetParent(transform);
        newObj_.transform.localPosition = Vector3.zero;

        return newObj_;
    }
    public static Effect GetEffect(Enum effectName_)
    {
        switch (effectName_)
        {
            case EffectNames.Hit:
                if (instance.hitQueue.Count > 0)
                {
                    Effect obj_ = instance.hitQueue.Dequeue();
                    obj_.gameObject.SetActive(true);
                    obj_.transform.SetParent(null);
                    return obj_;
                }
                else
                {
                    Effect newObj_ = instance.CreateNewEffect(EffectNames.Hit);
                    newObj_.gameObject.SetActive(true);
                    newObj_.transform.SetParent(null);
                    return newObj_;
                }

            case EffectNames.Footfall:
                if (instance.footfallQueue.Count > 0)
                {
                    Effect obj_ = instance.footfallQueue.Dequeue();
                    obj_.gameObject.SetActive(true);
                    obj_.transform.SetParent(null);
                    return obj_;
                }
                else
                {
                    Effect newObj_ = instance.CreateNewEffect(EffectNames.Footfall);
                    newObj_.gameObject.SetActive(true);
                    newObj_.transform.SetParent(null);
                    return newObj_;
                }

            case EffectNames.Metamor:
                if (instance.metamorQueue.Count > 0)
                {
                    Effect obj_ = instance.metamorQueue.Dequeue();
                    obj_.gameObject.SetActive(true);
                    obj_.transform.SetParent(null);
                    return obj_;
                }
                else
                {
                    Effect newObj_ = instance.CreateNewEffect(EffectNames.Metamor);
                    newObj_.gameObject.SetActive(true);
                    newObj_.transform.SetParent(null);
                    return newObj_;
                }

            case EffectNames.Posses:
                if (instance.possesQueue.Count > 0)
                {
                    Effect obj_ = instance.possesQueue.Dequeue();
                    obj_.gameObject.SetActive(true);
                    obj_.transform.SetParent(null);
                    return obj_;
                }
                else
                {
                    Effect newObj_ = instance.CreateNewEffect(EffectNames.Posses);
                    newObj_.gameObject.SetActive(true);
                    newObj_.transform.SetParent(null);
                    return newObj_;
                }

            default:
                return null;
        }
    }
    public static void ReturnObject(Effect obj_, Enum effectName_)
    {
        obj_.gameObject.SetActive(false);
        obj_.transform.SetParent(instance.transform);
        obj_.transform.localPosition = Vector3.zero;

        switch (effectName_)
        {
            case EffectNames.Hit:
                instance.hitQueue.Enqueue(obj_);
                break;

            case EffectNames.Footfall:
                instance.footfallQueue.Enqueue(obj_);
                break;

            case EffectNames.Metamor:
                instance.metamorQueue.Enqueue(obj_);
                break;
                
            case EffectNames.Posses:
                instance.possesQueue.Enqueue(obj_);
                break;

            default:
                break;
        }
    }
}
