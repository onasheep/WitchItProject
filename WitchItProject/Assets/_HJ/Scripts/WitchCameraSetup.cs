using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchCameraSetup : MonoBehaviourPun
{
    CinemachineVirtualCamera witchCam = default;
    [SerializeField] private CinemachineFramingTransposer myBody = default;
    GameObject hunterCam = default;
    Transform lookPoint = default;
    [SerializeField][Range(0f, 10f)] private float defaultCameraDistance;
    [SerializeField][Range(0f, 10f)] private float maxCameraDistance = 1f;
    [SerializeField][Range(0f, 10f)] private float minCameraDistance = 8f;
    private void Start()
    {
        if(photonView.IsMine) 
        {
            return;
        }
        hunterCam = GameObject.Find("HunterCamera");
        witchCam = GameObject.Find("WitchCamera").GetComponent<CinemachineVirtualCamera>();

        //if(photonView.IsMine)
        //{
        //    lookPoint = GameObject.Find("CameraLookPoint").transform;
        //}

        // SJ_ 230925
        lookPoint = this.gameObject.FindChildObj("CameraLookPoint").GetComponent<Transform>();

        // 만약 자신이 로컬플레이어라면
     
            hunterCam.SetActive(false);
            //씬에 있는 시네머신 가상 카메라를 찾고
            witchCam.Follow = transform;
            witchCam.LookAt = transform;


            witchCam.Follow = lookPoint;
        witchCam.LookAt = lookPoint;

        

        myBody = witchCam.GetCinemachineComponent<CinemachineFramingTransposer>();

        //damping 설정
        myBody.m_XDamping = 0f;
        myBody.m_YDamping = 0f;
        myBody.m_ZDamping = 0f;
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        Zoom();
    }
    //HJ_
    // 카메라 distance 값을 직접 조절해주는 스크립트입니다.
    void Zoom()
    {
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (wheelInput > 0f)
        {
            myBody.m_CameraDistance -= 0.1f;
            if (myBody.m_CameraDistance < minCameraDistance)
            {
                myBody.m_CameraDistance = minCameraDistance;
            }
        }
        else if (wheelInput < 0f)
        {
            myBody.m_CameraDistance += 0.1f;
            if (myBody.m_CameraDistance > maxCameraDistance)
            {
                myBody.m_CameraDistance = maxCameraDistance;
            }
        }
    }//Zoom 함수 끝
}
