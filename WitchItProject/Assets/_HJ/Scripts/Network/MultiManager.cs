using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using static Singleton;
using ExitGames.Client.Photon;



public class MultiManager : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public List<PlayerInfo> playerInfos;
    public bool isStart, isEnd;


    void MasterInitPlayerInfo()
    {
        // ���ӽ��۽� �ʱ�ȭ
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            Photon.Realtime.Player player = PhotonNetwork.PlayerList[i];
            playerInfos.Add(new PlayerInfo(player.NickName, player.ActorNumber, 0, PhotonNetwork.Time + 3.0, false));
        }
        MasterSendPlayerInfo(INIT);
    }

    void MasterRemovePlayerInfo(int actorNum)
    {
        // OnPlayerLeftRoom���� ���� ������� �÷��̾� ����
        PlayerInfo playerInfo = playerInfos.Find(x => x.actorNum == actorNum);
        playerInfos.Remove(playerInfo);
        MasterSendPlayerInfo(REMOVE);
    }

    [PunRPC]
    public void MasterReceiveRPC(byte code, int actorNum, int colActorNum)
    {
        // �ð��� ��������
        PlayerInfo playerInfo = playerInfos.Find(x => x.actorNum == actorNum);
        double lifeTime = PhotonNetwork.Time - playerInfo.lifeTime;
        lifeTime = System.Math.Truncate(lifeTime * 100) * 0.01;
        playerInfo.lifeTime = lifeTime;
        playerInfo.isDie = true;


        // �ڱⰡ �ƴ� �����̳� �÷��̾� �浹�� ų���� ����
        if (code == DIE)
        {
            playerInfo = null;
            playerInfo = playerInfos.Find(x => x.actorNum == colActorNum);
            ++playerInfo.killDeath;
        }

        MasterSendPlayerInfo(code);
    }



    void MasterSendPlayerInfo(byte code)
    {
        // ������ PlayerInfo ���� �� ������
        playerInfos.Sort((p1, p2) => p2.lifeTime.CompareTo(p1.lifeTime));

        string jdata = JsonUtility.ToJson(new Serialization<PlayerInfo>(playerInfos));
        PV.RPC("OtherReceivePlayerInfoRPC", RpcTarget.Others, code, jdata);
    }

    [PunRPC]
    void OtherReceivePlayerInfoRPC(byte code, string jdata)
    {
        // �ٸ� ����� PlayerInfo �ޱ�
        playerInfos = JsonUtility.FromJson<Serialization<PlayerInfo>>(jdata).target;
    }

    [PunRPC]
    void StartSyncRPC()
    {
        isStart = true;
    }

    IEnumerator Loading()
    {
        S.SetTag("loadScene", true);
        while (!S.AllhasTag("loadScene")) yield return null;

        // ��� ���� �־�� ������ �� ����, �����Ϳ� Ŭ��� �����Ͱ� ������
        //PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-5f, 5f), 0, 0), QI);

        //while (!S.AllhasTag("loadPlayer")) yield return null;
    }

    IEnumerator Start()
    {
        yield return Loading();

        if (S.master())
        {
            MasterInitPlayerInfo();
            yield return new WaitForSeconds(3);
            PV.RPC("StartSyncRPC", RpcTarget.AllViaServer);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        MasterInitPlayerInfo();
       
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        // �����Ͱ� ������ �ٲ� �����Ͱ� ȣ��Ǽ� ����
        //if (S.master())
        //{
        //    MasterRemovePlayerInfo(otherPlayer.ActorNumber);
        //} 
        MasterRemovePlayerInfo(otherPlayer.ActorNumber);
    }

}