using Photon.Pun;
using UnityEngine;

public class RollingMove : MonoBehaviourPun
{
    public Transform myCamera;

    private Rigidbody rigid;

    public WitchController myWitchCon;

    private void Start()
    {
        myCamera = GameObject.Find("WitchCamera").transform;
        rigid = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

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
