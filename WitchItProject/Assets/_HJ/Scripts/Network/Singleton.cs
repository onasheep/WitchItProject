using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class Singleton : MonoBehaviourPun
{
    public const byte INIT = 0, REMOVE = 1, DIEWALL = 2, DIE = 3;
    public static readonly Quaternion QI = Quaternion.identity;



    #region ½Ì±ÛÅæ
    public static Singleton S;
    void Awake()
    {
        if (null == S)
        {
            S = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);
    }
    #endregion



    #region Set Get
    public bool master() => PhotonNetwork.LocalPlayer.IsMasterClient;

    public int actorNum(Photon.Realtime.Player player = null)
    {
        if (player == null) player = PhotonNetwork.LocalPlayer;
        return player.ActorNumber;
    }

    public void destroy(List<GameObject> GO)
    {
        for (int i = 0; i < GO.Count; i++) PhotonNetwork.Destroy(GO[i]);
    }

    public void SetPos(Transform Tr, Vector3 target)
    {
        Tr.position = target;
    }

    public void SetTag(string key, object value, Photon.Realtime.Player player = null)
    {
        if (player == null) player = PhotonNetwork.LocalPlayer;
        player.SetCustomProperties(new Hashtable { { key, value } });
    }

    public object GetTag(Photon.Realtime.Player player, string key)
    {
        if (player.CustomProperties[key] == null) return null;
        return player.CustomProperties[key].ToString();
    }

    public bool AllhasTag(string key)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            if (PhotonNetwork.PlayerList[i].CustomProperties[key] == null) return false;
        return true;
    }
    #endregion



    void Setting()
    {
        Screen.SetResolution(960, 540, false);
        //PhotonNetwork.NickName = "ÇÃ·¹ÀÌ¾î" + Random.Range(0, 100);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.SendRate = 40;
        PhotonNetwork.SerializationRate = 20;
        //PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        Setting();
    }

}