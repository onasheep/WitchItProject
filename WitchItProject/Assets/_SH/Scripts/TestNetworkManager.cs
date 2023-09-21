using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    void Awake()
    {
       

        //Screen.SetResolution(960, 540, false); //해상도 설정 
        //PhotonNetwork.ConnectUsingSettings();

    }
    //지금 바로 연결하게끔 해놓음 -> 콜백으로 호출되서 바로 방 만들고 하게끔 하는 로직
    //public override void OnConnectedToMaster() => PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 6 }, null);
    //public override void OnConnectedToMaster() => PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 6 }, null);


    //이 방에서 테스트를 함 
    //public override void OnJoinedRoom() 
    //{
    //    Debug.Log("처음에 들어올 생기나?");
    //    PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
    //}


     void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
    }


}
