using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SJ_ 230915
// PlayerBase ���
public class Hunter : PlayerBase
{
    /*
    ��Ÿ� ������ �����ϴ� ���̸� ���� ũ�ν����� ���
    ����ĳ��Ʈ���� �������� ������, ���� �Ÿ���ŭ ������ ���� ũ�ν����� ���

    ī�޶�� �������� ũ�ν��� �ٶ󺻴�
     
    */

    // SJ_ 230915
    // LEGACY : PlayerBase�� ����
    private Transform myCamera;
    //private Rigidbody rigid;
    //private Animator animator;
    //

    private GameObject crossHair;
    private RaycastHit hunterRayHit;

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
        Physics.Raycast(myCamera.transform.position + myCamera.transform.forward, myCamera.transform.forward, out hunterRayHit, 15f);

        // SJ_ 230915
        //MoveHunter();
        Move();
        base.InputPlayer();
        // SJ_ 230915

        //Jump();
        //ThrowKnife();

        LimitCameraAngle();
    
        if(rigid.velocity.magnitude > 5f)
        {
            rigid.velocity = rigid.velocity.normalized * 5f;
        }
    }

    #region SJ_ ��ӹ޾Ƽ� �����ϴ� �Լ�
    // SJ_230915 

    protected override void Init()
    {
        base.Init();

        base.type = TYPE.HUNTER;
        // { ��ų ��� 
        skillSlot.SelSkill((int)type);

        
        //  ��ų ��� }



        this.leftFunc = () => ThrowKnife();
        this.rigthFunc =
            () =>
            {
                GameObject obj = Instantiate
                (ResourceManager.objs[skillSlot.Slots[0].SkillType], this.transform.position, Quaternion.identity);
                skillSlot.Slots[0].ActivateSkill(obj);
            };
        this.QFunc =
            () =>
            {
                GameObject obj = Instantiate
                  (ResourceManager.objs[skillSlot.Slots[1].SkillType], this.transform.position, Quaternion.identity);
                skillSlot.Slots[1].ActivateSkill(obj);
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

    // TODO : ���� ���Ϳ� ���డ ������ ���� �ٸ��ٸ� 
    // ���� ���� �������, �ƴ϶�� PalyerBase���� ���������� ���� ��
    private void JumpHunter()
    {
        // InputPlayer�� ��ü 
        //if (Input.GetButtonDown("Jump"))
        //{
            animator.SetBool("IsGround", false);
            animator.SetTrigger("Jump");
       
        rigid.AddForce(transform.up * 6, ForceMode.Impulse);
        //}
    }

    private void MoveHunter()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        rigid.AddForce(transform.forward * verticalInput * 15,ForceMode.Force);
        rigid.AddForce(transform.right * horizontalInput * 15, ForceMode.Force);

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
