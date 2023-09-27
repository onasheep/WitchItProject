using Cinemachine;
using Photon.Pun;
using System;
using UnityEngine;

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


    // 변신한 물체에 따라 바뀔 예정 const X
    private float healthMax = 100;
    private float health;

    // 변신 여부
    public bool isMetamor = false;

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
            CancelMetamorphosis();
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            DieWitch();
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
                        isMetamol_On = false;
                        MetamorphosisToObj(hit.collider.gameObject);
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
                    PossesionToObj(hit.collider.gameObject);
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


        if (isMetamor) { /* Do Nothing */ }
        else if (!isMetamor)
        {
            Vector3 forwardLook = new Vector3(myCamera.forward.x, 0, myCamera.forward.z);
            Vector3 moveDirection = forwardLook * verticalMove + myCamera.right * horizontalMove;

            Vector3 dirVelocity = moveDirection * MOVESPEED;

            dirVelocity.y = rigid.velocity.y;
            rigid.velocity = dirVelocity;
        }

    }

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


    private void CancelMetamorphosis()
    {

        if (witchBody.activeInHierarchy)
        {
            return;
        }
        else if (!witchBody.activeInHierarchy)
        {
            transform.position = lookPoint.transform.position;

            witchBody.SetActive(true);

            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Collider>().enabled = true;

            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;

            GameObject prevBody_ = lookPoint.transform.parent.gameObject;

            lookPoint.transform.SetParent(transform);
            lookPoint.transform.localPosition = new Vector3(0, 1.379f, 0);

            Destroy(prevBody_);

            currBody = witchBody;
        }


        GetComponent<WitchController>().isMetamor = false;
    }

    private void PossesionToObj(GameObject obj_)
    {
        obj_.AddComponent<Cube>();
        obj_.layer = LayerMask.NameToLayer("Witch");

        if (currBody == witchBody)
        {
            currBody.SetActive(false);

            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Collider>().enabled = false;

            lookPoint.transform.SetParent(obj_.transform);
            lookPoint.transform.position = obj_.GetComponent<Renderer>().bounds.center;

            currBody = obj_;
        }
        else
        {
            GameObject prevBody_ = lookPoint.transform.parent.gameObject;

            lookPoint.transform.SetParent(obj_.transform);
            lookPoint.transform.position = obj_.GetComponent<Renderer>().bounds.center;

            Destroy(prevBody_);

            currBody = obj_;
        }

        GetComponent<WitchController>().isMetamor = true;

        //SJ_230927
        ThreadManager.instance.DoRoutine(() => OnSkill(ref isMetamol_On), metamolCool);
    }

    private void MetamorphosisToObj(GameObject obj_)
    {
        GameObject newBody_ = Instantiate(obj_);

        newBody_.layer = LayerMask.NameToLayer("Witch");

        newBody_.transform.position = lookPoint.transform.position;
        newBody_.AddComponent<Cube>();

        Effect smoke_ = ObjPool.GetEffect(ObjPool.EffectNames.Metamor);
        smoke_.gameObject.transform.position = lookPoint.transform.position;

        if (currBody == witchBody)
        {
            currBody.SetActive(false);

            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Collider>().enabled = false;

            lookPoint.transform.SetParent(newBody_.transform);
            lookPoint.transform.position = newBody_.GetComponent<Renderer>().bounds.center;

            currBody = newBody_;
        }
        else
        {
            GameObject prevBody_ = lookPoint.transform.parent.gameObject;

            lookPoint.transform.SetParent(newBody_.transform);
            lookPoint.transform.position = newBody_.GetComponent<Renderer>().bounds.center;

            Destroy(prevBody_);

            currBody = newBody_;
        }

        GetComponent<WitchController>().isMetamor = true;

    }

    private void ShootRay()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (hit.collider != null)
            {
                MetamorphosisToObj(hit.collider.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hit.collider != null)
            {
                PossesionToObj(hit.collider.gameObject);
            }
        }
    }


}
