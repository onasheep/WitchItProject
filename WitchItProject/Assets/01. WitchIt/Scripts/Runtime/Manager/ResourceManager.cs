using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceManager
{

    public static Dictionary<string, GameObject> objs;
    public static Dictionary<string, GameObject> effects;
    public static Dictionary<string, GameObject> players;
    public static void Init()
    {
        objs = new Dictionary<string, GameObject>();
        effects = new Dictionary<string, GameObject>();
        players = new Dictionary<string, GameObject>();

        AddResouces();
    }

    public static void AddResouces()
    {
        // TODO : 추후 리소스 폴더 내부에서 폴더로 또 나눠지는 경우 "path" 지정 
        GameObject[] effectResource = Resources.LoadAll<GameObject>(RDefine.EFFECT_PATH);
        GameObject[] objResources = Resources.LoadAll<GameObject>(RDefine.OBJ_PATH);
        GameObject[] playerResources = Resources.LoadAll<GameObject>(RDefine.PLAYER_PATH);
        #region [TEST] 받아온 리소스들을 한번의 작업으로 다른 Dictionary로 넣으려 함 
        // TEST : 
        //List<GameObject[]> resourcesList = new List<GameObject[]>();

        //resourcesList.Add(objResources);
        //resourcesList.Add(effectResource);

        //foreach (GameObject[] resources in resourcesList)
        //{
        //    foreach (GameObject resource in resources)
        //    {
        //        effects.Add(resource.name, resource);
        //    }
        //}
        // TEST :
        #endregion

        foreach (GameObject resource in effectResource)
        {
            effects.Add(resource.name, resource);
        }

        foreach (GameObject resource in objResources)
        {
            objs.Add(resource.name, resource);
        }

        foreach (GameObject resource in  playerResources)
        {
            players.Add(resource.name, resource);
        }

    }
}