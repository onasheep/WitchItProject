using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SJ_ 230915
// PlayerBase 상속
public class Hunter : PlayerBase
{
    /*
    사거리 제한이 존재하는 레이를 쏴서 크로스헤어로 사용
    레이캐스트힛이 존재하지 않으면, 제한 거리만큼 떨어진 곳을 크로스헤어로 사용

    카메라와 오른손은 크로스헤어를 바라본다
     
    */

    // SJ_ 230915
    // LEGACY : PlayerBase에 존재
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
        // LEGACY : PlayerBase에서 가져옴
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

    #region SJ_ 상속받아서 동작하는 함수
    // SJ_230915 

    protected override void Init()
    {
        base.Init();

        base.type = TYPE.HUNTER;
        // { 스킬 담김 
        skillSlot.SelSkill((int)type);

        
        //  스킬 담김 }



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

    // TODO : 각각 헌터와 마녀가 점프가 많이 다르다면 
    // 지금 같은 방식으로, 아니라면 PalyerBase에서 공통적으로 만들 것
    private void JumpHunter()
    {
        // InputPlayer로 대체 
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
