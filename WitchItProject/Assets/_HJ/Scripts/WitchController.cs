using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// SJ_ 230915
// PlayerBase 상속
public class WitchController : PlayerBase
{

    // SJ_ 230915
    // LEGACY : PlayerBase에 존재
    //[SerializeField] private Rigidbody rigid;
    //[SerializeField] private Animator animator;
    [SerializeField] private Transform myCamera;
    //
    
    [SerializeField][Range(0, 100)] private int witchHealth = 50;
    [SerializeField][Range(0f, 100f)] private float witchMana = 50f;


    [SerializeField][Range(0f, 10f)] private float rotationSpeed = 5f;

    private bool isJump = false;
    public bool isDead = false;

    void Start()
    {
        Init();
        
        myCamera = GameObject.Find("WitchCamera").GetComponent<CinemachineVirtualCamera>().transform;// 가상 카메라 가져와버리기!
        //myCamera = GameObject.Find("Main Camera").transform; //메인카메라를 가져와버리기
        
        // SJ_ 230915
        // LEGACY : PlayerBase에서 가져옴
        //rigid = GetComponent<Rigidbody>();
        //animator = GetComponent<Animator>();

    }

    void Update()
    {

        base.InputPlayer();

        //if (isDead) { return; }
        //if (Input.GetKeyDown(KeyCode.Space) && isJump == false)
        //{
        //    Jump();
        //    animator.SetTrigger("Jumping");
        //}
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    isDead = true;
        //}
        //if (isDead)
        //{
        //    animator.SetTrigger("Die");
        //}
    }

    private void FixedUpdate()
    {
        if (isDead) { return; }
        if (isJump == false)
        {
            // SJ_ 230915
            //MoveHunter();
            Move();
            // SJ_ 230915
        }
        else
        {
            JumpMove();
        }
        //HJ_ TODO 애니메이션 추가할 때 사용
        SetAnimation("MoveTotal");
        Turn();
    }

    #region SJ_ 상속받아서 동작하는 함수
    // SJ_230915 

    protected override void Init()
    {
        base.Init();

        base.type = TYPE.WITCH;
        // { 스킬 담김 
        skillSlot.SelSkill((int)type);

        //  스킬 담김 }

        // { 자체적으로 초기화해야 할 변수들 
        moveSpeed = 5f;
        jumpForce = 5f;
        //  자체적으로 초기화해야 할 변수들 }

        


        // TODO : 변신 기능 함수 추가
        //this.leftFunc = () => ThrowKnife();
        this.rigthFunc =
            () =>
            {
                GameObject obj = Instantiate(
                    ResourceManager.objs[skillSlot.Slots[0].SkillType], this.transform.position, Quaternion.identity);
                skillSlot.Slots[0].ActivateSkill(obj);
            };
        this.QFunc =
            () =>
            {
                GameObject obj = Instantiate(
                    ResourceManager.objs[skillSlot.Slots[1].SkillType], this.transform.position, Quaternion.identity);
                skillSlot.Slots[1].ActivateSkill(obj);
            };
        this.jumpFunc = () => JumpWitch();
            
    }
    protected override void Move()
    {
        this.moveFunc = () => MoveWitch();
        base.Move();
    }

    #endregion

    // SJ_ 230915
    // Base에 Move함수가 있어 함수 이름 변경
    void MoveWitch()
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

        dirVelocity.y = rigid.velocity.y;
        rigid.velocity = dirVelocity;
        //==============================
    }

    void SetAnimation(string name)
    {
        float moveDirectionZ = Input.GetAxisRaw("Vertical");
        float moveDirectionX = Input.GetAxisRaw("Horizontal");

        float moveTotal = Mathf.Clamp01(Mathf.Abs(moveDirectionZ) + Mathf.Abs(moveDirectionX));
        animator.SetFloat(name, moveTotal);
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

    // TODO : 각각 헌터와 마녀가 점프가 많이 다르다면 
    // 지금 같은 방식으로, 아니라면 PalyerBase에서 공통적으로 만들 것
    private void JumpWitch()
    {
        isJump = true;
        // myAnimator.SetBool("Jumping", false);


        rigid.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void JumpMove()
    {
        float moveDirectionX = Input.GetAxis("Horizontal");
        float moveDirectionZ = Input.GetAxis("Vertical");

        Vector3 moveHorizontal = new Vector3(1, 0, 0) * moveDirectionX;
        Vector3 moveVertical = new Vector3(0, 0, 1) * moveDirectionZ;

        Vector3 moveVelocity = (moveHorizontal + moveVertical).normalized * moveSpeed;

        rigid.AddForce(moveVelocity, ForceMode.Force);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isJump = false;
        }
    }

}
