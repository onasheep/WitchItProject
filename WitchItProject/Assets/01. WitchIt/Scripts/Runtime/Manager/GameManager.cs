using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;
using Photon.Pun.UtilityScripts;
using System.Runtime.InteropServices;
using UnityEngine.Events;
using Smooth;

public class GameManager : MonoBehaviourPunCallbacks
{

    #region Varaiables

    //HJ__
    [Header("Hunter choose Standard")]
    public int randNum = 0;

    [Header("character prefab")]
    [SerializeField] private GameObject witchPrefab;
    [SerializeField] private GameObject hunterPrefab;

    [Header("Canvas Object")]
    [SerializeField] private GameObject hostCanvasObj;
    [SerializeField] private GameObject clientCanvasObj;

    [Header("start/ready Panel")]
    [SerializeField] private GameObject startPannel;
    [SerializeField] private GameObject readyPannel;

    [SerializeField] private Button masterStartBtn = default;
    [SerializeField] private Button clientReadyBtn = default;

    public bool isPlayerReady = false;  //포톤 플레이어 레디 상태
    [SerializeField] private int playerCount = 0; //게임에 들어온 인원 
    [SerializeField] private int readyCount = 0; //포톤 인원 카운트 룸에 들어오든지 아니면 맵에 들어올때  이걸 늘려줍니다.
    public bool isEveryReady = false; //모두가 준비했는지

    [Header("Witch Canvas")]
    [SerializeField] private GameObject witchCanvas;
    [SerializeField] private GameObject hpPanel;
    [SerializeField] private GameObject wSkillPanel;

    [Header("Hunter Canvas")]
    [SerializeField] private GameObject hunterCanvas;
    [SerializeField] private GameObject crossHairPanel; //hunter cross hair 패널입니다.
    [SerializeField] private GameObject hSkillPanel; 



    [Header("Game Logic")]
    public bool isGameStart = false;

    public int totalWHCount = 0; // 이 값으로 헌터가 몇명 있고 나머지 마녀로 해줌
    public int hunterCount = 0; //헌터의 수 입니다.
    public int witchCount = 0; //마녀의 수 입니다.

    public bool isHunter = false; //헌터인지
    public bool isWitch = false; //마녀인지

    public bool isHiding = false; //마녀 숨는시간
    public bool isPlaying = false; //게임중인지

    [Header("Result Panel")]
    public GameObject hunterWinPanel;
    public GameObject witchWinPanel;

    [Header("Time sec")]
    [SerializeField] private float timeRemaining = 600;
    [SerializeField] private TMP_Text timeText = default;

    [Header("Esc Menu")]
    public bool isEscape = false;
    public GameObject escMenuCanvas = default;
    [SerializeField] private GameObject escPanel = default;
    [SerializeField] private Button continueBtn = default;
    [SerializeField] private Button returnmMainMenuBtn = default;
    //[SerializeField] private Button settingsBtn = default;
    //[SerializeField] private Button inviteBtn = default;
    //[SerializeField] private Button reportBtn = default;

    //public TMP_Text roomName;
    //public TMP_Text connectInfo;
    //public TMP_Text msgList;



    #endregion


    void Awake()
    {
        #region 오브젝트 가져오기
        ResourceManager.Init();
        hostCanvasObj = GameObject.Find("TestHostCanvasHJ");
        clientCanvasObj = GameObject.Find("TestClientCanvasHJ");

        escMenuCanvas = GameObject.Find("EscMenuCanvas");
        escPanel = escMenuCanvas.transform.GetChild(0).gameObject;
        continueBtn = escPanel.transform.GetChild(0).gameObject.GetComponent<Button>();
        returnmMainMenuBtn = escPanel.transform.GetChild(4).gameObject.GetComponent<Button>();

        startPannel = hostCanvasObj.transform.GetChild(1).gameObject;
        readyPannel = clientCanvasObj.transform.GetChild(1).gameObject;

        masterStartBtn = hostCanvasObj.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
        clientReadyBtn = clientCanvasObj.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();

        witchCanvas = GameObject.Find("WitchCanvas");
        hunterCanvas = GameObject.Find("HunterCanvas");

        hpPanel = witchCanvas.transform.GetChild(1).gameObject;
        wSkillPanel = witchCanvas.transform.GetChild(2).gameObject;

        crossHairPanel = hunterCanvas.transform.GetChild(0).gameObject;
        hSkillPanel = hunterCanvas.transform.GetChild(1).gameObject;


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("이거 호스트일떄 실행되는거임");
            timeText = hostCanvasObj.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
            clientCanvasObj.SetActive(false);
        }
        else
        {
            Debug.Log("클라이언트일때 실행되는거임");
            timeText = clientCanvasObj.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
            hostCanvasObj.SetActive(false);
        }

