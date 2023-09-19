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
        ////���� ���� ���� �� ǥ��
        //SetRoomInfo();
        ////EXIT ��ư �̺�Ʈ ����
        //exitBtn.onClick.AddListener(() => OnExitClick());
    }

    void CreatePlayer()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = UnityEngine.Random.Range(1, points.Length);

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

    //HJ_work
    //========================================================================
    //1.����, 2. ����, 3.Ÿ�̸�   , ���� �� Ȯ��

    public bool isPlayerReady = false; //���� �÷��̾� ���� ����
    [SerializeField] private int readyCount = 0; //���� �ο� ī��Ʈ
    public bool isEveryReady = false; //��ΰ� �غ��ߴ���
    public bool isGameStart = false;
   

    [SerializeField] private Button masterStartBtn = default;

    public bool isHiding = false; //���� ���½ð�

    public bool isPlaying = false; //��� �ʿ��������?

    [SerializeField] private float timeRemaining = 240;

    [SerializeField] private TMP_Text timeText = default;

    private void Start()
    {
        masterStartBtn = GameObject.Find("TestCanvasHJ").transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
    }

    private void Update()
    {
        //HJ_
        //��� �ο��� �غ� �Ϸ��ϸ� �������� ���� ��ư�� Ȱ��ȭ �˴ϴ�.
        if (isEveryReady)
        {
            masterStartBtn.interactable = true;
        }
        else //�� ���� ��쿡�� false �� ���ѷ��� �մϴ�.
        {
            masterStartBtn.interactable = false;
        } //���� ���� Ȱ��ȭ ���� 

        if (isGameStart)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("���ӽ��� ���� �� ���ð� Ÿ�̸� �������ϴ�.");
                //�ؿ��� isHiding 15�ʷ� �ʱ�ȭ ���ֱ� �� �Ŷ� ���ֵ� �� �� ���� �մϴ� . �ϴ��� ����
                timeRemaining = 0;
                timeText.text = string.Format("00:00");
                isGameStart= false;
                //TODO ���⼭ �г� ���ְ� ĳ���� �����ָ� �� �� �����ϴ�.
                // �� ������ ������ �������� ĳ���͸� ������ �� �ְԲ� �ϸ� �� �� �����ϴ�.
                //�׽�Ʈ�̱� ������ isHiging�� true�� �ٲ��ݴϴ�.
                
                isHiding = true; 
                timeRemaining = 15f;
                return;
                //���⼭���� ���� �� ���ð� Ÿ�̸Ӱ� �����ϴ�.
            }
            Debug.Log("���ӽ��� ���� �� ���ð� Ÿ�̸� ������ ������ ����� ���� ? 1");
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
                Debug.Log("���డ ���� �ð��� �������ϴ�.");
                timeRemaining = 0f;
                timeText.text = string.Format("00:00");
                isHiding = false;
                isPlaying = true;
                //���⼭���ʹ� ���Ͱ� ã�� �ð��� ���۵˴ϴ�.
                //4���� �־����ϴ�.
                timeRemaining = 240f;
                return;
                
            }
            Debug.Log("���డ ���� �ð� = �̰� ��µǳ� 2");
            DisplayTime(timeRemaining);
        }
        else if (!isHiding && isPlaying) // ���� �ð��� ������ ������ �������̶�� 
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            //else if()// ���⼭ ������ ���� �� �� �ִ� ����� ã�Ƽ� �� ���� 0�� �Ǹ� �� �κп� �Լ��� ����ǰ� ���ָ� �� �� �����ϴ�.
            //{
                //���� �¸� UI �� ó��
            //}
            else
            {
                timeRemaining = 0;
                timeText.text = string.Format("00:00");
                Debug.Log("���Ͱ� ã�� �ð��� �������ϴ�. �׸��� ������ �¸��Դϴ�.");
                isPlaying = false;
                //���⼭ �¸� UI �˾�â �ѹ� ����ְ� 2�ʵڿ� ��� panel�� �����Բ� ���ָ� �� �� �����ϴ�.
                return;
            }
            Debug.Log("���Ͱ� ã�� �ð��� ��=���� �̰� ��µǸ� �� ���� ��������.");
            DisplayTime(timeRemaining);
        }
    }
    //HJ_
    //�ð� ǥ�� �Լ��Դϴ�.
    public void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    //HJ_ �÷��̾� �غ� bool �� �ٲ��ִ� �Լ�
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
