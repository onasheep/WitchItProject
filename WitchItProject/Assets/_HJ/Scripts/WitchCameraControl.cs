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
        //��ǥ Ÿ���� ã�ƿɴϴ�.
        target = GameObject.Find("WitchCharacter").transform.GetChild(2).transform;
        myCamera = GetComponent<CinemachineVirtualCamera>(); //ī�޶� ������Ʈ �޾ƿ���
        
        myCamera.Follow = target; // ī�޶� ����ٴ� Ÿ�� ����
        myCamera.LookAt = target; // �߽����� ȸ���� Ÿ�� ����

        myBody =  myCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        myAim = myCamera.GetCinemachineComponent<CinemachinePOV>();
        
        //�ʱ� ī�޶� �Ÿ� ����(��ȸ��)
        defaultCameraDistance = 6f; 
        myBody.m_CameraDistance = defaultCameraDistance;


        //damping ����
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
