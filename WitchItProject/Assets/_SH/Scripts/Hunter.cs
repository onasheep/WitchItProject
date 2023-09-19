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

    private RaycastHit hunterRayHit;

    private float rightFuncCool = default;
    private float QFuncCool = default;
    private float skillTimer = default;

    // SJ_ 230918
    private GameObject dogRing;

    private void Start()
    {
        // SJ_ 230915
        Init();
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
        Debug.LogFormat("timer : {0}", skillTimer);
        Debug.LogFormat("right : {0}", rightFuncCool);
        Debug.LogFormat("q : {0}", QFuncCool);
        Physics.Raycast(myCamera.transform.position + myCamera.transform.forward, myCamera.transform.forward, out hunterRayHit, 15f);

        // SJ_ 230915
        //MoveHunter();
        Move();
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

    #region SJ_ ��ӹ޾Ƽ� �����ϴ� �Լ�
    // SJ_230915 

    protected override void Init()
    {
        base.Init();

        // SJ_ 230918 ���� �߻� ��ġ
        dogRing = this.gameObject.FindChildObj("DogRing");

        base.type = TYPE.HUNTER;

        // { ��ų ��� 
        skillSlot.SelSkill((int)type);


        //  ��ų ��� }
        rightFuncCool = skillSlot.Slots[0].CoolTime;
        QFuncCool = skillSlot.Slots[1].CoolTime;



        this.leftFunc = () => ThrowKnife();
        this.rigthFunc =
            () =>
            {                
                if(skillTimer > rightFuncCool)
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
                if(skillTimer > QFuncCool)
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
        animator.SetBool("IsGround", false);
        animator.SetTrigger("Jump");

        rigid.AddForce(transform.up * JUMPFORCE, ForceMode.Impulse);
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
