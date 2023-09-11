using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WitchController : MonoBehaviour
{
    [SerializeField] private Rigidbody myRigid;
    [SerializeField] private Animator myAnimator;

    [SerializeField] private  Transform myCamera;

    [SerializeField][Range (0, 100)]private int witchHealth = 50;
    [SerializeField][Range (0f, 100f)]private float witchMana = 50f;


    [SerializeField][Range (0f, 10f)]private float moveSpeed = 5f;
    [SerializeField][Range (0f, 10f)]private float rotationSpeed = 5f;
    [SerializeField][Range(0f, 10f)] private float jumpForce = 5f;
    
    private bool isJump = false;

    public bool isDead = false;

    void Start()
    {
        myCamera = GameObject.Find("PlayerCamera").GetComponent<CinemachineVirtualCamera>().transform;// 가상 카메라 가져와버리기!
        //myCamera = GameObject.Find("Main Camera").transform; //메인카메라를 가져와버리기
        myRigid = GetComponent<Rigidbody>();
        myAnimator = GetComponent<Animator>();

    }

    private void FixedUpdate()
    {
        if (isDead) { return; }
        if(isJump == false)
        {
            Move();
        }
        else
        {
            JumpMove();
        }
        //HJ_ TODO 애니메이션 추가할 때 사용
        SetAnimation("MoveTotal"); 
        Turn();
    }

    void Update()
    {
        if (isDead) { return; }
        if (Input.GetKeyDown(KeyCode.Space) && isJump == false)
        {
            Jump();
        }

        if(Input.GetKeyDown(KeyCode.Y))
        {
            isDead = true;
        }

        if (isDead)
        {
            myAnimator.SetTrigger("Die");
        }
    }


    void Move()
    {
        //==============================공통 부분 삭제 예정
        float moveDirectionZ = Input.GetAxisRaw("Vertical");
        float moveDirectionX = Input.GetAxisRaw("Horizontal");

        Vector3 forwardLook = new Vector3(myCamera.forward.x, 0, myCamera.forward.z);
        Vector3 moveDirection = forwardLook * moveDirectionZ + myCamera.right * moveDirectionX;
        //==============================변경 전
        //Vector3 moveVertical = new Vector3(0, 0, 1) * moveDirectionZ;
        //Vector3 moveHorizontal = new Vector3(1, 0, 0) * moveDirectionX;
        //Vector3 moveNormalized = (moveHorizontal + moveVertical).normalized;
        //Vector3 moveVelocity = moveNormalized * moveSpeed;
        //myRigid.velocity = moveVelocity;
        //==============================변경 후
        Vector3 dirVelocity = moveDirection * moveSpeed;

        dirVelocity.y = myRigid.velocity.y;
        myRigid.velocity = dirVelocity;
        //==============================
    }
    
    void SetAnimation(string name)
    {
        float moveDirectionZ = Input.GetAxisRaw("Vertical");
        float moveDirectionX = Input.GetAxisRaw("Horizontal");

        float moveTotal = Mathf.Clamp01(Mathf.Abs(moveDirectionZ) + Mathf.Abs(moveDirectionX));
        myAnimator.SetFloat(name, moveTotal);
    }

    void Turn()
    {
        //==============================공통 부분 삭제 예정
        float moveDirectionX = Input.GetAxis("Horizontal");
        float moveDirectionZ = Input.GetAxis("Vertical");

        Vector3 forwardLook = new Vector3(myCamera.forward.x, 0, myCamera.forward.z);
        Vector3 moveDirection = forwardLook * moveDirectionZ + myCamera.right * moveDirectionX;
        //==============================변경 전
        //if (!(moveDirectionX == 0f && moveDirectionZ == 0f))
        //{
        //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(moveDirectionX, 0, moveDirectionZ)), Time.deltaTime * rotationSpeed);
        //}
        //==============================변경 후
        moveDirection += myCamera.right * moveDirectionX;
        Vector3 targetDirection = moveDirection;
        targetDirection.y = 0f;
        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }
        Quaternion lookDirection = Quaternion.LookRotation(targetDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, lookDirection, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
        //==============================
    }
    void Jump()
    {
        isJump = true;
        myRigid.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    void JumpMove()
    {
        float moveDirectionX = Input.GetAxis("Horizontal");
        float moveDirectionZ = Input.GetAxis("Vertical");

        Vector3 moveHorizontal = new Vector3(1, 0, 0) * moveDirectionX;
        Vector3 moveVertical = new Vector3(0, 0, 1) * moveDirectionZ;

        Vector3 moveVelocity = (moveHorizontal + moveVertical).normalized * moveSpeed;

        myRigid.AddForce(moveVelocity, ForceMode.Force);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isJump = false;
        }
    }

}
