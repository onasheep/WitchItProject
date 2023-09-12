using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    /*
    ��Ÿ� ������ �����ϴ� ���̸� ���� ũ�ν����� ���
    ����ĳ��Ʈ���� �������� ������, ���� �Ÿ���ŭ ������ ���� ũ�ν����� ���

    ī�޶�� �������� ũ�ν��� �ٶ󺻴�
     
     */

    private Transform myCamera;
    private GameObject crossHair;

    private RaycastHit hunterRayHit;

    private void Start()
    {
        myCamera = GameObject.Find("HunterCamera").transform;
        crossHair = GameObject.Find("CrossHair");
    }

    private void Update()
    {
        Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hunterRayHit, 15f);

        myCamera.transform.position = transform.position + new Vector3(0, 1.6f, 0);
    }

    private void FixedUpdate()
    {
        RotateCameraVertical();
        RotateCameraHorizontal();

        if (hunterRayHit.collider != null)
        {
            crossHair.transform.position = hunterRayHit.collider.transform.position;
        }
        else
        {
            crossHair.transform.position = myCamera.transform.forward.normalized * 15;
        }
    }

    private void RotateCameraVertical()
    {
        float mouseVerticalMove = Input.GetAxis("Mouse X");

        myCamera.Rotate(Vector3.up * mouseVerticalMove * -5);
    }

    private void RotateCameraHorizontal()
    {
        float mouseHorizontalMove = Input.GetAxis("Mouse Y");

        myCamera.Rotate(Vector3.right * mouseHorizontalMove * 5);
    }
}
