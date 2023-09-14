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
        // TODO : ���� ���ҽ� ���� ���ο��� ������ �� �������� ��� "path" ���� 
        GameObject[] tempResource = Resources.LoadAll<GameObject>("Prefabs");

        foreach (GameObject obj in tempResource)
        {
            resources.Add(obj.name, obj);
        }

    }
}
