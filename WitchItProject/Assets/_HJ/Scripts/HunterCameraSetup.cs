using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterCameraSetup : MonoBehaviourPun
{
    CinemachineVirtualCamera hunterCam = default;
    GameObject witchCam = default;

    private void Awake()
    {
        //hunterCam = GameObject.Find("HunterCamera").GetComponent<CinemachineVirtualCamera>();
        //witchCam = GameObject.Find("WitchCamera");
        
        //witchCam.SetActive(false);
        //// 만약 자신이 로컬플레이어라면
        //if (photonView.IsMine)
        //{
        //    //씬에 있는 시네머신 가상 카메라를 찾고
        //    hunterCam.Follow = transform;
        //    hunterCam.LookAt = transform;
        //}
    }

    private void Start()
    {
        //witchCam.SetActive(false);
        //만약 자신이 로컬플레이어라면
        if (!photonView.IsMine)
        {
            return;
        }
            hunterCam = GameObject.Find("HunterCamera").GetComponent<CinemachineVirtualCamera>();
            witchCam = GameObject.Find("WitchCamera");
            //   씬에 있는 시네머신 가상 카메라를 찾고
            hunterCam.Follow = transform;
            hunterCam.LookAt = transform;
    }
}
