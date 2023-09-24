using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
//using static Singleton;
using ExitGames.Client.Photon;

public class MultiManager : MonoBehaviourPunCallbacks
{
    public PhotonView myPV;
    public List<Player> playerInfos;
    
    public bool isStart; 
    public bool isEnd;

    //void MasterInitPlayerInfo()
    //{
    //    //게임을 시작하면 초기화 해주는 부분입니다.
    //    for (int i  = 0; i < PhotonNetwork.PlayerList.Length; i++)
    //    {
    //        Player player = PhotonNetwork.PlayerList[i];
    //        playerInfos.Add(new PlayerInfo(player.NickName, player.ActorNumber, 0, PhotonNetwork.Time + 3.0, false));
    //    }
    //    MasterSendPlayerInfo(INIT);
    //}


}
