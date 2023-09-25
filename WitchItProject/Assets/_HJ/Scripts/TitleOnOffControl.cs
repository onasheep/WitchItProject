using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


public class TitleOnOffControl : MonoBehaviour
{
    [SerializeField] private GameObject introCanvas;
    [SerializeField] private GameObject titleCanvas;
    [SerializeField] private VideoPlayer introVideo;

    public int pressCount = 0;

    public  bool isPress = false;
    public bool isIntro = true;

    void Start()
    {
        introCanvas = GameObject.Find("IntroCanvas");
        // TODO 타이틀 화면 변경해야합니다 저희 게임에 맞게끔
        titleCanvas = GameObject.Find("TitleCanvas");
        introVideo = introCanvas.transform.GetChild(0).gameObject.GetComponent<VideoPlayer>();
        titleCanvas.SetActive(false);
        introVideo.loopPointReached += CheckOver;
    }

    
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if(isPress == false)
            {
                pressCount++;
                isPress = true;
                isPress= false;
            }
            //introCanvas.SetActive(false);
            introCanvas.GetComponent<Canvas>().enabled = false;
            introCanvas.transform.GetChild(0).gameObject.GetComponent<VideoPlayer>().enabled = false;
            //isIntro = false;
            titleCanvas.SetActive(true);
        }
        
        if(pressCount >= 2 || isIntro == false)
        {
            if(Input.anyKeyDown)
            {
                SceneManager.LoadScene("LobbyScene");
            }
        }
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        //introCanvas.SetActive(false);
        introCanvas.GetComponent<Canvas>().enabled = false;
        introCanvas.transform.GetChild(0).gameObject.GetComponent<VideoPlayer>().enabled = false;

        isIntro = false;
        titleCanvas.SetActive(true);
    }
}
