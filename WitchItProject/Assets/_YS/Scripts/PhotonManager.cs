using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string version = "1.0";

    private string userId = "Test";

    public TMP_InputField userIF;

    public TMP_InputField roomNameIF;

    //�� ��Ͽ� ���� �����͸� �����ϱ� ���� ��ųʸ� �ڷ���
    private Dictionary<string, GameObject> rooms = new Dictionary<string, GameObject>();
    //�� ����� ǥ���� ������
    private GameObject roomItemPrefab;
    //RoomItem �������� �߰��� ScrollContent
    public Transform scrollContent;

    // Start is called before the first frame update
    void Awake()
    {
        //������ Ŭ���̾�Ʈ �� �ڵ� ����ȭ �ɼ�
        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.GameVersion = version;

        roomItemPrefab = Resources.Load<GameObject>("RoomItem");

        if(PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        
    }

    void Start()
    {
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(1,21):00}");
        userIF.text = userId;

        PhotonNetwork.NickName = userId;
    }

    public void SetUserId()
    {
        if (string.IsNullOrEmpty(userIF.text))
        {
            userId = $"USER_{Random.Range(1,21):00}";
        }
        else
        {
            userId = userIF.text;
        }

        PlayerPrefs.SetString("USER_ID", userId);

        PhotonNetwork.NickName = userId;
    }

    string SetRoomName()
    {
        if(string.IsNullOrEmpty(roomNameIF.text))
        {
            roomNameIF.text = $"Room_{Random.Range(1,101):000}";
        }
        return roomNameIF.text;
    }

    //���� ������ ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master!");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby();
    }

    //�κ� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        //�������� �����ϱ� ���� �ּ�
        //PhotonNetwork.JoinRandomRoom();
    }

    //������ �� ������ �������� ��� ȣ�͵Ǵ� �ݹ� �Լ�
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRandom Failed {returnCode}:{message}");
        //���� �����ϴ� �Լ� ����
        OnMakeRoomClick();

        //���� �Ӽ� ����
        //RoomOptions ro = new RoomOptions();
        //ro.MaxPlayers = 20;
        //ro.IsOpen = true;
        //ro.IsVisible = true;

        //�� ����
        //PhotonNetwork.CreateRoom("My Room", ro);
    }

    //�� ������ �Ϸ�� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnCreatedRoom()
    {
        Debug.Log($"Created Room");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }

    //�뿡 ������ �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        foreach (var Player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{Player.Value.NickName} , {Player.Value.ActorNumber}");
        }

        //���� ��ġ ������ �迭�� ����
        //Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        //int idx = Random.Range(0, points.Length);

        //��Ʈ��ũ�� ĳ���� ����
        //PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);

        //������ Ŭ���̾�Ʈ�� ��쿡 �뿡 ������ �� ���� ���� �ε�
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("TestGameMap");
        }
        //PhotonNetwork.LoadLevel("TestGameMap");
    }

    //�� ����� �����ϴ� �ݹ� �Լ�
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //������ RoomItem �������� ������ �ӽú���
        GameObject tempRoom = null;

        foreach (var roomInfo in roomList)
        {
            if(roomInfo.RemovedFromList == true)
            {
                //��ųʸ����� �� �̸����� �˻��� ����� RoomItem �������� ����
                rooms.TryGetValue(roomInfo.Name, out tempRoom);

                //RoomItem ������ ����
                Destroy(tempRoom);

                //��ųʸ����� �ش� �� �̸��� �����͸� ����
                rooms.Remove(roomInfo.Name);
            }
            else //�� ������ ����� ���
            {
                //�� �̸��� ��ųʸ��� ���� ��� ���� �߰�
                if(rooms.ContainsKey(roomInfo.Name) == false)
                {
                    //RoomInfo �������� scrollContent ������ ����
                    GameObject roomPrefab = Instantiate(roomItemPrefab, scrollContent);
                    //�� ������ ǥ���ϱ� ���� RoomInfo ���� ����
                    roomPrefab.GetComponent<RoomData>().RoomInfo = roomInfo;

                    //��ųʸ� �ڷ����� ������ �߰�
                    rooms.Add(roomInfo.Name, roomPrefab);
                }
                else  //�� �̸��� ��ųʸ��� ���� ��쿡 �� ������ ����
                {
                    rooms.TryGetValue(roomInfo.Name, out tempRoom);
                    tempRoom.GetComponent<RoomData>().RoomInfo = roomInfo;

                }
            }
            //room.ToString();
            Debug.Log($"Room = {roomInfo.Name} ({roomInfo.PlayerCount}/{roomInfo.MaxPlayers})");
        }
    }

    #region UI_BUTTON_EVENT

    public void onLoginClick()
    {
        //������ ����
        SetUserId();

        //������ ������ ������ ����
        PhotonNetwork.JoinRandomRoom();

    }

    public void OnMakeRoomClick()
    {
        //������ ����
        SetUserId();

        //���� �Ӽ� ����
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20;
        ro.IsOpen = true;
        ro.IsVisible = true;

        //�� ����
        PhotonNetwork.CreateRoom(SetRoomName(), ro);

    }

    #endregion
}