using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCameraControl : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera myCamera;
    [SerializeField] private CinemachineFramingTransposer myBody;

    [SerializeField] private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        myCamera = GetComponent<CinemachineVirtualCamera>();
        myBody =  myCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        if (GameObject.Find("DeathCamTestObj") == null)
        {
            return;
        }
        else
        {
            target = GameObject.Find("DeathCamTestObj").transform;
            myCamera.Follow = target; // 카메라가 따라다닐 타겟 지정
            myCamera.LookAt = target; // 중심으로 회전할 타겟 지정
            myBody.m_CameraDistance = 0f;

            //damping 설정
            myBody.m_XDamping = 0f;
            myBody.m_YDamping = 0f;
            myBody.m_ZDamping = 0f;
        }

    }
    private void FixedUpdate()
    {
        //if (GameObject.Find("DeathCamTestObj") == null)
        //{
        //    return;
        //}
        //else
        //{
        //    target = GameObject.Find("DeathCamTestObj").transform;
        //    myCamera.Follow = target; // 카메라가 따라다닐 타겟 지정
        //    myCamera.LookAt = target; // 중심으로 회전할 타겟 지정
        //    myBody.m_CameraDistance = 0f;

        //}
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
