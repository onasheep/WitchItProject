using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public TMP_Text roomName;
    public TMP_Text connectInfo;
    public TMP_Text msgList;

    public Button exitBtn;

    // Start is called before the first frame update
    void Awake()
    {

        ResourceManager.Init();
        //CreatePlayer();
        ////���� ���� ���� �� ǥ��
        //SetRoomInfo();
        ////EXIT ��ư �̺�Ʈ ����
        //exitBtn.onClick.AddListener(() => OnExitClick());
    }

    // Update is called once per frame
    void Update()
    {
        // { ~~~ ���� ���� / sj_h
        // } ~~~ ���� ���� / sj_h
    }

    void CreatePlayer()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);

        PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);
    }

    //�� ���� ������ ���
    void SetRoomInfo()
    {
        Room room = PhotonNetwork.CurrentRoom;
        roomName.text = room.Name;
        connectInfo.text = $"({room.PlayerCount}/{room.MaxPlayers})";
        
    }

    //exit ��ư�� onclick�� ������ �Լ�
    private void OnExitClick()
    {
        PhotonNetwork.LeaveRoom();
    }

    //���� �뿡�� �������� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    //������ ���ο� ��Ʈ��ũ ������ �������� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#00ff00>{newPlayer.NickName}</color> is joined room";
        msgList.text += msg;

    }

    //�뿡�� ��Ʈ��ũ ������ ���������� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#ff0000>{otherPlayer.NickName}</color> is left room";
        msgList.text += msg;
    }


}
