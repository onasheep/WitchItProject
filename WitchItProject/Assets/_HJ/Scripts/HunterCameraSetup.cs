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
        //// ���� �ڽ��� �����÷��̾���
        //if (photonView.IsMine)
        //{
        //    //���� �ִ� �ó׸ӽ� ���� ī�޶� ã��
        //    hunterCam.Follow = transform;
        //    hunterCam.LookAt = transform;
        //}
    }

    private void Start()
    {
        //witchCam.SetActive(false);
        //���� �ڽ��� �����÷��̾���
        if (!photonView.IsMine)
        {
            return;
        }
            hunterCam = GameObject.Find("HunterCamera").GetComponent<CinemachineVirtualCamera>();
            witchCam = GameObject.Find("WitchCamera");
            //   ���� �ִ� �ó׸ӽ� ���� ī�޶� ã��
            hunterCam.Follow = transform;
            hunterCam.LookAt = transform;
    }
}
