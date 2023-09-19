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


    public bool isPlayerReady = false;  //포톤 플레이어 레디 상태
    [SerializeField] private int playerCount = 0; //게임에 들어온 인원 
    [SerializeField] private int readyCount = 0; //포톤 인원 카운트 룸에 들어오든지 아니면 맵에 들어올때  이걸 늘려줍니다.
    public bool isEveryReady = false; //모두가 준비했는지
    public bool isGameStart = false;

    public int chooseWH = 0; // 이 값으로 헌터가 몇명 있고 나머지 마녀로 해줌
    public int huterCount = 0; //헌터의 수 입니다.

    [SerializeField] private Button masterStartBtn = default;

    public bool isHiding = false; //마녀 숨는시간
    public bool isPlaying = false; //게임중인지

    public int witchCount = 0; //마녀의 수 입니다.

    [SerializeField] private float timeRemaining = 600;
    [SerializeField] private TMP_Text timeText = default;

    Hashtable initialProps = new Hashtable() { { "isHunter", false } };
    //PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);

    void Awake()
    {
        ResourceManager.Init();
        //CreatePlayer();
        ////접속 정보 추출 및 표시
        //SetRoomInfo();
        ////EXIT 버튼 이벤트 연결
        //exitBtn.onClick.AddListener(() => OnExitClick());
    }
    
    private void Start()
    {
       
        CreatePlayer();
        masterStartBtn = GameObject.Find("TestCanvasHJ").transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();

    }
    private void Update()
    {
        //HJ_
        //모든 인원이 준비를 완료하면 마스터의 시작 버튼이 활성화 됩니다.
        if (isEveryReady)//&& readyCount >= maxPlayerCount )
        {
            masterStartBtn.interactable = true;
        }
        else //그 외의 경우에는 false 로 놔둘려고 합니다.
        {
            masterStartBtn.interactable = false;
        } //게임 시작 활성화 관련 

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
        //진영 선택하는 함수를 골라줍니다.
        //ChooseRandomTeam();
        //TODO 혹은 애초에 처음부터 진영을 골라주면 이건 안해줘도 됩니다.
    }

    //HJ_ 230919 변경 
    [PunRPC]
    void CreatePlayer()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int witchSpawnPoint = 1;
        int hunterSpawnPoint = 2;

        
        if (UnityEngine.Random.Range(0, 10) > 1 && chooseWH != 1)
        {
            PhotonNetwork.Instantiate(hunterPrefab.name, points[hunterSpawnPoint].position, points[hunterSpawnPoint].rotation, 0); //헌터 생성입니다.
            //chooseWH = 1;
            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
        }
        else
        {
            PhotonNetwork.Instantiate(witchPrefab.name, points[witchSpawnPoint].position, points[witchSpawnPoint].rotation, 0); //마녀 생성입니다.
        }
        
    }
   


    //public void ChooseRandomTeam()
    //{
    //    for(int i = 0; i < 5/*게임 최대 참여 인원까지 돌려서 해줍니다. */; i++)
    //    {
    //        chooseWH = UnityEngine.Random.Range(0, 1);
    //        if(chooseWH == 1)
    //        {
    //            //헌터 프리팹을 선택해서 건네줍니다.
    //            //그리고 나머지 다른 애들한테는 마녀 프리팹을 건네줍니다.
    //            break;
    //        }
    //        else
    //        {
    //            //마녀 프리팹을 건네줍니다.
    //        }
    //    }
    //    //TODO 각각의 포톤 뷰가 달려있는 캐릭터에게 값을 전달해줍니다.
    //}
}
