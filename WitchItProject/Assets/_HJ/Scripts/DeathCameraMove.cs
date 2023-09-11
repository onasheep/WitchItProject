using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCameraMove : MonoBehaviour
{
    [SerializeField] private Transform myCamera;

    [SerializeField] private Rigidbody myRigid;

    [SerializeField][Range(0f, 10f)] private float rotationSpeed = 5f;
    [SerializeField][Range(0f, 10f)] private float moveSpeed = 5f;
    [SerializeField][Range(0f, 10f)] private float jumpForce = 5f;
    // Start is called before the first frame update
    void Start()
    {
        myCamera = GameObject.Find("DeathCamera").GetComponent<CinemachineVirtualCamera>().transform;
        myRigid = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        DeathMove();
        DeathRotate();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            UpCamera();
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            DownCamera();
        }
        else 
        {
            StopMove();
        }
    }

    void DeathMove()
    {
        float moveDirectionZ = Input.GetAxisRaw("Vertical");
        float moveDirectionX = Input.GetAxisRaw("Horizontal");

        Vector3 forwardLook = new Vector3(myCamera.forward.x, 0, myCamera.forward.z);
        Vector3 moveDirection = forwardLook * moveDirectionZ + myCamera.right * moveDirectionX;

        Vector3 dirVelocity = moveDirection * moveSpeed;


        dirVelocity.y = myRigid.velocity.y;
        myRigid.velocity = dirVelocity;

    }
    void UpCamera()
    {
        Vector3 testVelocity = transform.up * jumpForce;
        myRigid.velocity = testVelocity;
        //myRigid.AddForce(transform.up * jumpForce);
    }
    void DownCamera()
    {
        Vector3 testVelocity = -transform.up * jumpForce;
        myRigid.velocity = testVelocity;
        //myRigid.AddForce(-transform.up * jumpForce);
    }
    void StopMove()
    {
        myRigid.velocity = Vector3.zero;
    }
    void DeathRotate()
    {
        float moveDirectionX = Input.GetAxis("Horizontal");
        float moveDirectionZ = Input.GetAxis("Vertical");

        Vector3 forwardLook = new Vector3(myCamera.forward.x, 0, myCamera.forward.z);
        Vector3 moveDirection = forwardLook * moveDirectionZ + myCamera.right * moveDirectionX;

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
    }


}
