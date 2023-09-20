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

public class GameManager : MonoBehaviourPunCallbacks
{
    public TMP_Text roomName;
    public TMP_Text connectInfo;
    public TMP_Text msgList;

    public Button exitBtn;

    
    //HJ__230919 
    //Awake 테스트를위해 아래에 생성했기에 잠시 비활성화 해둡니다.
    //void Awake()
    //{

    //    ResourceManager.Init();
    //    //CreatePlayer();
    //    ////접속 정보 추출 및 표시
    //    //SetRoomInfo();
    //    ////EXIT 버튼 이벤트 연결
    //    //exitBtn.onClick.AddListener(() => OnExitClick());
    //}

    //HJ_ 230919 변경 
    //void CreatePlayer()
    //{
    //    Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
    //    //int idx = UnityEngine.Random.Range(1, points.Length);
   
    //    //PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);
    //}

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
        PhotonNetwork.LeaveRoom();
    }

    //포톤 룸에서 퇴장했을 때 호출되는 콜백 함수
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    //룸에서 네트워크 유저가 퇴장했을때 호출되는 콜백 함수
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#00ff00>{newPlayer.NickName}</color> is joined room";
        msgList.text += msg;

        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        //int witchSpawnPoint = 1;
        int hunterSpawnPoint = 2;

        if (photonView.IsMine)
        {
            // 현재 접속한 플레이어가 로컬 플레이어인 경우에만 캐릭터를 생성합니다.
            PhotonNetwork.Instantiate(RDefine.PLAYER_HUNTER, points[hunterSpawnPoint].position, points[hunterSpawnPoint].rotation, 0);
        }
        Debug.Log("유저가 접속");
    }

    //룸에서 네트워크 유저가 퇴장했을때 호출되는 콜백 함수
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#ff0000>{otherPlayer.NickName}</color> is left room";
        msgList.text += msg;
    }

    //HJ_work
    //========================================================================
    //1.승패, 2. 진영, 3.타이머   , 마녀 수 확인
    [SerializeField] private GameObject witchPrefab;
    [SerializeField] private GameObject hunterPrefab;

    [SerializeField] private GameObject hostCanvasObj;
    [SerializeField] private GameObject clientCanvasObj;


    [SerializeField] private Button masterStartBtn = default;
    [SerializeField] private Button clientReadyBtn = default;

    public bool isPlayerReady = false;  //포톤 플레이어 레디 상태
    [SerializeField] private int playerCount = 0; //게임에 들어온 인원 
    [SerializeField] private int readyCount = 0; //포톤 인원 카운트 룸에 들어오든지 아니면 맵에 들어올때  이걸 늘려줍니다.
    public bool isEveryReady = false; //모두가 준비했는지
    public bool isGameStart = false;

    public int totalWHCount = 0; // 이 값으로 헌터가 몇명 있고 나머지 마녀로 해줌
    public int huterCount = 0; //헌터의 수 입니다.
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

        masterStartBtn = hostCanvasObj.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
        clientReadyBtn = clientCanvasObj.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
        
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        //int witchSpawnPoint = 1;
        int hunterSpawnPoint = 2;
        //if (photonView.IsMine) { }
        GameObject player = PhotonNetwork.Instantiate(RDefine.PLAYER_HUNTER, points[hunterSpawnPoint].position, points[hunterSpawnPoint].rotation, 0); //헌터 생성입니다.
        int temp = player.GetComponent<PhotonView>().ViewID;

        Debug.Log("이번 생성된 캐릭터의 아이디 : " + temp);

        //호스트라면 
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("이거 호스트일떄 실행되는거임");
            clientCanvasObj.SetActive(false);
        }
        else
        {
            Debug.Log("클라이언트일때 실행되는거임");

            hostCanvasObj.SetActive(false);
        }
        //CreatePlayer();
        ////접속 정보 추출 및 표시
        //SetRoomInfo();
        ////EXIT 버튼 이벤트 연결
        //exitBtn.onClick.AddListener(() => OnExitClick());
    }
    
    public override void OnJoinedRoom()
    {

            Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
            //int witchSpawnPoint = 1;
            int hunterSpawnPoint = 2;
            //if (photonView.IsMine) { }
            GameObject player = PhotonNetwork.Instantiate(RDefine.PLAYER_HUNTER, points[hunterSpawnPoint].position, points[hunterSpawnPoint].rotation, 0); //헌터 생성입니다.
            int temp = player.GetComponent<PhotonView>().ViewID;

            Debug.Log("이번 생성된 캐릭터의 아이디 : " + temp);

    }

    private void Update()
    {
        //HJ_
        //모든 인원이 준비를 완료하면 마스터의 시작 버튼이 활성화 됩니다.
        if (PhotonNetwork.IsMasterClient) //0920변경점 호스트일때 추가
        {
            if (isEveryReady)//&& readyCount >= playerCount )
            {
                masterStartBtn.interactable = true;
            }
            else //그 외의 경우에는 false 로 놔둘려고 합니다.
            {
                masterStartBtn.interactable = false;
            } //게임 시작 활성화 관련 
        }

        if (isGameStart)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("게임시작 시작 전 대기시간 타이머 끝났습니다.");
                //밑에서 isHiding 15초로 초기화 해주긴 할 거라 없애도 될 것 같긴 합니다 . 일단은 남김
                timeRemaining = 0;
                timeText.text = string.Format("00:00");
                isGameStart= false;
                //TODO 여기서 패널 꺼주고 캐릭터 보여주면 될 것 같습니다.
                // 그 다음에 프리즈 해제해준 캐릭터를 움직일 수 있게끔 하면 될 것 같습니다.
                //테스트이긴 하지만 isHiging를 true로 바꿔줍니다.

                isHiding = true; 
                timeRemaining = 15f;
                return;
                //여기서부터 시작 전 대기시간 타이머가 끝납니다.
            }
            //TODO 여기로 오는지 한번 디버그 찍어보기
            DisplayTime(timeRemaining);
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
            //TODO 여기로 오는지 한번 디버그 찍어보기
            DisplayTime(timeRemaining);
        }
        else if (!isHiding && isPlaying) // 숨는 시간이 끝났고 게임을 진행중이라면 
        {
            if(witchCount <= 0)// 여기서 마녀의 수를 셀 수 있는 방법을 찾아서 그 수가 0이 되면 이 부분에 함수가 실행되게 해주면 될 것 같습니다.
            {
            //헌터 승리 UI 와 처리
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
            //TODO 여기로 오는지 한번 디버그 찍어보기
            DisplayTime(timeRemaining);
        }
    }
    //HJ_
    //시간 표시 함수입니다.
    public void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    //HJ_ 플레이어 준비 bool 값 바꿔주는 함수
    [PunRPC]
    public void ReadyGame()
    {
        isPlayerReady = true;
        readyCount++;
    }

    public void PushGameStart()
    {
        isGameStart = true;
        timeRemaining = 10f;
        //TODO 들어온 인원들 진영을 선택해줘야합니다.
    }

    //HJ_ 230919 변경 
    void CreatePlayer()
    {
        //Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        //int witchSpawnPoint = 1;
        //int hunterSpawnPoint = 2;
        

        //if (PhotonNetwork.IsMasterClient) //0920변경점 호스트일때 추가
        //{
        //    //PhotonNetwork.Instantiate(RDefine.PLAYER_HUNTER, points[hunterSpawnPoint].position, points[hunterSpawnPoint].rotation, 0); //헌터 생성입니다.

        //}
        //else
        //{
        //    PhotonNetwork.Instantiate(RDefine.PLAYER_WITCH, points[witchSpawnPoint].position, points[witchSpawnPoint].rotation, 0); //마녀 생성입니다.
        //}

        //    if (UnityEngine.Random.Range(0, 10) > 1 )
        //{            
        //    PhotonNetwork.Instantiate(RDefine.PLAYER_HUNTER, points[hunterSpawnPoint].position, points[hunterSpawnPoint].rotation, 0); //헌터 생성입니다.
        //    //chooseWH = 1;            
        //}
        //else
        //{
        //    PhotonNetwork.Instantiate(RDefine.PLAYER_WITCH, points[witchSpawnPoint].position, points[witchSpawnPoint].rotation, 0); //마녀 생성입니다.
        //}
    }
}
