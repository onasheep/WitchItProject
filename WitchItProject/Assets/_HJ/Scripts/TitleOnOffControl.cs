using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class TitleOnOffControl : MonoBehaviour
{
    [SerializeField] private GameObject introCanvas;
    //public bool isTitle = true;

    [SerializeField] private VideoPlayer introVideo;
   
    void Start()
    {
        introCanvas = GameObject.Find("IntroCanvas");
        introVideo = introCanvas.transform.GetChild(0).gameObject.GetComponent<VideoPlayer>();
    }

    
    void Update()
    {
        if (Input.anyKeyDown && introVideo.isPlaying)
        {
            introCanvas.SetActive(false);
            //isTitle = false;
        }

       

    }
}
