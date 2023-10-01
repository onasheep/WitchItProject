using Cinemachine;
using Photon.Pun;
using UnityEngine;


public class Hunter : PlayerBase
{


    //HJ__0920 포톤 테스트 추가
    private PhotonView myPv;
    //=====================
    private RaycastHit hunterRayHit;

    // 늑대 스킬 발사 위치
    private GameObject dogRing;

    private void OnEnable()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        Init();

        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 잠금 상태로 설정
        Cursor.visible = false; // 마우스 커서를 숨김

        myPv = GetComponent<PhotonView>();


        myCamera = GameObject.Find("HunterCamera").transform;
        myCamera.GetComponent<CinemachineVirtualCamera>().Priority += 1;
        myCamera.SetParent(transform);
        myCamera.transform.position = transform.position + new Vector3(0, 1.6f, 0);
        crossHair = gameObject.FindChildObj("CrossHair");

    }

    private void Update()
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

        Physics.Raycast(myCamera.transform.position + myCamera.transform.forward, myCamera.transform.forward, out hunterRayHit, 15f);

        InputPlayer();


        RotateVertical();
        RotateHorizontal();
        LimitCameraAngle();

        if (rigid.velocity.magnitude > 5f)
        {
            rigid.velocity = rigid.velocity.normalized * 5f;
        }
    }

    protected override void InputPlayer()
    {
        base.InputPlayer();
    }

    #region SJ_ PlayerBase override

    protected override void Init()
    {
        base.Init();
        dogRing = this.gameObject.FindChildObj("DogRing");
        base.type = TYPE.HUNTER;

        // { 스킬 담김 
        skillSlot.SelSkill((int)type);
        // } 스킬 담김 

        Camera main = GameObject.Find("Main Camera").GetComponent<Camera>();
        // { SJ_ 석환씨가 말한 주석 

        //// 현재 Culling Mask 값을 가져옵니다.
        //int currentCullingMask = main.cullingMask;

        //// 지정된 레이어를 해제하기 위해 해당 레이어 비트를 제거합니다.
        //int newCullingMask = currentCullingMask & ~(1 << LayerMask.NameToLayer("Hunter"));

        //// 새로운 Culling Mask를 설정합니다.
        //main.cullingMask = newCullingMask;

        // } SJ_ 석환씨가 말한 주석 



        this.leftFunc =
        () =>
        {
            animator.SetTrigger("Shot");

            photonView.RPC("ThrowKnife", RpcTarget.All, myCamera.transform.position + transform.forward, myCamera.transform.rotation);
        };
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
        if (!photonView.IsMine)
        {
            return;
        }

        Move();

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
        Bullet obj_ = ObjPool.GetBullet();

        if (obj_ == null)
        {
            Debug.LogError("칼이 엄슴");
        }

        obj_.transform.position = start_;
        obj_.transform.rotation = direction_;
    }

    private void JumpHunter()
    {
        if (animator.GetBool("IsGround"))
        {
            canSpawnFootfall = false;

            StopCoroutine(Footfall());

            animator.SetBool("IsGround", false);
            animator.SetTrigger("Jump");

            rigid.AddForce(transform.up * JUMPFORCE, ForceMode.Impulse);
        }
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

                        photonView.RPC("FootfallEffect", RpcTarget.AllBufferedViaServer, transform.position);

                        StartCoroutine(Footfall());
                    }
                }

                animator.SetBool("IsGround", true);
                break;
            }
        }
    }

    [PunRPC]
    private void FootfallEffect(Vector3 myPos_)
    {
        Effect footfall_ = ObjPool.GetEffect(ObjPool.EffectNames.Footfall);
        footfall_.transform.position = myPos_;
    }
}
