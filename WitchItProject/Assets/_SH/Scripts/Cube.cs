using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public Transform myCamera;
    public GameObject followTarget;

    private Rigidbody rigid;

    private void Start()
    {
        myCamera = GameObject.Find("PlayerCamera").transform;
        followTarget = FindObjectOfType<WitchController>().gameObject;
        rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // 카메라가 바라보는 방향
        Vector3 direction_ = (transform.position - myCamera.transform.position).normalized;

        MoveVertical(direction_);
        MoveHorizontal(direction_);
    }

    void MoveVertical(Vector3 direction_)
    {
        float movePowerV_ = Input.GetAxisRaw("Vertical");

        // 이동에 사용할 축
        // 전후 방향에 수직이며, 시계방향으로 90도 만큼 돌아간 축
        Vector3 torqueAxis_ = new Vector3(-direction_.x, 0, direction_.z);

        if (movePowerV_ > 0)
        {
            // 축에 시계방향으로 회전
            rigid.AddTorque(torqueAxis_ * 50);
        }
        else if (movePowerV_ < 0)
        {
            // 축에 반시계방향으로 회전
            rigid.AddTorque(torqueAxis_ * -50);
        }

        //Vector3 direction = (followTarget.transform.position - transform.position).normalized;

        //Vector3 torqueAxis = new Vector3(direction.z, direction.y, -direction.x);

        //rigid.AddTorque(torqueAxis * 10);
    }

    void MoveHorizontal(Vector3 direction_)
    {
        float movePowerH_ = Input.GetAxisRaw("Horizontal");

        // 이동에 사용할 축
        // 벡터 상 반대의 방향을 갖는 축
        Vector3 torqueAxis_ = new Vector3(-direction_.x, 0, -direction_.z);

        if (movePowerH_ > 0)
        {
            // 축에 시계방향으로 회전
            rigid.AddTorque(torqueAxis_ * 50);
        }
        else if (movePowerH_ < 0)
        {
            // 축에 반시계방향으로 회전
            rigid.AddTorque(torqueAxis_ * -50);
        }
    }
}
