using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SJ_ 230915
// PlayerBase ���
public class Hunter : PlayerBase
{
    // SJ_ 230915
    // LEGACY : PlayerBase�� ����
    //private Transform myCamera;
    //private Rigidbody rigid;
    //private Animator animator;

    //HJ__0920 포톤 테스트 추가
    private PhotonView myPv;
    //=====================
    private RaycastHit hunterRayHit;

    private float rightFuncCool = default;
    private float QFuncCool = default;
    private float skillTimer = default;

    // SJ_ 230918
    private GameObject dogRing;

    private void OnEnable()
    {
        //HJ
        if (!photonView.IsMine)
        {
            return;
        }

        // SJ_ 230915
        Init();
        //

        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 잠금 상태로 설정
        Cursor.visible = false; // 마우스 커서를 숨김

        // HJ_ 230920 
        myPv = GetComponent<PhotonView>();
        //

        myCamera = GameObject.Find("HunterCamera").transform;
        myCamera.SetParent(transform);
        myCamera.transform.position = transform.position + new Vector3(0, 1.6f, 0);
        crossHair = GameObject.Find("CrossHair");
        // SJ_ 230915
        // LEGACY : PlayerBase���� ������
        //rigid = GetComponent<Rigidbody>();
        //animator = GetComponent<Animator>();
    }

    private void Update()
    {

        //if (!photonView.IsMine)
        //{
        //    return;
        //}

        //Debug.LogFormat("timer : {0}", skillTimer);
        //Debug.LogFormat("right : {0}", rightFuncCool);
        //Debug.LogFormat("q : {0}", QFuncCool);

        if (!myPv.IsMine)
        {
            return;
        }

        Physics.Raycast(myCamera.transform.position + myCamera.transform.forward, myCamera.transform.forward, out hunterRayHit, 15f);


        // SJ_ 230915
        //MoveHunter();
        //HJ__

        base.InputPlayer();
        skillTimer += Time.deltaTime;

        // SJ_ 230915

        //Jump();
        //ThrowKnife();

        LimitCameraAngle();

        if (rigid.velocity.magnitude > 5f)
        {
            rigid.velocity = rigid.velocity.normalized * 5f;
        }
    }

    protected override void InputPlayer()
    {
        if (!myPv.IsMine)
        {
            return;
        }

        base.InputPlayer();
    }

    #region SJ_ ��ӹ޾Ƽ� �����ϴ� �Լ�
    // SJ_230915 

    protected override void Init()
    {
        base.Init();

        // SJ_ 230918 ���� �߻� ��ġ
        dogRing = this.gameObject.FindChildObj("DogRing");

        base.type = TYPE.HUNTER;

        Camera main = GameObject.Find("Main Camera").GetComponent<Camera>();

        // 현재 Culling Mask 값을 가져옵니다.
        int currentCullingMask = main.cullingMask;

        // 지정된 레이어를 해제하기 위해 해당 레이어 비트를 제거합니다.
        int newCullingMask = currentCullingMask & ~(1 << LayerMask.NameToLayer("Hunter"));

        // 새로운 Culling Mask를 설정합니다.
        main.cullingMask = newCullingMask;

        // { ��ų ��� 
        skillSlot.SelSkill((int)type);

        //  ��ų ��� }
        rightFuncCool = skillSlot.Slots[0].CoolTime;
        QFuncCool = skillSlot.Slots[1].CoolTime;



        this.leftFunc = () => ThrowKnife();
        this.rigthFunc =
            () =>
            {
                if (skillTimer > rightFuncCool)
                {
                    rightFuncCool += skillTimer;
                    GameObject obj = Instantiate
                (ResourceManager.objs[skillSlot.Slots[0].SkillType], dogRing.transform.position, dogRing.transform.rotation);
                    skillSlot.Slots[0].ActivateSkill(obj, dogRing.transform.forward);
                }

            };
        this.QFunc =
            () =>
            {
                if (skillTimer > QFuncCool)
                {
                    skillTimer -= QFuncCool;
                    GameObject obj = Instantiate
                  (ResourceManager.objs[skillSlot.Slots[1].SkillType], myCamera.position + myCamera.forward, myCamera.transform.rotation);
                    skillSlot.Slots[1].ActivateSkill(obj, myCamera.forward);
                }

            };
        this.jumpFunc = () => JumpHunter();
    }
    protected override void Move()
    {
        this.moveFunc = () => MoveHunter();

        base.Move();
    }



    #endregion
    private void FixedUpdate()
    {
        //HJ 포톤 붙이기
        if (!photonView.IsMine)
        {
            return;
        }

        Move();

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
        Debug.Log("칼던짐");
        //if (Input.GetButtonDown("Fire1"))
        //{
        Bullet obj_ = BulletPool.GetObject();

        obj_.transform.position = myCamera.position + myCamera.forward;
        obj_.transform.rotation = Quaternion.LookRotation(myCamera.transform.up, myCamera.transform.forward * -1);

        animator.SetTrigger("Shot");
        //}
        //else if (Input.GetButton("Fire1"))
        //{
        //    Bullet obj_ = BulletPool.GetObject();

        //    obj_.transform.position = myCamera.position + myCamera.forward;
        //    obj_.transform.rotation = Quaternion.LookRotation(myCamera.transform.up, myCamera.transform.forward * -1);
        //}
    }

    private void JumpHunter()
    {
        // InputPlayer�� ��ü 
        //if (Input.GetButtonDown("Jump"))
        //{
        if (animator.GetBool("IsGround"))
        {
            animator.SetBool("IsGround", false);
            animator.SetTrigger("Jump");

            rigid.AddForce(transform.up * JUMPFORCE, ForceMode.Impulse);
        }
        //}
    }

    private void MoveHunter()
    {
        rigid.AddForce(transform.forward * verticalMove * MOVESPEED, ForceMode.Force);
        rigid.AddForce(transform.right * horizontalMove * MOVESPEED, ForceMode.Force);

        animator.SetFloat("InputVertical", verticalMove);
        animator.SetFloat("InputHorizontal", horizontalMove);
    }

    private void RotateVertical()
    {
        float mouseVerticalMove = Input.GetAxis("Mouse X");

        transform.Rotate(Vector3.up * mouseVerticalMove * 5);
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
        float mouseHorizontalMove = Input.GetAxis("Mouse Y");

        myCamera.Rotate(Vector3.right * mouseHorizontalMove * -5);
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Vector3 point = contact.point;

            if (point.y <= transform.position.y + 0.5f)
            {
                animator.SetBool("IsGround", true);
                break;
            }
        }
    }
}
