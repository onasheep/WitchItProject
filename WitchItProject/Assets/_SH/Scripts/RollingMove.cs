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

        // 이동에 사용할 축
        // 전후 방향에 수직이며, 시계방향으로 90도 만큼 돌아간 축
        Vector3 torqueAxis_ = myCamera.transform.right;
        torqueAxis_.y = 0;

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
    }

    void MoveHorizontal()
    {
        float movePowerH_ = Input.GetAxisRaw("Horizontal");

        // 이동에 사용할 축
        // 벡터 상 반대의 방향을 갖는 축
        Vector3 torqueAxis_ = myCamera.transform.forward;
        torqueAxis_.y = 0;

        if (movePowerH_ > 0)
        {
            // 축에 시계방향으로 회전
            rigid.AddTorque(torqueAxis_ * -50);
        }
        else if (movePowerH_ < 0)
        {
            // 축에 반시계방향으로 회전
            rigid.AddTorque(torqueAxis_ * 50);
        }
    }
}
