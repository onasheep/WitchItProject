using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RDefine;

public static class ResourceManager
{


    // ! Enum 과 리소스 경로 매칭
    public static Dictionary<EResourceType, string> resourcesPath
     = new Dictionary<EResourceType, string>()
     {
         {EResourceType.EFFECT, EFFECT_PATH },
         {EResourceType.OBJECT, OBJ_PATH },
         {EResourceType.PLAYER, PLAYER_PATH }
     };

    public static readonly Dictionary<EResourceType, Dictionary<string, GameObject>> resources
        = new Dictionary<EResourceType, Dictionary<string, GameObject>>();

    public static void Init()
    {

        foreach (EResourceType type in Enum.GetValues(typeof(EResourceType)))
        {
            resources[type] = new Dictionary<string, GameObject>();
            AddResources(type);
        }   

    }       // Init()

    private static void AddResources(EResourceType type)
    {
        if (!resourcesPath.TryGetValue(type, out string path))
        {
            Debug.LogWarning($"{path} is not defined");
            return;
        }       // if : 경로가 잘못되었을 때 

        GameObject[] loadedResources = Resources.LoadAll<GameObject>(path);
        foreach(GameObject resource in loadedResources)
        {
            resources[type].Add(resource.name, resource);
        }
    }       // AddResources()

    
    public static GameObject GetResource(EResourceType type,string name)
    {
        if (!resources.TryGetValue(type, out Dictionary<string,GameObject> dict))
        {
            Debug.LogWarning($"Resoucre {type} is not defined");
            return null;
        }       // if : type이 존재하지 않을 떄

        if(!dict.TryGetValue(name, out GameObject resource))
        {
            Debug.LogWarning($"Resoucre {name} is not defined");
            return null;
        }       // if : name이 존재하지 않을 때
        
        return resource;
        
    }       // GetResource()
}