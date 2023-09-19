using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

// SJ_ 230915
// PlayerBase 상속
public class WitchController : PlayerBase
{

    // SJ_ 230915
    // LEGACY : PlayerBase에 존재
    //[SerializeField] private Rigidbody rigid;
    //[SerializeField] private Animator animator;
    //

    [SerializeField][Range(0, 100)] private int witchHealth = 50;
    [SerializeField][Range(0f, 100f)] private float witchMana = 50f;


    [SerializeField][Range(0f, 10f)] private float rotationSpeed = 5f;

    private bool isJump = false;
    public bool isDead = false;

    // SJ_230918
    // 마녀 스킬 발사 위치
    private GameObject barrel = default;
    private GameObject lookPoint = default;
    private RaycastHit hit;

    // 09/19 Jung
    // 변신한 물체에 따라 바뀔 예정 const X
    private float healthMax = 100;
    private float health;

    // 변신 여부
    public bool isMetamor = false;
    // 09/19 Jung

    void Start()
    {
        Init();
        health = healthMax;

        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 잠금 상태로 설정
        Cursor.visible = false; // 마우스 커서를 숨김
        Debug.Log(GetComponent<Collider>().bounds.size.magnitude);

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
            //JumpMove();
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
        barrel = this.gameObject.FindChildObj("Barrel");
        lookPoint = this.gameObject.FindChildObj("CameraLookPoint");
        base.type = TYPE.WITCH;
        // { 스킬 담김 
        skillSlot.SelSkill((int)type);
        //  스킬 담김 }

        //  ��ų ��� }






        // TODO : 변신 기능 함수 추가
        this.leftFunc =
            () =>
            {

            };

        this.rigthFunc =
            () =>
            {
                GameObject obj = Instantiate(
                    ResourceManager.objs[RDefine.MUSHROOM_ORB], barrel.transform.position, Quaternion.identity);
                skillSlot.Slots[0].ActivateSkill(obj, (barrel.transform.position - myCamera.position).normalized);
            };
        this.QFunc =
            () =>
            {
                Physics.Raycast(lookPoint.transform.position, (lookPoint.transform.position - myCamera.position).normalized,
                    out hit, Mathf.Infinity, LayerMask.GetMask("ChangeableObjects"));
                if (hit.collider != null)
                {
                    skillSlot.Slots[1].ActivateSkill(hit.collider.gameObject);
                }
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
        //==============================���� �κ� ���� ����
        // 09/18 Jung
        //float moveDirectionZ = Input.GetAxisRaw("Vertical");
        //float moveDirectionX = Input.GetAxisRaw("Horizontal");

        if (isMetamor) { /* Do Nothing */ }
        else if (!isMetamor)
        {
            Vector3 forwardLook = new Vector3(myCamera.forward.x, 0, myCamera.forward.z);
            Vector3 moveDirection = forwardLook * verticalMove + myCamera.right * horizontalMove;

            Vector3 dirVelocity = moveDirection * MOVESPEED;

            dirVelocity.y = rigid.velocity.y;
            rigid.velocity = dirVelocity;
        }
        // 09/18 Jung
        //==============================���� ��
        //Vector3 moveVertical = new Vector3(0, 0, 1) * moveDirectionZ;
        //Vector3 moveHorizontal = new Vector3(1, 0, 0) * moveDirectionX;
        //Vector3 moveNormalized = (moveHorizontal + moveVertical).normalized;
        //Vector3 moveVelocity = moveNormalized * moveSpeed;
        //myRigid.velocity = moveVelocity;
        //==============================���� ��
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


        rigid.AddForce(transform.up * JUMPFORCE, ForceMode.Impulse);
    }

    void JumpMove()
    {
        float moveDirectionX = Input.GetAxis("Horizontal");
        float moveDirectionZ = Input.GetAxis("Vertical");

        Vector3 moveHorizontal = new Vector3(1, 0, 0) * moveDirectionX;
        Vector3 moveVertical = new Vector3(0, 0, 1) * moveDirectionZ;

        Vector3 moveVelocity = (moveHorizontal + moveVertical).normalized * MOVESPEED;

        rigid.AddForce(moveVelocity, ForceMode.Force);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isJump = false;
        }
    }

    public void TakeDamage()
    {
        // 데미지는 5?
        health -= 5;
    }

    //HJ=======================================================================
    //230919 작업 게임매니저의 witchCount 줄여주고 늘려주는 함수 추가
    void GetWitch()
    {
        //마녀가 생성될 때 이 함수를 써서 늘려줍니다.
        FindObjectOfType<GameManager>().witchCount += 1;
    }

    void DieWitch()
    {
        FindObjectOfType<GameManager>().witchCount -= 1;
    }



}
