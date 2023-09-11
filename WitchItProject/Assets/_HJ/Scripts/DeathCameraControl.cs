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
            myCamera.Follow = target; // ī�޶� ����ٴ� Ÿ�� ����
            myCamera.LookAt = target; // �߽����� ȸ���� Ÿ�� ����
            myBody.m_CameraDistance = 0f;

            //damping ����
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
        //    myCamera.Follow = target; // ī�޶� ����ٴ� Ÿ�� ����
        //    myCamera.LookAt = target; // �߽����� ȸ���� Ÿ�� ����
        //    myBody.m_CameraDistance = 0f;

        //}
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
