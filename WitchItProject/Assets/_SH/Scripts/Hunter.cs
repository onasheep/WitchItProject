using Cinemachine;
using Photon.Pun;
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

    private GameObject footFall = default;

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

        //myCamera = GameObject.Find("PersonalCamera").transform;
        myCamera = GameObject.Find("HunterCamera").transform;
        myCamera.GetComponent<CinemachineVirtualCamera>().Priority += 1;
        myCamera.SetParent(transform);
        myCamera.transform.position = transform.position + new Vector3(0, 1.6f, 0);
        crossHair = gameObject.FindChildObj("CrossHair");

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
        if (myPv != null)
        {
            if (!myPv.IsMine)
            {
                return;
            }
        }
        else if (myPv == null)
        {
            if (!photonView.IsMine)
            {
                return;
            }
        }

        Physics.Raycast(myCamera.transform.position + myCamera.transform.forward, myCamera.transform.forward, out hunterRayHit, 15f);

        InputPlayer();


        LimitCameraAngle();

        if (rigid.velocity.magnitude > 5f)
        {
            rigid.velocity = rigid.velocity.normalized * 5f;
        }
    }

    protected override void InputPlayer()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        base.InputPlayer();
    }

    #region SJ_ PlayerBase override
    // SJ_230915 

    protected override void Init()
    {
        base.Init();

        dogRing = this.gameObject.FindChildObj("DogRing");

        base.type = TYPE.HUNTER;

        Camera main = GameObject.Find("Main Camera").GetComponent<Camera>();

        // { SJ_ 석환씨가 말한 주석 

        //// 현재 Culling Mask 값을 가져옵니다.
        //int currentCullingMask = main.cullingMask;

        //// 지정된 레이어를 해제하기 위해 해당 레이어 비트를 제거합니다.
        //int newCullingMask = currentCullingMask & ~(1 << LayerMask.NameToLayer("Hunter"));

        //// 새로운 Culling Mask를 설정합니다.
        //main.cullingMask = newCullingMask;

        // } SJ_ 석환씨가 말한 주석 

        skillSlot.SelSkill((int)type);

        rightFuncCool = skillSlot.Slots[0].CoolTime;
        QFuncCool = skillSlot.Slots[1].CoolTime;



        this.leftFunc = () => photonView.RPC("ThrowKnife", RpcTarget.All, transform.position + transform.up * 1.6f + transform.forward, myCamera.transform.rotation);
        this.rigthFunc =
            () =>
            {
                GameObject obj = PhotonNetwork.Instantiate
                (RDefine.WOLF_OBJ, dogRing.transform.position, Quaternion.identity);
                skillSlot.Slots[0].ActivateSkill(obj, dogRing.transform.forward);

            };
        this.QFunc =
            () =>
            {

                GameObject obj = PhotonNetwork.Instantiate
                (RDefine.CROSS_OBJ, myCamera.position + myCamera.forward, Quaternion.identity);
                skillSlot.Slots[1].ActivateSkill(obj, myCamera.forward);


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

    [PunRPC]
    private void ThrowKnife(Vector3 start_, Quaternion direction_)
    {
        Debug.Log("칼던짐");

        Bullet obj_ = ObjPool.GetBullet();

        if (obj_ == null)
        {
            Debug.LogError("칼이 엄슴");
        }

        //obj_.transform.position = myCamera.position + myCamera.forward;
        obj_.transform.position = start_;
        //obj_.transform.rotation = Quaternion.LookRotation(myCamera.transform.forward, myCamera.transform.forward * -1);
        obj_.transform.rotation = direction_;

        animator.SetTrigger("Shot");

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
            canSpawnFootfall = false;

            StopCoroutine(Footfall());

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
        if (myPv != null)
        {
            if (!myPv.IsMine)
            {
                return;
            }
        }
        else if (myPv == null)
        {
            if (!photonView.IsMine)
            {
                return;
            }
        }

        foreach (ContactPoint contact in collision.contacts)
        {
            Vector3 point = contact.point;

            if (point.y <= transform.position.y + 0.5f)
            {
                if (verticalMove != 0 || horizontalMove != 0)
                {
                    if (canSpawnFootfall)
                    {
                        canSpawnFootfall = false;

                        Effect footfall_ = ObjPool.GetEffect(ObjPool.EffectNames.Footfall);
                        footfall_.transform.position = transform.position;

                        StartCoroutine(Footfall());
                    }
                }

                animator.SetBool("IsGround", true);
                break;
            }
        }
    }
}
