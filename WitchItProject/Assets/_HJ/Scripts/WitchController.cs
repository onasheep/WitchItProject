using Cinemachine;
using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.UI;

public class WitchController : PlayerBase
{

    //HJ__0920 포톤 테스트 추가
    private PhotonView myPv;
    //=====================

    [SerializeField][Range(0, 100)] private int witchHealth = 50;
    [SerializeField][Range(0f, 100f)] private float witchMana = 50f;


    [SerializeField][Range(0f, 10f)] private float rotationSpeed = 5f;

    private bool isJump = false;
    public bool isDead = false;

    // { 마녀 스킬 발사 위치
    private GameObject barrel = default;
    private GameObject lookPoint = default;
    private RaycastHit hit;
    // } 마녀 스킬 발사 위치


    // 변신한 물체에 따라 바뀔 예정 const 불가
    private float healthMax = 100;
    private float health;

    // 변신 여부
    public bool isMetamor;

    private GameObject witchBody;
    private GameObject currBody;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        Init();
        health = healthMax;
        isMetamor = false;


        myPv = GetComponent<PhotonView>();

        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 잠금 상태로 설정
        Cursor.visible = false; // 마우스 커서를 숨김

        myCamera = GameObject.Find("WitchCamera").transform;// 가상 카메라 가져와버리기!

        myCamera.GetComponent<CinemachineVirtualCamera>().Follow = transform.GetChild(2);
        myCamera.GetComponent<CinemachineVirtualCamera>().LookAt = transform.GetChild(2);

