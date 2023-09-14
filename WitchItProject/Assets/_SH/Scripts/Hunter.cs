using Photon.Pun.Demo.Cockpit;
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

    private Rigidbody rigid;
    private Animator animator;

    private RaycastHit hunterRayHit;

    private void Start()
    {
        myCamera = GameObject.Find("HunterCamera").transform;
        myCamera.SetParent(transform);
        myCamera.transform.position = transform.position + new Vector3(0, 1.6f, 0);
        crossHair = GameObject.Find("CrossHair");

        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Physics.Raycast(myCamera.transform.position + myCamera.transform.forward, myCamera.transform.forward, out hunterRayHit, 15f);

        MoveHunter();
        Jump();
        ThrowKnife();

        LimitCameraAngle();
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
    }

    private void ThrowKnife()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Bullet obj_ = BulletPool.GetObject();

            obj_.transform.position = myCamera.position + myCamera.forward;
            obj_.transform.rotation = Quaternion.LookRotation(myCamera.transform.up, myCamera.transform.forward * -1);

            animator.SetTrigger("Shot");
        }
        //else if (Input.GetButton("Fire1"))
        //{
        //    Bullet obj_ = BulletPool.GetObject();

        //    obj_.transform.position = myCamera.position + myCamera.forward;
        //    obj_.transform.rotation = Quaternion.LookRotation(myCamera.transform.up, myCamera.transform.forward * -1);
        //}
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("IsGround", false);
            animator.SetTrigger("Jump");
            rigid.AddForce(transform.up * 6, ForceMode.Impulse);
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

    private void LimitCameraAngle()
    {
        Vector3 currentAngle = myCamera.transform.rotation.eulerAngles;

        if (currentAngle.x > 180)
        {
            currentAngle.x = Mathf.Clamp(currentAngle.x, 270, 360);
        }
        else
        {
            currentAngle.x = Mathf.Clamp(currentAngle.x, 0, 72);
        }

        currentAngle.y = transform.rotation.eulerAngles.y;
        currentAngle.z = transform.rotation.eulerAngles.z;

        myCamera.rotation = Quaternion.Euler(currentAngle);
    }

    private void RotateHorizontal()
    {
        float mouseHorizontalMove = Input.GetAxis("Mouse X");

        transform.Rotate(Vector3.up * mouseHorizontalMove * 5);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (animator.GetBool("IsGround") == false)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                Vector3 point = contact.point;

                if (point.y <= transform.position.y)
                {
                    animator.SetBool("IsGround", true);
                    break;
                }
            }
        }
    }
}
