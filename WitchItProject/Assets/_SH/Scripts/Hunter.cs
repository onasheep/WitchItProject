using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    /*
    사거리 제한이 존재하는 레이를 쏴서 크로스헤어로 사용
    레이캐스트힛이 존재하지 않으면, 제한 거리만큼 떨어진 곳을 크로스헤어로 사용

    카메라와 오른손은 크로스헤어를 바라본다
     
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
