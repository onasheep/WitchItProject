using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchCameraControl : MonoBehaviourPun
{
    [SerializeField] private CinemachineVirtualCamera myCamera;
    [SerializeField] private CinemachineFramingTransposer myBody;
    [SerializeField] private CinemachinePOV myAim;
    [SerializeField] private CinemachineCollider myColl;

    [SerializeField][Range(0f, 10f)] private float defaultCameraDistance;
    [SerializeField][Range(0f, 10f)] private float maxCameraDistance = 1f;
    [SerializeField][Range(0f, 10f)] private float minCameraDistance = 8f;


    [SerializeField] private Transform target;


    // Start is called before the first frame update
    void OnEnable()
    {
        //��ǥ Ÿ���� ã�ƿɴϴ�.
        // 9/25 Jung 페퍼로니 제거
        if (GameObject.Find("CameraLookPoint") == null)
        {
            return;
        }
        // 9/25 Jung 페퍼로니 제거

        target = GameObject.Find("CameraLookPoint").transform;
        myCamera = GetComponent<CinemachineVirtualCamera>(); //ī�޶� ������Ʈ �޾ƿ���

        myCamera.Follow = target; // ī�޶� ����ٴ� Ÿ�� ����
        myCamera.LookAt = target; // �߽����� ȸ���� Ÿ�� ����

        //myCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = Vector3.zero;
        myBody = myCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        myAim = myCamera.GetCinemachineComponent<CinemachinePOV>();
        //myColl = myCamera.GetComponent<CinemachineCollider>();

        //�ʱ� ī�޶� �Ÿ� ����(��ȸ��)
        defaultCameraDistance = 6f;
        myBody.m_CameraDistance = defaultCameraDistance;

        //damping ����
        myBody.m_XDamping = 0f;
        myBody.m_YDamping = 0f;
        myBody.m_ZDamping = 0f;

        // 9/22 Jung
        // �����̰� inspectorâ���� ������ ���� ����
        //myAim.m_VerticalAxis.m_MaxSpeed = 1f;
        //myAim.m_VerticalAxis.m_SpeedMode = AxisState.SpeedMode.InputValueGain;

        //myAim.m_VerticalAxis.m_MaxValue = 90f;
        //myAim.m_VerticalAxis.m_MinValue = -9f;

        //myAim.m_HorizontalAxis.m_MaxSpeed = 2f;
        //myAim.m_HorizontalAxis.m_SpeedMode = AxisState.SpeedMode.InputValueGain;

        //// Default, Ignore Raycast, Water, Ground, Wall, Board
        //string[] layers = { "Default", "Ignore Raycast", "Water", "Ground", "Wall", "Board" };

        //myColl.m_CollideAgainst = LayerMask.GetMask(layers);
        //myColl.m_IgnoreTag = "Player";
        //myColl.m_TransparentLayers = LayerMask.GetMask("Everything");
        // 9/22 Jung
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
