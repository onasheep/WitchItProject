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
        // TODO : ���� ���ҽ� ���� ���ο��� ������ �� �������� ��� "path" ���� 
        GameObject[] effectResource = Resources.LoadAll<GameObject>(RDefine.EFFECT_PATH);
        GameObject[] objResources = Resources.LoadAll<GameObject>(RDefine.OBJ_PATH);
        GameObject[] playerResources = Resources.LoadAll<GameObject>(RDefine.PLAYER_PATH);
        #region [TEST] �޾ƿ� ���ҽ����� �ѹ��� �۾����� �ٸ� Dictionary�� ������ �� 
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