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
       

        //Screen.SetResolution(960, 540, false); //�ػ� ���� 
        //PhotonNetwork.ConnectUsingSettings();

    }
    //���� �ٷ� �����ϰԲ� �س��� -> �ݹ����� ȣ��Ǽ� �ٷ� �� ����� �ϰԲ� �ϴ� ����
    //public override void OnConnectedToMaster() => PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 6 }, null);
    //public override void OnConnectedToMaster() => PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 6 }, null);


    //�� �濡�� �׽�Ʈ�� �� 
    //public override void OnJoinedRoom() 
    //{
    //    Debug.Log("ó���� ���� ���⳪?");
    //    PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
    //}


     void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
    }


}
