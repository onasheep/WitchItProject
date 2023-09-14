using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceManager 
{
    public static Dictionary<string, GameObject> resources;
    public static void Init()
    {
        resources = new Dictionary<string, GameObject>();
        AddResouces();
    }

    public static void AddResouces()
    {
        // TODO : 추후 리소스 폴더 내부에서 폴더로 또 나눠지는 경우 "path" 지정 
        GameObject[] tempResource = Resources.LoadAll<GameObject>("Prefabs");

        foreach (GameObject obj in tempResource)
        {
            resources.Add(obj.name, obj);
        }

    }
}
