using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetChangeableObject : MonoBehaviourPun
{
    public List<GameObject> changeableObjs = default;
    public List<Transform> spawnPoints = default;

    private void Start()
    {
        photonView.RPC("SetObjs", RpcTarget.MasterClient, 100);
    }

    [PunRPC]
    private void SetObjs(int count_)
    {
        for (int i = 0; i < count_; ++i)
        {
            int objIdx_ = Random.Range(0, changeableObjs.Count);
            int pointIdx_ = Random.Range(0, spawnPoints.Count);

            GameObject obj_ = PhotonNetwork.Instantiate(changeableObjs[objIdx_].name, spawnPoints[pointIdx_].GetComponent<Collider>().bounds.center + new Vector3(0, 2, 0), Quaternion.identity);

            obj_.name = changeableObjs[objIdx_].name;
            obj_.transform.SetParent(transform, true);
        }
    }
}
