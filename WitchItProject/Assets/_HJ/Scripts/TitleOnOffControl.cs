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

    public bool isIntro = true;
    void Start()
    {
        introCanvas = GameObject.Find("IntroCanvas");
        // TODO Ÿ��Ʋ ȭ�� �����ؾ��մϴ� ���� ���ӿ� �°Բ�
        titleCanvas = GameObject.Find("TitleCanvas");
        introVideo = introCanvas.transform.GetChild(0).gameObject.GetComponent<VideoPlayer>();

        introVideo.loopPointReached += CheckOver;
    }

    
    void Update()
    {
        if (Input.anyKeyDown )
        {
            introCanvas.SetActive(false);
            isIntro = false;
        }
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        introCanvas.SetActive(false);
        isIntro = false;
    }
}