        witchWinPanel = GameObject.Find("ResutlCanvas").transform.GetChild(0).gameObject;
        hunterWinPanel = GameObject.Find("ResutlCanvas").transform.GetChild(1).gameObject;
        #endregion
        #region 버튼에 함수 연결
        ////EXIT 버튼 이벤트 연결
        returnmMainMenuBtn.onClick.AddListener(() => OnExitClick());

        // 버튼에 이벤트 연결해줍니다>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        clientReadyBtn.onClick.AddListener(() => ReadyGame());

        // 스타트 버튼에 이벤트 연결해줍니다>>>>>>>>>>>>>>>>>>>>>>>>>>
        masterStartBtn.onClick.AddListener(() => PushGameStart());
        #endregion
    }
    private void Start()
    {
        hunterCanvas.SetActive(false);
        witchCanvas.SetActive(false);
        escPanel.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
        {
            randNum = UnityEngine.Random.Range(0, PhotonNetwork.PlayerList.Length);
            // RPC를 사용하여 다른 플레이어에게 randNum 값을 할당합니다.
            photonView.RPC("AssignRandNum", RpcTarget.AllBuffered, randNum);
        }
    }
    private void Update()
    {
        if (isPlayerReady)
        {
            readyCount++;
            isPlayerReady = false;
        }
        //모든 인원이 준비를 완료하면 마스터의 시작 버튼이 활성화 됩니다.
        OnStartButton();
        // ESC 키를 눌렀을 경우
        OnEscMenu();
        //게임 시작
        GameStart(); 
    }
    public void GameStart()
    {
        if (isGameStart)
        {
            startPannel.SetActive(false);
            readyPannel.SetActive(false);

            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                //밑에서 isHiding 15초로 초기화 해주긴 할 거라 없애도 될 것 같긴 합니다
                timeRemaining = 0;
                timeText.text = string.Format("00:00");
                isGameStart = false;
                //TODO 여기서 패널 꺼주고 캐릭터 보여주면 될 것 같습니다.
                isHiding = true;
                timeRemaining = 15f;
                RandomChoose();
                return;
                //여기서부터 시작 전 대기시간 타이머가 끝납니다.
            }
            photonView.RPC("DisplayTime", RpcTarget.All, timeRemaining);
        }
        else if (isHiding)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("마녀가 숨는 시간이 끝났습니다.");
                timeRemaining = 0f;
                timeText.text = string.Format("00:00");
                isHiding = false;
                isPlaying = true;
                //여기서부터는 헌터가 찾는 시간이 시작됩니다.
                //4분이 주어집니다.
                timeRemaining = 240f;
                return;

            }
            photonView.RPC("DisplayTime", RpcTarget.All, timeRemaining);
        }
        else if (!isHiding && isPlaying) // 숨는 시간이 끝났고 게임을 진행중이라면 
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (witchCount <= 0)// 여기서 마녀의 수를 셀 수 있는 방법을 찾아서 그 수가 0이 되면 이 부분에 함수가 실행되게 해주면 될 것 같습니다.
                {
                    //헌터 승리 UI 와 처리
                    //헌터 승리 캔버스 띄어주고 RPC로 other에게 헌터 승리 함수 원격 실행 시켜야 함 
                    //Debug.Log("헌터승리");
                    SetOver();
                    photonView.RPC("SetOver", RpcTarget.Others);
                    WinH();
                    photonView.RPC("WinH", RpcTarget.Others);
                    return;
                }
            }
            
            if (isWitch) //이게 true 되면 카운트 하나 줄여줌
            {
                photonView.RPC("DieWitch", RpcTarget.MasterClient);
                isWitch = false;
            }

            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                timeText.text = string.Format("00:00");
                if (PhotonNetwork.IsMasterClient)
                {
                    SetOver();
                    photonView.RPC("SetOver", RpcTarget.Others);
                    WinW();
                    photonView.RPC("WinW", RpcTarget.Others);
                    //UnityAction action = () =>
                    ////{
                    //SetOver();
                    //photonView.RPC("SetOver", RpcTarget.Others);
                    //};
                    //ThreadManager.instance.DoRoutine(action, 5f);

                }
                Debug.Log("헌터가 찾는 시간이 끝났습니다. 그리고 마녀의 승리입니다.");
                //isPlaying = false;
                //여기서 승리 UI 팝업창 한번 띄어주고 2초뒤에 결과 panel이 나오게끔 해주면 될 것 같습니다.
                return;
            }
            photonView.RPC("DisplayTime", RpcTarget.All, timeRemaining);
        }
    }
    public void OnStartButton()
    {
        if (PhotonNetwork.IsMasterClient) //0920변경점 호스트일때 추가
        {
            clientCanvasObj.SetActive(false);
            hostCanvasObj.SetActive(true);
            if (readyCount == PhotonNetwork.CurrentRoom.PlayerCount - 1 && PhotonNetwork.CurrentRoom.PlayerCount != 0) //레디한 사람의 수가 
            {
                isEveryReady = true;
            }

            if (isEveryReady)
            {
                masterStartBtn.interactable = true;
            }
            else //그 외의 경우에는 false 로 놔둘려고 합니다.
            {
                masterStartBtn.interactable = false;
            } //게임 시작 활성화 관련 
        }
        else
        {
            hostCanvasObj.SetActive(false);
            clientCanvasObj.SetActive(true);
        }
    }
    private void OnExitClick()
    {
        PhotonNetwork.LeaveRoom();
    }
    public void OnEscMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isEscape = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        if (isEscape)
        {
            escPanel.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isEscape = false;
            }
        }
        else
        {
            escPanel.SetActive(false);
        }
    }
    //포톤 룸에서 퇴장했을 때 호출되는 콜백 함수
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("TestGorani");
    }

    //룸에서 네트워크 유저가입장했을때 호출되는 콜백 함수
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        //SetRoomInfo();
        //string msg = $"\n<color=#00ff00>{newPlayer.NickName}</color> is joined room";
        //msgList.text += msg;
        Debug.Log("유저가 접속");
        photonView.RPC("EnterPlayer", RpcTarget.MasterClient);
    }

    //룸에서 네트워크 유저가 퇴장했을때 호출되는 콜백 함수
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        //string msg = $"\n<color=#ff0000>{otherPlayer.NickName}</color> is left room";
        //msgList.text += msg;
        photonView.RPC("ExitPlayer", RpcTarget.MasterClient);
    }

    //시간 표시 함수입니다.
    [PunRPC]
    public void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    //HJ_ 플레이어 준비 bool 값 바꿔주는 함수
    public void ReadyGame()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ChangeReady", RpcTarget.MasterClient);
        }
            //HJ__0926 변경점
            clientReadyBtn.interactable = false;
            clientReadyBtn.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = "Wait";
    }
    
    [PunRPC]
    public void ChangeReady()
    {
        Debug.Log("왜 2번찍히지?");
        isPlayerReady = true;
    }
    
    [PunRPC]
    public void SetStart()
    {
        isGameStart = true;
        timeRemaining = 10f;
        //TODO 들어온 인원들 진영을 선택해줘야합니다.
    }
    
    [PunRPC]
    public void PushGameStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetStart", RpcTarget.All);
        }
    }
    
    [PunRPC]
    public void SetOver()
    {
        isPlaying = false;
    }
    public void CreateHunter()
    {
        //Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        //int hunterSpawnPoint = 2;
        //PhotonNetwork.Instantiate(RDefine.PLAYER_HUNTER, points[hunterSpawnPoint].position, points[hunterSpawnPoint].rotation, 0); //헌터 생성입니다.

        // YS_ 230927 Merged
        //PhotonNetwork.Instantiate(RDefine.PLAYER_HUNTER2, points[hunterSpawnPoint].position, points[hunterSpawnPoint].rotation, 0); //헌터 생성입니다.

        // YS_ 230927 test
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int hunterSpawnPoint = 2;
        GameObject hunter = PhotonNetwork.Instantiate(RDefine.PLAYER_HUNTER, points[hunterSpawnPoint].position, points[hunterSpawnPoint].rotation, 0);

        //// // 컴포넌트를 활성화합니다.
        //hunter.GetComponent<PhotonTransformView>().enabled = true;
        ////Debug.Log("PhotonTransformView 활성화됨");
        //hunter.GetComponent<PhotonAnimatorView>().enabled = true;
        ////Debug.Log("PhotonAnimatorView 활성화됨");
        //hunter.GetComponent<Hunter>().enabled = true;
        //Debug.Log("스크립트 활성화됨");
        //PhotonNetwork.Instantiate(RDefine.PLAYER_HUNTER, points[hunterSpawnPoint].position, points[hunterSpawnPoint].rotation, 0); //헌터 생성입니다.
        //int witchSpawnPoint = 1;
        // TEST : Witch 변신가능 오브젝트 아웃라인 테스트

        // YS_ 230927 Merged
        //PhotonNetwork.Instantiate(RDefine.PLAYER_HUNTER2, points[hunterSpawnPoint].position, points[hunterSpawnPoint].rotation, 0); //헌터 생성입니다.
    }
    public void CreateWitch()
    {
    //    Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
    //    int witchSpawnPoint = 1;
    //    PhotonNetwork.Instantiate(RDefine.PLAYER_WITCH2, points[witchSpawnPoint].position, points[witchSpawnPoint].rotation, 0); //마녀 생성입니다.

        //_YS TEST
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int witchSpawnPoint = 1;
        GameObject witch = PhotonNetwork.Instantiate(RDefine.PLAYER_WITCH, points[witchSpawnPoint].position, points[witchSpawnPoint].rotation, 0); //마녀 생성입니다.

        //witch.GetComponent<PhotonTransformView>().enabled = true;
        ////Debug.Log("PhotonTransformView 활성화됨");
        //witch.GetComponent<PhotonAnimatorView>().enabled = true;
        ////Debug.Log("PhotonAnimatorView 활성화됨");
        //witch.GetComponent<WitchController>().enabled = true;
        //witch.GetComponent<SmoothSyncPUN2>().enabled = true;

    }

    [PunRPC]
    void WinH()
    {
        hunterWinPanel.SetActive(true);
        Debug.Log("헌터 승리");
        // SJ_ 이 기능을 하는 다른 함수에도 아래와 같이 넣으면 동작함!!
        ThreadManager.instance.DoRoutine(() => PhotonNetwork.LoadLevel("TestGameMap"), 5f);
        
    }
    
    [PunRPC]
    void WinW()
    {
        witchWinPanel.SetActive(true);
        Debug.Log("마녀 승리");
        ThreadManager.instance.DoRoutine(() => PhotonNetwork.LoadLevel("TestGameMap"), 5f);
        
    }
    
    [PunRPC]
    void AssignRandNum(int value)
    {
        randNum = value;
    }
    public void RandomChoose()
    {
        int myPlayerIndex = Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer); //각 플레이어 번호

        if (randNum == myPlayerIndex)
        { 
            CreateHunter();
            hunterCanvas.SetActive(true);
        }
        else if (randNum != myPlayerIndex)
        {
            CreateWitch();
            photonView.RPC("AddWCount", RpcTarget.MasterClient);
            witchCanvas.SetActive(true);
        }
    }
    
    [PunRPC]
    public void BoolHunter()
    {
        isHunter = true;
    }
    
    [PunRPC]
    public void AddWCount()
    {
        witchCount++;
    }

    //TODO 일단 밑에 두긴 하지만 함수 실행별로 내림차순으로 정리해놓을 필요 있음 흐름 외에 것들은 제일 하단에 두고
    [PunRPC]
    public void EnterPlayer()
    {
        playerCount++;
    }
   
    [PunRPC]
    public void ExitPlayer()
    {
        playerCount--;
    }
    
    [PunRPC]
    public void DieWitch()
    {
        witchCount--;
    }
   
    [PunRPC]
    public void SetIsWitch(bool value)
    {
        isWitch = value;
        // isWitch 값이 변경되었습니다.
    }

}