        witchBody = gameObject.FindChildObj("Character_Female_Witch");
        currBody = witchBody;

    }

    private void OnDisable()
    {
        //TODO
        //죽었을 때 관전으로 갈 수 있는 기능 넣을 예정
    }


    void Update()
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

        if (health <= 0)
        {

        }

        // { Ray로 변신, 빙의, 아웃라인 체크 
        if (Physics.Raycast(lookPoint.transform.position, (lookPoint.transform.position - myCamera.position).normalized,
            out hit, 15f, LayerMask.GetMask("ChangeableObjects")))
        {
            if (hit.transform.GetComponent<ChangeableObject>() == null)
            {
                return;
            }       // if : 변신가능 오브젝트가 아니면 return

            hit.transform.GetComponent<ChangeableObject>().SetOutline();
        }
        // } Ray로 변신, 빙의, 아웃라인 체크 


        InputPlayer();

        if (Input.GetKeyDown(KeyCode.F))
        {
            isMetamor = false;
            //CancelMetamorphosis(myPv.ViewID);
            //photonView.RPC("CancelMetamorphosis", RpcTarget.All, myPv.ViewID);
            photonView.RPC("CancelPlease", RpcTarget.MasterClient, myPv.ViewID);
            // 변신여부에 따라 모델 on/off
            photonView.RPC("SetModelActive", RpcTarget.AllBufferedViaServer, isMetamor);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            DieWitch();
        }
    }

    protected override void InputPlayer()
    {
        base.InputPlayer();
    }

    private void FixedUpdate()
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

        if (isDead) { return; }
        if (isJump == false)
        {
            Move();
        }
        else
        {
            //JumpMove();
        }



        SetAnimation("MoveTotal");
        Turn();
    }

    #region SJ_ 상속받아서 동작하는 함수

    protected override void Init()
    {
        base.Init();
        barrel = this.gameObject.FindChildObj("Barrel");
        lookPoint = this.gameObject.FindChildObj("CameraLookPoint");
        base.type = TYPE.WITCH;

        // { 스킬 담김 
        skillSlot.SelSkill((int)type);
        //  스킬 담김 }



        this.leftFunc =
            () =>
            {
                if (hit.collider != null)
                {
                    //SJ_230927
                    if (isMetamol_On == true)
                    {
                        isMetamor = true;

                        isMetamol_On = false;
                        //MetamorphosisToObj(myPv.ViewID, hit.collider.gameObject.GetComponent<PhotonView>().ViewID);
                        //photonView.RPC("MetamorphosisToObj", RpcTarget.All, myPv.ViewID, hit.collider.gameObject.GetComponent<PhotonView>().ViewID);
                        photonView.RPC("MetamorphosisPlease", RpcTarget.MasterClient, myPv.ViewID, hit.collider.gameObject.GetComponent<PhotonView>().ViewID);
                        // 변신여부에 따라 모델 on/off
                        photonView.RPC("SetModelActive", RpcTarget.AllBufferedViaServer, isMetamor);
                        ThreadManager.instance.DoRoutine(() => OnSkill(ref isMetamol_On), metamolCool);
                    }
                }
            };

        this.rigthFunc =
            () =>
            {
                GameObject obj = PhotonNetwork.Instantiate
                (RDefine.MUSHROOM_ORB, barrel.transform.position, Quaternion.identity);
                skillSlot.Slots[0].ActivateSkill(obj, (barrel.transform.position - myCamera.position).normalized);
            };
        this.QFunc =
            () =>
            {
                if (hit.collider != null)
                {
                    isMetamor = true;

                    //PossesionToObj(myPv.ViewID, hit.collider.gameObject.GetComponent<PhotonView>().ViewID);
                    //photonView.RPC("PossesionToObj", RpcTarget.All, myPv.ViewID, hit.collider.gameObject.GetComponent<PhotonView>().ViewID);
                    photonView.RPC("PossesionPlease", RpcTarget.MasterClient, myPv.ViewID, hit.collider.gameObject.GetComponent<PhotonView>().ViewID);
                    // 변신여부에 따라 모델 on/off
                    photonView.RPC("SetModelActive", RpcTarget.AllBufferedViaServer, isMetamor);
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


    void MoveWitch()
    {


        if (isMetamor)
        {
            //MoveVertical();
            //MoveHorizontal();
        }
        else if (!isMetamor)
        {
            Vector3 forwardLook = new Vector3(myCamera.forward.x, 0, myCamera.forward.z);
            Vector3 rightLook = new Vector3(myCamera.right.x, 0, myCamera.right.z);
            //Vector3 moveDirection = forwardLook * verticalMove + myCamera.right * horizontalMove;

            //Vector3 dirVelocity = moveDirection * 5;

            //dirVelocity.y = rigid.velocity.y;
            //rigid.velocity = dirVelocity;

            rigid.AddForce(forwardLook * verticalMove * MOVESPEED, ForceMode.Force);
            rigid.AddForce(rightLook * horizontalMove * MOVESPEED, ForceMode.Force);
        }

    }

    // { 변신 후 이동 로직
    void MoveVertical()
    {
        // 이동에 사용할 축
        // 전후 방향에 수직이며, 시계방향으로 90도 만큼 돌아간 축
        Vector3 torqueAxis_ = myCamera.transform.right;
        torqueAxis_.y = 0;

        if (verticalMove > 0)
        {
            // 축에 시계방향으로 회전
            rigid.AddTorque(torqueAxis_ * 50);
        }
        else if (verticalMove < 0)
        {
            // 축에 반시계방향으로 회전
            rigid.AddTorque(torqueAxis_ * -50);
        }
    }

    void MoveHorizontal()
    {
        // 이동에 사용할 축
        // 벡터 상 반대의 방향을 갖는 축
        Vector3 torqueAxis_ = myCamera.transform.forward;
        torqueAxis_.y = 0;

        if (horizontalMove > 0)
        {
            // 축에 시계방향으로 회전
            rigid.AddTorque(torqueAxis_ * -50);
        }
        else if (horizontalMove < 0)
        {
            // 축에 반시계방향으로 회전
            rigid.AddTorque(torqueAxis_ * 50);
        }
    }
    // } 변신 후 이동 로직

    void SetAnimation(string name)
    {


        float moveTotal = Mathf.Clamp01(Mathf.Abs(verticalMove) + Mathf.Abs(horizontalMove));
        animator.SetFloat(name, moveTotal);
    }

    void Turn()
    {


        Vector3 forwardLook = new Vector3(myCamera.forward.x, 0, myCamera.forward.z);
        Vector3 moveDirection = forwardLook * verticalMove + myCamera.right * horizontalMove;

        moveDirection += myCamera.right * horizontalMove;
        Vector3 targetDirection = moveDirection;
        targetDirection.y = 0f;
        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }
        Quaternion lookDirection = Quaternion.LookRotation(targetDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, lookDirection, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }


    private void JumpWitch()
    {
        if (!photonView.IsMine)
        { return; }
        if (isMetamor) { /* Do Nothing */ }
        else if (!isMetamor)
        {
            if (!isJump)
            {
                StopCoroutine(Footfall());

                isJump = true;


                rigid.AddForce(transform.up * JUMPFORCE, ForceMode.Impulse);
                animator.SetTrigger("Jumping");
            }
        }
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

                isJump = false;
                break;
            }
        }
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


    [PunRPC]
    public void TakeDamage()
    {
        // 데미지는 5?
        if (photonView.IsMine)
        {
            health -= 5;
        }
    }



    void DieWitch()
    {

        // 마스터 클라이언트에 RPC를 보내서 isWitch 값을 변경합니다.
        gameManager.photonView.RPC("SetIsWitch", RpcTarget.MasterClient, true);


        // 여기에서 플레이어를 파괴하거나 다른 처리를 수행할 수 있습니다.
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    private void SetModelActive(bool isMetamor_)
    {
        bool isActive_ = !isMetamor_;

        witchBody.SetActive(isActive_);
    }

    [PunRPC]
    private void CancelPlease(int myViewID_)
    {
        // 마스터에서만 실행
        // AllBufferedViaServer는 
        photonView.RPC("CancelMetamorphosis", RpcTarget.AllBufferedViaServer, myViewID_);
    }

    [PunRPC]
    private void CancelMetamorphosis(int myViewID_)
    {
        WitchController witch_ = PhotonView.Find(myViewID_).GetComponent<WitchController>();
        GameObject witchBody_ = witch_.witchBody;
        GameObject lookPoint_ = witch_.lookPoint;

        if (witch_.currBody == witchBody_)
        {
            return;
        }
        else
        {
            //witchBody_.SetActive(isActive_);
            witch_.transform.position = lookPoint_.transform.position;

            Rigidbody witchRigid_ = witch_.GetComponent<Rigidbody>();
            witchRigid_.useGravity = true;
            witchRigid_.velocity = Vector3.zero;
            witchRigid_.angularVelocity = Vector3.zero;

            Collider witchCollider_ = witch_.GetComponent<Collider>();
            witchCollider_.enabled = true;

            GameObject prevBody_ = lookPoint_.transform.parent.gameObject;

            lookPoint_.transform.SetParent(transform);
            lookPoint_.transform.localPosition = new Vector3(0, 1.379f, 0);

            witch_.currBody = witchBody_;

            PhotonNetwork.Destroy(prevBody_);
        }
    }


    [PunRPC]
    private void PossesionPlease(int myViewID_, int objViewID_)
    {
        photonView.RPC("PossesionToObj", RpcTarget.AllBufferedViaServer, myViewID_, objViewID_);
    }

    [PunRPC]
    private void PossesionToObj(int myViewID_, int objViewID_)
    {
        WitchController witch_ = PhotonView.Find(myViewID_).GetComponent<WitchController>();
        GameObject witchBody_ = witch_.witchBody;
        GameObject lookPoint_ = witch_.lookPoint;

        GameObject obj_ = PhotonView.Find(objViewID_).gameObject;
        obj_.AddComponent<Cube>();
        obj_.layer = LayerMask.NameToLayer("Witch");

        Effect sparkle_ = ObjPool.GetEffect(ObjPool.EffectNames.Posses);
        sparkle_.transform.position = lookPoint_.transform.position;

        if (witch_.currBody == witchBody_)
        {
            //witchBody_.SetActive(isActive_);

            Rigidbody witchRigid_ = witch_.GetComponent<Rigidbody>();
            witchRigid_.useGravity = false;
            witchRigid_.velocity = Vector3.zero;
            witchRigid_.angularVelocity = Vector3.zero;

            Collider witchCollider_ = witch_.GetComponent<Collider>();
            witchCollider_.enabled = false;

            lookPoint_.transform.SetParent(obj_.transform);
            lookPoint_.transform.position = obj_.GetComponent<Renderer>().bounds.center;

            witch_.currBody = obj_;
        }
        else
        {
            GameObject prevBody_ = lookPoint_.transform.parent.gameObject;

            lookPoint_.transform.SetParent(obj_.transform);
            lookPoint_.transform.position = obj_.GetComponent<Renderer>().bounds.center;

            witch_.currBody = obj_;

            PhotonNetwork.Destroy(prevBody_);
        }

        //SJ_230927
        ThreadManager.instance.DoRoutine(() => OnSkill(ref isMetamol_On), metamolCool);
    }

    [PunRPC]
    private void MetamorphosisPlease(int myViewID_, int objViewID_)
    {
        photonView.RPC("MetamorphosisToObj", RpcTarget.AllBufferedViaServer, myViewID_, objViewID_);
    }

    [PunRPC]
    private void MetamorphosisToObj(int myViewID_, int objViewID_)
    {
        WitchController witch_ = PhotonView.Find(myViewID_).GetComponent<WitchController>();
        GameObject witchBody_ = witch_.witchBody;
        GameObject lookPoint_ = witch_.lookPoint;

        string objName_ = PhotonView.Find(objViewID_).name.Replace("(Clone)", "");
        GameObject newBody_ = PhotonNetwork.Instantiate(objName_, lookPoint_.transform.position, lookPoint_.transform.rotation);

        newBody_.name = PhotonView.Find(objViewID_).name;
        newBody_.transform.position = lookPoint_.transform.position;
        newBody_.AddComponent<Cube>();
        newBody_.layer = LayerMask.NameToLayer("Witch");

        Effect smoke_ = ObjPool.GetEffect(ObjPool.EffectNames.Metamor);
        smoke_.transform.position = lookPoint_.transform.position;

        if (witch_.currBody == witchBody_)
        {
            //witchBody_.SetActive(isActive_);

            Rigidbody witchRigid_ = witch_.GetComponent<Rigidbody>();
            witchRigid_.useGravity = false;
            witchRigid_.velocity = Vector3.zero;
            witchRigid_.angularVelocity = Vector3.zero;

            Collider witchCollider_ = witch_.GetComponent<Collider>();
            witchCollider_.enabled = false;

            lookPoint_.transform.SetParent(newBody_.transform);
            lookPoint_.transform.position = newBody_.GetComponent<Renderer>().bounds.center;

            witch_.currBody = newBody_;
        }
        else
        {
            GameObject prevBody_ = lookPoint_.transform.parent.gameObject;

            lookPoint_.transform.SetParent(newBody_.transform);
            lookPoint_.transform.position = newBody_.GetComponent<Renderer>().bounds.center;

            witch_.currBody = newBody_;

            PhotonNetwork.Destroy(prevBody_);
        }
    }
}
