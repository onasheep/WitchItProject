using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static Singleton;

public class PlayerScript : MonoBehaviourPun, IPunObservable
{
    MultiManager MM;
    PhotonView PV;
    Vector3 curPos;
    bool isDie;
    List<GameObject> Lines = new List<GameObject>();



    void Init()
    {
        MM = FindObjectOfType<MultiManager>();
        S.SetTag("loadPlayer", true);
        PV = photonView;
    }


    void Start()
    {
        Init();

        if (!PV.IsMine) return;

    }



    bool forbidden()
    {
        return !PV.IsMine || !MM.isStart || isDie;
    }

    void Update()
    {
       
        if (forbidden()) return;

    }



    void OtherSendMaster(PhotonView colPV)
    {
        // �ڱⰡ �ƴ� �����̳� �÷��̾� �浹
        if (colPV != null && S.actorNum() != colPV.Owner.ActorNumber)
            MM.PV.RPC("MasterReceiveRPC", RpcTarget.MasterClient, DIE, S.actorNum(), colPV.Owner.ActorNumber);

        // �� �浹
        else MM.PV.RPC("MasterReceiveRPC", RpcTarget.MasterClient, DIEWALL, S.actorNum(), 0);
    }

    public void OnTriggerStay(Collider col)
    {
        // �浹�� �������� ����
        if (forbidden()) return;
        isDie = true;

        OtherSendMaster(col.GetComponent<PhotonView>());
        
        // TEST : 
        // ���� �߰� 
        //S.SetPos(transform, new Vector3(0, 100, 0));
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
            stream.SendNext(transform.position);
        else
            curPos = (Vector3)stream.ReceiveNext();
    }
}
