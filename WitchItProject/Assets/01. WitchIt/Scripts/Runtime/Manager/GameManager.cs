using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviourPunCallbacks
{
    public TMP_Text roomName;
    public TMP_Text connectInfo;
    public TMP_Text msgList;

    public Button exitBtn;

    

    void Awake()
    {

        ResourceManager.Init();
        //CreatePlayer();
        ////접속 정보 추출 및 표시
        //SetRoomInfo();
        ////EXIT 버튼 이벤트 연결
        //exitBtn.onClick.AddListener(() => OnExitClick());
    }

    void CreatePlayer()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = UnityEngine.Random.Range(1, points.Length);

        PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);
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
        PhotonNetwork.LeaveRoom();
    }

    //포톤 룸에서 퇴장했을 때 호출되는 콜백 함수
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    //룸으로 새로운 네트워크 유저가 접속했을 때 호출되는 콜백 함수
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

    public bool isPlayerReady = false; //포톤 플레이어 레디 상태
    [SerializeField] private int readyCount = 0; //포톤 인원 카운트
    public bool isEveryReady = false; //모두가 준비했는지
    public bool isGameStart = false;
   

    [SerializeField] private Button masterStartBtn = default;

    public bool isHiding = false; //마녀 숨는시간

    public bool isPlaying = false; //얘는 필요없을지도?

    [SerializeField] private float timeRemaining = 240;

    [SerializeField] private TMP_Text timeText = default;

    private void Start()
    {
        masterStartBtn = GameObject.Find("TestCanvasHJ").transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
    }
    private void Update()
    {
        //HJ_
        //모든 인원이 준비를 완료하면 마스터의 시작 버튼이 활성화 됩니다.
        if (isEveryReady)
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
            Debug.Log("게임시작 시작 전 대기시간 타이머 끝나고 리턴이 여기로 오나 ? 1");
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
            Debug.Log("마녀가 숨는 시간 = 이거 출력되나 2");
            DisplayTime(timeRemaining);
        }
        else if (!isHiding && isPlaying) // 숨는 시간이 끝났고 게임을 진행중이라면 
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            //else if()// 여기서 마녀의 수를 셀 수 있는 방법을 찾아서 그 수가 0이 되면 이 부분에 함수가 실행되게 해주면 될 것 같습니다.
            //{
                //헌터 승리 UI 와 처리
            //}
            else
            {
                timeRemaining = 0;
                timeText.text = string.Format("00:00");
                Debug.Log("헌터가 찾는 시간이 끝났습니다. 그리고 마녀의 승리입니다.");
                isPlaying = false;
                //여기서 승리 UI 팝업창 한번 띄어주고 2초뒤에 결과 panel이 나오게끔 해주면 될 것 같습니다.
                return;
            }
            Debug.Log("헌터가 찾는 시간이 끝=만약 이게 출력되면 좀 문제 있을지도.");
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
    }
}
