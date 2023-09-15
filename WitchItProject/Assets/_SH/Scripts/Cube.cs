using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public Transform myCamera;
    //public GameObject followTarget;

    private Rigidbody rigid;

    private Vector3 direction;

    private void Start()
    {
        myCamera = GameObject.Find("WitchCamera").transform;
        //followTarget = FindObjectOfType<WitchController>().gameObject;
        rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MoveVertical();
        MoveHorizontal();
    }

    void MoveVertical()
    {
        float movePowerV_ = Input.GetAxisRaw("Vertical");

        // �̵��� ����� ��
        // ���� ���⿡ �����̸�, �ð�������� 90�� ��ŭ ���ư� ��
        Vector3 torqueAxis_ = myCamera.transform.right;
        torqueAxis_.y = 0;

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

    void MoveHorizontal()
    {
        float movePowerH_ = Input.GetAxisRaw("Horizontal");

        // �̵��� ����� ��
        // ���� �� �ݴ��� ������ ���� ��
        Vector3 torqueAxis_ = myCamera.transform.forward;
        torqueAxis_.y = 0;

        if (movePowerH_ > 0)
        {
            // �࿡ �ð�������� ȸ��
            rigid.AddTorque(torqueAxis_ * -50);
        }
        else if (movePowerH_ < 0)
        {
            // �࿡ �ݽð�������� ȸ��
            rigid.AddTorque(torqueAxis_ * 50);
        }
    }
}
