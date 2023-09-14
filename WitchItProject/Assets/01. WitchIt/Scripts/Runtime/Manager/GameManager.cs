using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        //CreatePlayer();
        ResourceManager.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreatePlayer()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(0, points.Length);

        PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);
    }
}
