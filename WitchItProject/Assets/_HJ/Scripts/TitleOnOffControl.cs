using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class TitleOnOffControl : MonoBehaviour
{
    [SerializeField] private GameObject introCanvas;
    [SerializeField] private GameObject titleCanvas;

    //public bool isTitle = true;

    [SerializeField] private VideoPlayer introVideo;
   
    void Start()
    {
        introCanvas = GameObject.Find("IntroCanvas");
        // TODO 타이틀 화면 변경해야합니다 저희 게임에 맞게끔
        titleCanvas = GameObject.Find("TitleCanvas");
        introVideo = introCanvas.transform.GetChild(0).gameObject.GetComponent<VideoPlayer>();

        introVideo.loopPointReached += CheckOver;
    }

    
    void Update()
    {
        if (Input.anyKeyDown )
        {
            introCanvas.SetActive(false);
            //isTitle = false;
        }

        if (Input.anyKeyDown && introCanvas.activeSelf == false ) 
        {
            //씬넘어가는 것들 추가해줘야할 것 같습니다.
        }
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        introCanvas.SetActive(false);
    }
}
