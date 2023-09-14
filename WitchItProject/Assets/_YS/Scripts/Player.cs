using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    private Rigidbody rigid;
    private Animator animator;
    private Transform tr;
    private new Camera camera;

    private PhotonView pv;

    private CinemachineVirtualCamera virtualCamera;

    public float speed = 4.0f;
    public float turnSpeed = 360.0f;

    private float h;
    private float v;
    private float x;

    Vector3 moveVec;
    Vector3 rotateVec;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        camera = Camera.main;

        pv = GetComponent<PhotonView>();
        virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();

        if(pv.IsMine)
        {
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pv.IsMine)
        {
            PlayerInput();
            PlayerMove();
            PlayerTurn();
        }
        
    }

    void PlayerInput()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        x = Input.GetAxis("Mouse X");
    }

    void PlayerMove()
    {
        Vector3 moveVec = new Vector3(h, 0, v);
        tr.Translate(moveVec * speed * Time.deltaTime);

        Vector3 rotateVec = new Vector3(0, x, 0);
        tr.Rotate(rotateVec * turnSpeed * Time.deltaTime);

        animator.SetBool("isRun", moveVec != Vector3.zero);
    }

    void PlayerTurn()
    {
        transform.LookAt(transform.position + moveVec);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        
    }
}
