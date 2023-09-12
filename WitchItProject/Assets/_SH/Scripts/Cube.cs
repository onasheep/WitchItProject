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
        // ī�޶� �ٶ󺸴� ����
        Vector3 direction_ = (transform.position - myCamera.transform.position).normalized;

        MoveVertical(direction_);
        MoveHorizontal(direction_);
    }

    void MoveVertical(Vector3 direction_)
    {
        float movePowerV_ = Input.GetAxisRaw("Vertical");

        // �̵��� ����� ��
        // ���� ���⿡ �����̸�, �ð�������� 90�� ��ŭ ���ư� ��
        Vector3 torqueAxis_ = new Vector3(-direction_.x, 0, direction_.z);

        if (movePowerV_ > 0)
        {
            // �࿡ �ð�������� ȸ��
            rigid.AddTorque(torqueAxis_ * 50);
        }
        else if (movePowerV_ < 0)
        {
            // �࿡ �ݽð�������� ȸ��
            rigid.AddTorque(torqueAxis_ * -50);
        }

        //Vector3 direction = (followTarget.transform.position - transform.position).normalized;

        //Vector3 torqueAxis = new Vector3(direction.z, direction.y, -direction.x);

        //rigid.AddTorque(torqueAxis * 10);
    }

    void MoveHorizontal(Vector3 direction_)
    {
        float movePowerH_ = Input.GetAxisRaw("Horizontal");

        // �̵��� ����� ��
        // ���� �� �ݴ��� ������ ���� ��
        Vector3 torqueAxis_ = new Vector3(-direction_.x, 0, -direction_.z);

        if (movePowerH_ > 0)
        {
            // �࿡ �ð�������� ȸ��
            rigid.AddTorque(torqueAxis_ * 50);
        }
        else if (movePowerH_ < 0)
        {
            // �࿡ �ݽð�������� ȸ��
            rigid.AddTorque(torqueAxis_ * -50);
        }
    }
}
