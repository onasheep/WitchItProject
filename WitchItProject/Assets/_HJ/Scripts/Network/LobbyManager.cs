using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using ExitGames.Client.Photon;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("LobbyPanel")]
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private Button playBtn;
    //�ʿ�� �ּ� �����ؼ� �� �� �ֽ��ϴ�. �� ��ư
    //[SerializeField] private Button collectionBtn;
    //[SerializeField] private Button settingBtn;
    [SerializeField] private Button exitBtn;

    [Header("PlayPanel")]
    [SerializeField] private GameObject playPanel;
    [SerializeField] private Button joinRoomBtn;
    [SerializeField] private Button randMatchBtn;
    [SerializeField] private Button creatMatchBtn;
    [SerializeField] private Button backBtn;

    [Header("RoomPanel")]
    [SerializeField] private GameObject roomPanel;
    public Button[] CellBtn;
    [SerializeField] private Button PreviousBtn;
    [SerializeField] private Button NextBtn;

    [Header("InRoomPanel")]
    [SerializeField] private GameObject inRoomPanel;
    [SerializeField] private Text listText;
    [SerializeField] private Text roomInfoText;
    public Text[] chatText;
    public InputField chatInput;
    [SerializeField] private Button sendBtn;
    [SerializeField] private Button leaveRoomBtn;
    [SerializeField] private Button startButton;
    [SerializeField] private Text startBtnText;

    [Header("CreateinRoomPanel")]
    [SerializeField] private GameObject createRoomPanel;
    public InputField RoomInput;
    [SerializeField] private Button createBtn;
    [SerializeField] private Button createBackBtn;

    [Header("ETC")]
    [SerializeField] private PhotonView PV;
    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;

   


    #region �渮��Ʈ ����
    //< ��ư - 2 , > ��ư -1 , �� ����

    public void MyListClick(int num)
    {
        if (num == -2)
        {
            --currentPage;
        }
        else if (num == -1)
        {
            ++currentPage;
        }
        else
        {
            PhotonNetwork.JoinRoom(myList[multiple + num].Name);
            MyListRenewal();
        }
    }
    void MyListRenewal()
    {
        //�ִ� ������
        maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

        // ����, ������ư
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // �������� �´� ����Ʈ ����
        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
            CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
        }
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }
        MyListRenewal();
    }
    #endregion
    void Awake()
    {
        //HJ_____LobbyPanel
        //PhotonNetwork.AutomaticallySyncScene = true; //�� ����ȭ 
        GameObject lobbyCanvas = GameObject.Find("LobbyCanvas");
        lobbyPanel = lobbyCanvas.transform.GetChild(0).gameObject;
        playBtn = lobbyPanel.transform.GetChild(0).GetComponent<Button>();
        //collectionBtn = lobbyPanel.transform.GetChild(1).GetComponent<Button>();
        //settingBtn =lobbyPanel.transform.GetChild(2).GetComponent<Button>();
        exitBtn = lobbyPanel.transform.GetChild(3).GetComponent<Button>();
        //������� ==================LobbyPanel

        //HJ_____PlayPanel
        playPanel = lobbyCanvas.transform.GetChild(1).gameObject;
        joinRoomBtn = playPanel.transform.GetChild(0).GetComponent<Button>();
        randMatchBtn = playPanel.transform.GetChild(1).GetComponent<Button>();
        creatMatchBtn = playPanel.transform.GetChild(2).GetComponent<Button>();
        backBtn = playPanel.transform.GetChild(3).GetComponent<Button>();
        //������� ================PlayPanel

        //HJ_____RoomPanel
        roomPanel = lobbyCanvas.transform.GetChild(2).gameObject;
        //������� RoomPanel

        //HJ_____inRoomPanel
        inRoomPanel = lobbyCanvas.transform.GetChild(3).gameObject;
        listText = inRoomPanel.transform.GetChild(0).GetComponent<Text>();
        roomInfoText = inRoomPanel.transform.GetChild(1).GetComponent<Text>();
        sendBtn = inRoomPanel.transform.GetChild(4).GetComponent<Button>();
        leaveRoomBtn = inRoomPanel.transform.GetChild(5).GetComponent<Button>();
        startButton = inRoomPanel.transform.GetChild(6).GetComponent<Button>();
        startBtnText = startButton.transform.GetChild(0).GetComponent<Text>();
        //������� ===============inRoomPanel

        createRoomPanel = lobbyCanvas.transform.GetChild(4).gameObject;
        createBtn = createRoomPanel.transform.GetChild(1).GetComponent<Button>();
        createBackBtn = createRoomPanel.transform.GetChild(2).GetComponent<Button>();
    }

    public void Start()
    {
        playBtn.onClick.AddListener(() => PushPlayBtn());
        exitBtn.onClick.AddListener(() => EndGame());

        joinRoomBtn.onClick.AddListener(() => PushJoinRoom());
        randMatchBtn.onClick.AddListener(() => JoinRandomRoom());
        creatMatchBtn.onClick.AddListener(() => PushCreateRoom());
        backBtn.onClick.AddListener(() => ReturnLobby());

        leaveRoomBtn.onClick.AddListener(() => LeaveRoom());

        startButton.onClick.AddListener(() => StartGame());

        createBtn.onClick.AddListener(() => CreateRoom());
        createBackBtn.onClick.AddListener(() => BackCreateRoom());

        sendBtn.onClick.AddListener(() => Send());

        playPanel.SetActive(false);
        roomPanel.SetActive(false);
        inRoomPanel.SetActive(false);
        createRoomPanel.SetActive(false);
    }
    void Update()
    {
        if(roomPanel.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            roomPanel.SetActive(false);
            playPanel.SetActive(true);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("LobbyScene");
    }

    #region ��
    public void CreateRoom()
    {
        if (playPanel.activeSelf == true)
        {
            playPanel.SetActive(false);
        }
        createRoomPanel.SetActive(false);
        PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Room" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 4 });
    }

    public void JoinRandomRoom()
    {
        if (roomPanel.activeSelf==true)
        {
            roomPanel.SetActive(false);
        }
        playPanel.SetActive(false);
        PhotonNetwork.JoinRandomRoom();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = false; //�� ����ȭ 

        roomPanel.SetActive(false);
        inRoomPanel.SetActive(false);
        playPanel.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true; //�� ����ȭ 
        if (roomPanel.activeSelf == true)
        {
            roomPanel.SetActive(false);
        }
        inRoomPanel.SetActive(true);
        RoomRenewal();
        chatInput.text = "";
        for (int i = 0; i < chatText.Length; i++) chatText[i].text = "";
        if (!PhotonNetwork.IsMasterClient)
        {
            startButton.interactable = false;
            startBtnText.text = "wait for start";
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }

    public override void OnJoinRandomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        JoinRoomRenewal();
        RoomRenewal();
        ChatRPC("<color=yellow>" + newPlayer.NickName + "���� �����ϼ̽��ϴ�</color>");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        JoinRoomRenewal();
        RoomRenewal();
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "���� �����ϼ̽��ϴ�</color>");
    }

    void RoomRenewal()
    {
        listText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            listText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");
        roomInfoText.text =string.Format("{0}/{1}��{2}�ִ�", PhotonNetwork.CurrentRoom.Name, PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers);//  + " / " +  + "�� / " +  + "�ִ�";
    }
    void JoinRoomRenewal()
    {
        listText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            listText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");
    }
    #endregion

    #region ä��
    public void Send()
    {
        PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + chatInput.text);
        chatInput.text = "";
    }

    [PunRPC] // RPC�� �÷��̾ �����ִ� �� ��� �ο����� �����Ѵ�
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < chatText.Length; i++)
            if (chatText[i].text == "")
            {
                isInput = true;
                chatText[i].text = msg;
                break;
            }
        if (!isInput) // ������ ��ĭ�� ���� �ø�
        {
            for (int i = 1; i < chatText.Length; i++) chatText[i - 1].text = chatText[i].text;
            chatText[chatText.Length - 1].text = msg;
        }
    }
    #endregion

    #region myFuntion
    public void PushPlayBtn()
    {
        lobbyPanel.SetActive(false);
        playPanel.SetActive(true);
    }

    public void PushJoinRoom()
    {
        playPanel.SetActive(false);
        roomPanel.SetActive(true);
        JoinRoomRenewal();
    }
    public void PushCreateRoom()
    {
        playPanel.SetActive(false);
        createRoomPanel.SetActive(true);
    }
    public void ReturnLobby()
    {
        playPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }
    public void BackCreateRoom()
    {
        createRoomPanel.SetActive(false);
        playPanel.SetActive(true);
    }
    public void StartGame()
    {
        PhotonNetwork.LoadLevel("TestGameMap");
    }
    public void EndGame()
    {
        Application.Quit();
    }
    #endregion
}

