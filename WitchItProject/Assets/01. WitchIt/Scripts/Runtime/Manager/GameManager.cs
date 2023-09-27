using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;
using Photon.Pun.UtilityScripts;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviourPunCallbacks
{
    //HJ__
    public int randNum = 0;

    public TMP_Text roomName;
    public TMP_Text connectInfo;
    public TMP_Text msgList;

    public Button exitBtn;

    //HJ-----> 변수 위치 이동 0921
    //HJ_work 해야될 일
    //========================================================================
    //1.승패, 2. 진영, 마녀 수 확인
    [SerializeField] private GameObject witchPrefab;
    [SerializeField] private GameObject hunterPrefab;

    [SerializeField] private GameObject hostCanvasObj;
    [SerializeField] private GameObject clientCanvasObj;
    [SerializeField] private GameObject roomInfoCanvasObj;

    [SerializeField] private GameObject startPannel;
    [SerializeField] private GameObject readyPannel;

    [SerializeField] private Button masterStartBtn = default;
    [SerializeField] private Button clientReadyBtn = default;

    public bool isPlayerReady = false;  //포톤 플레이어 레디 상태
    [SerializeField] private int playerCount = 0; //게임에 들어온 인원 
    [SerializeField] private int readyCount = 0; //포톤 인원 카운트 룸에 들어오든지 아니면 맵에 들어올때  이걸 늘려줍니다.
    public bool isEveryReady = false; //모두가 준비했는지
    public bool isGameStart = false;

    public int totalWHCount = 0; // 이 값으로 헌터가 몇명 있고 나머지 마녀로 해줌
    public int hunterCount = 0; //헌터의 수 입니다.
    public int witchCount = 0; //마녀의 수 입니다.

    public bool isHunter = false; //헌터인지
    public bool isWitch = false; //마녀인지


    public bool isHiding = false; //마녀 숨는시간
    public bool isPlaying = false; //게임중인지


    [SerializeField] private float timeRemaining = 600;
    [SerializeField] private TMP_Text timeText = default;
    void Awake()
    {
        ResourceManager.Init();
        hostCanvasObj = GameObject.Find("TestHostCanvasHJ");
        clientCanvasObj = GameObject.Find("TestClientCanvasHJ");
        
        roomInfoCanvasObj = GameObject.Find("CanvasRoomInfo");

        startPannel = hostCanvasObj.transform.GetChild(1).gameObject;
        readyPannel = clientCanvasObj.transform.GetChild(1).gameObject;

        masterStartBtn = hostCanvasObj.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
        clientReadyBtn = clientCanvasObj.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
        
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

        

        ////접속 정보 추출 및 표시
        SetRoomInfo();
        ////EXIT 버튼 이벤트 연결
        exitBtn.onClick.AddListener(() => OnExitClick());

        // 버튼에 이벤트 연결해줍니다>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        clientReadyBtn.onClick.AddListener(() => ReadyGame());

        // 스타트 버튼에 이벤트 연결해줍니다>>>>>>>>>>>>>>>>>>>>>>>>>>
        masterStartBtn.onClick.AddListener(() => PushGameStart());
        
    }
    private void Start()
    {
        //CreateHunter();
        //CreatePlayer();
        roomInfoCanvasObj.SetActive(false);
        
        //CreateHunter();

    }
    private void Update()
    {
       
        if (isPlayerReady)
        {
            readyCount++;
            isPlayerReady = false;
        }

        
        //HJ___0924
       
        Debug.LogFormat("이건 로컬 플레이어 액터넘버임 : {0}", PhotonNetwork.LocalPlayer.ActorNumber);

        Debug.LogFormat("플레이어 리스트입니다. : {0}", PhotonNetwork.PlayerList.Length);
        Debug.LogFormat("플레이어 카운터입니다 {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        //Debug.LogFormat("플레이어 i 번째 리스트{0}", PhotonNetwork.PlayerList[2]);
        //Debug.LogFormat("플레이어 i 번째 리스트", PhotonNetwork.PlayerList[3]);


        //HJ_
        //모든 인원이 준비를 완료하면 마스터의 시작 버튼이 활성화 됩니다.
        if (PhotonNetwork.IsMasterClient) //0920변경점 호스트일때 추가
        {
            //Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount); //이거 지금 룸에 플레이어 수입니다
            clientCanvasObj.SetActive(false);
            hostCanvasObj.SetActive(true);
            if (readyCount == playerCount && playerCount != 0) //레디한 사람의 수가 
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

        //HJ LeftAlt 누르고 있을때 마우스 포인트 보이게끔
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            roomInfoCanvasObj.SetActive(true);
        }
        else 
        {
            roomInfoCanvasObj.SetActive(false);
        }

      

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
                //밑에서 isHiding 15초로 초기화 해주긴 할 거라 없애도 될 것 같긴 합니다 . 일단은 남김
                timeRemaining = 0;
                timeText.text = string.Format("00:00");
                isGameStart = false;
                //TODO 여기서 패널 꺼주고 캐릭터 보여주면 될 것 같습니다.
                // 그 다음에 프리즈 해제해준 캐릭터를 움직일 수 있게끔 하면 될 것 같습니다.
                //테스트이긴 하지만 isHiging를 true로 바꿔줍니다.

                isHiding = true;
                timeRemaining = 15f;
                RandomChoose();
                
                Debug.LogFormat("{0} <<< 랜덤 넘버", randNum);
                Debug.LogFormat("{0} <<< 액터 넘버", PhotonNetwork.LocalPlayer.ActorNumber);
                Debug.LogFormat("{0} <<< 아닌지 ", randNum != PhotonNetwork.LocalPlayer.ActorNumber);
                
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
            if (witchCount <= 0)// 여기서 마녀의 수를 셀 수 있는 방법을 찾아서 그 수가 0이 되면 이 부분에 함수가 실행되게 해주면 될 것 같습니다.
            {
                //헌터 승리 UI 와 처리
                Debug.Log("헌터승리");
                return;
            }

            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                timeText.text = string.Format("00:00");
                Debug.Log("헌터가 찾는 시간이 끝났습니다. 그리고 마녀의 승리입니다.");
                isPlaying = false;
                //여기서 승리 UI 팝업창 한번 띄어주고 2초뒤에 결과 panel이 나오게끔 해주면 될 것 같습니다.
                return;
            }
            photonView.RPC("DisplayTime", RpcTarget.All, timeRemaining);
        }
    }
    //룸 접속 정보를 출력
    void SetRoomInfo()
    {
        Room room = PhotonNetwork.CurrentRoom;
        roomName.text = room.Name;
        connectInfo.text = $"({room.PlayerCount}/{room.MaxPlayers})";

    }
    //exit 버튼의 onclick에 연결할 함수
    private void OnExitClick()
    {
        //photonView.RPC("ExitPlayer", RpcTarget.MasterClient);
        PhotonNetwork.LeaveRoom();
    }

   
    //포톤 룸에서 퇴장했을 때 호출되는 콜백 함수
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("TestGorani");
      
        //SceneManager.LoadScene("TestGorani");
    }

    //룸에서 네트워크 유저가입장했을때 호출되는 콜백 함수
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#00ff00>{newPlayer.NickName}</color> is joined room";
        msgList.text += msg;

        Debug.Log("유저가 접속");
        photonView.RPC("EnterPlayer", RpcTarget.MasterClient);
    }

    //룸에서 네트워크 유저가 퇴장했을때 호출되는 콜백 함수
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        SetRoomInfo();

        string msg = $"\n<color=#ff0000>{otherPlayer.NickName}</color> is left room";
        msgList.text += msg;

        photonView.RPC("ExitPlayer", RpcTarget.MasterClient);
    }
    
    //HJ_
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
    //230922_HJ 변경한 것
    [PunRPC]
    public void PushGameStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetStart", RpcTarget.All);
        }
    }

    [PunRPC]
    public void ChangeBool(bool test, bool test2)
    {
        test = test2;
    }

    //----------------------
    //HJ_ 230919 변경 
    void CreateHunter()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        //int witchSpawnPoint = 1;
        int hunterSpawnPoint = 2;
        // TODO : Hunter로 변경하기 
        // TEST : Witch 변신가능 오브젝트 아웃라인 테스트
        PhotonNetwork.Instantiate(RDefine.PLAYER_HUNTER2, points[hunterSpawnPoint].position, points[hunterSpawnPoint].rotation, 0); //헌터 생성입니다.
    }
    void CreateWitch()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int witchSpawnPoint = 1;
        PhotonNetwork.Instantiate(RDefine.PLAYER_WITCH, points[witchSpawnPoint].position, points[witchSpawnPoint].rotation, 0); //마녀 생성입니다.
    }

    [PunRPC]
    public void RandomNum()
    {
        randNum = UnityEngine.Random.Range(1, PhotonNetwork.CurrentRoom.PlayerCount);
    }
    //HJ++ 0924
    public void RandomChoose()
    {
        photonView.RPC("RandomNum", RpcTarget.All);
         
        if (randNum == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            CreateHunter();
        }
        else if (randNum != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            
            CreateWitch();
            photonView.RPC("AddWCount", RpcTarget.All);
        }
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
}
