using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rigid;
    private Animator animator;
    private Transform tr;

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
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        PlayerMove();
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
}
