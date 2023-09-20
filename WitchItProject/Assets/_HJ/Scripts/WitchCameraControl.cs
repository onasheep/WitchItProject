using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchCameraControl : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera myCamera;
    [SerializeField] private CinemachineFramingTransposer myBody;
    [SerializeField] private CinemachinePOV myAim;

    [SerializeField][Range(0f,10f)] private float defaultCameraDistance;
    [SerializeField][Range(0f, 10f)] private float maxCameraDistance = 1f;
    [SerializeField][Range(0f, 10f)] private float minCameraDistance = 8f;


    [SerializeField] private Transform target;


    // Start is called before the first frame update
    void Start()
    {
        //목표 타겟을 찾아옵니다.
        target = GameObject.Find("WitchCharacter").transform.GetChild(2).transform;
        myCamera = GetComponent<CinemachineVirtualCamera>(); //카메라 컴포넌트 받아오기
        
        myCamera.Follow = target; // 카메라가 따라다닐 타겟 지정
        myCamera.LookAt = target; // 중심으로 회전할 타겟 지정

        myBody =  myCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        myAim = myCamera.GetCinemachineComponent<CinemachinePOV>();
        
        //초기 카메라 거리 설정(일회용)
        defaultCameraDistance = 6f; 
        myBody.m_CameraDistance = defaultCameraDistance;


        //damping 설정
        myBody.m_XDamping = 0f;
        myBody.m_YDamping = 0f;
        myBody.m_ZDamping = 0f;
    }

    // Update is called once per frame
    void Update()
    {
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
