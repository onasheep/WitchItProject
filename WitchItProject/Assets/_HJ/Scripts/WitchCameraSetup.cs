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

        // ���� �ڽ��� �����÷��̾���
     
            hunterCam.SetActive(false);
            //���� �ִ� �ó׸ӽ� ���� ī�޶� ã��
            witchCam.Follow = transform;
            witchCam.LookAt = transform;


            witchCam.Follow = lookPoint;
        witchCam.LookAt = lookPoint;

        

        myBody = witchCam.GetCinemachineComponent<CinemachineFramingTransposer>();

        //damping ����
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
    // ī�޶� distance ���� ���� �������ִ� ��ũ��Ʈ�Դϴ�.
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
    }//Zoom �Լ� ��
}
