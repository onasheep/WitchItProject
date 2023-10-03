using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using Photon.Pun;
using Photon.Realtime;
public class TitleManager : MonoBehaviourPunCallbacks
{
    [Header("IntroCanvas")]
    [SerializeField] private GameObject introCanvas;
    [SerializeField] private VideoPlayer introVideo;

    [Header("TitleCanvas")]
    [SerializeField] private GameObject titleCanvas;
    [SerializeField] private GameObject disconnectPanel;
    [SerializeField] private Button lobbyButton;
    [SerializeField] private Text buttonText = default;
    [SerializeField] private GameObject pressObj = default;

    [Header("DisconnectPanel")]
    public InputField NickNameInput;

    [Header("ETC")]
    public int pressCount = 0;
    public bool isPress = false;
    public bool isIntro = true;

    void Awake()
    {
        Screen.SetResolution(960, 540, false);
    }

    void Start()
    {
        introCanvas = GameObject.Find("IntroCanvas");
        // TODO 타이틀 화면 변경해야합니다 저희 게임에 맞게끔
        titleCanvas = GameObject.Find("TitleCanvas");
        disconnectPanel = titleCanvas.transform.GetChild(1).gameObject;
        lobbyButton = disconnectPanel.transform.GetChild(1).gameObject.GetComponent<Button>();
        
        buttonText = lobbyButton.transform.GetChild(0).gameObject.GetComponent<Text>();
        pressObj = titleCanvas.transform.GetChild(2).gameObject;
        introVideo = introCanvas.transform.GetChild(0).gameObject.GetComponent<VideoPlayer>();
        lobbyButton.onClick.AddListener(() => EnterLobby());
        disconnectPanel.SetActive(false);
        titleCanvas.SetActive(false);
        introVideo.loopPointReached += CheckOver;
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (isPress == false)
            {
                pressCount++;
                isPress = true;
                isPress = false;
            }
            introCanvas.GetComponent<Canvas>().enabled = false;
            introCanvas.transform.GetChild(0).gameObject.GetComponent<VideoPlayer>().enabled = false;
            titleCanvas.SetActive(true);
        }
        if (pressCount >= 2 || isIntro == false)
        {
            if (Input.anyKeyDown)
            {
                pressObj.SetActive(false);
                disconnectPanel.SetActive(true);
            }
        }
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        introCanvas.GetComponent<Canvas>().enabled = false;
        introCanvas.transform.GetChild(0).gameObject.GetComponent<VideoPlayer>().enabled = false;

        isIntro = false;
        titleCanvas.SetActive(true);
    }

    void EnterLobby()
    {
        lobbyButton.interactable = false; //중복 접속 막기위해서 버튼 잠시 비활성화
        PhotonNetwork.ConnectUsingSettings(); //서버 접속 시도 
        buttonText.fontSize = 50;
        buttonText.text = "서버 접속중";
    }

    public override void OnConnectedToMaster()
    {
        buttonText.fontSize = 50;
        buttonText.text = "서버 접속 완료";
        PhotonNetwork.JoinLobby();//로비에 조인
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        buttonText.fontSize = 50;
        buttonText.text = "서버 접속 실패";
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnJoinedLobby()
    {
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        SceneManager.LoadScene("LobbyScene");
    }
}
