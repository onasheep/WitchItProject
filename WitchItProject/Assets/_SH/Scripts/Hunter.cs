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


    private GameObject bulletPrefab;
    private Vector3 fireDirection;

    private Rigidbody rigid;
    private Animator animator;

    private RaycastHit hunterRayHit;

    private void Start()
    {
        myCamera = GameObject.Find("HunterCamera").transform;
        myCamera.SetParent(transform);
        myCamera.transform.position = transform.position + new Vector3(0, 1.6f, 0);
        crossHair = GameObject.Find("CrossHair");

        bulletPrefab = (GameObject)Resources.Load("Bullet");

        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Physics.Raycast(myCamera.transform.position + myCamera.transform.forward, myCamera.transform.forward, out hunterRayHit, 15f);

        MoveHunter();
    }

    private void FixedUpdate()
    {
        RotateVertical();
        RotateHorizontal();

        if (hunterRayHit.collider != null)
        {
            crossHair.transform.position = hunterRayHit.point;
        }
        else
        {
            crossHair.transform.position = myCamera.transform.forward.normalized * 15;
        }

        fireDirection = crossHair.transform.position - myCamera.transform.position;
    }

    private void ThrowKnife()
    {
        if (Input.GetButtonDown("Fire1"))
        {

        }
    }

    private void MoveHunter()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        rigid.AddForce(transform.forward * verticalInput * 50);
        rigid.AddForce(transform.right * horizontalInput * 50);

        animator.SetFloat("InputVertical", verticalInput);
        animator.SetFloat("InputHorizontal", horizontalInput);
    }

    private void RotateVertical()
    {
        float mouseVerticalMove = Input.GetAxis("Mouse Y");

        myCamera.Rotate(Vector3.right * mouseVerticalMove * -5);
    }

    private void RotateHorizontal()
    {
        float mouseHorizontalMove = Input.GetAxis("Mouse X");

        transform.Rotate(Vector3.up * mouseHorizontalMove * 5);
    }
}
