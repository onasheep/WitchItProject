//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Photon.Pun;
//using Photon.Realtime;


//public class Customize : MonoBehaviourPun
//{
//    public GameObject player;
//    public GameObject[] customs;
//    private int currentCustom;
//    private new PhotonView photonView;

//    // Start is called before the first frame update
//   void Start()
//   {


//            if (photonView.IsMine)
//            {
//                // PlayerPrefs에서 외형 인덱스를 불러옵니다.
//                currentCustom = PlayerPrefs.GetInt("CustomIndex", 0);
//                photonView.RPC("UpdateCustom", RpcTarget.AllBuffered, currentCustom);
//            }
//    }




//    // Update is called once per frame
//    void Update()
//    {
//        UpdateCustoms();
//    }

//    public void SwitchCustom()
//    {
//        if (photonView.IsMine)
//        {
//            if (currentCustom == customs.Length - 1)
//            {
//                currentCustom = 0;
//            }
//            else
//            {
//                currentCustom++;
//            }

//            photonView.RPC("UpdateCustom", RpcTarget.AllBuffered, currentCustom);
//        }
//    }

//    [PunRPC]
//    void UpdateCustom(int newCustom)
//    {
//        currentCustom = newCustom;
//        UpdateCustoms();
//    }

//    private void UpdateCustoms()
//    {
//        for (int i = 0; i < customs.Length; i++)
//        {
//            customs[i].SetActive(i == currentCustom);
//        }
//    }

//    //커스텀 저장 버튼
//    public void OnCustomizeButtonClick()
//    {
//        if (photonView.IsMine)
//        {
//            SwitchCustom();

//            // PlayerPrefs에 외형 인덱스를 저장
//            PlayerPrefs.SetInt("CustomIndex", currentCustom);
//            PlayerPrefs.Save();
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Customize : MonoBehaviourPun
{
    public string[] customResourceNames;
    public GameObject[] customs;
    private int currentCustom;

    // Start is called before the first frame update
    void Start()
    {
        //if (photonView.IsMine)
        //{
        //    LoadCustom();
        //    InstantiateCustom();
        //}
       
    }

   

    public void SwitchCustom()
    {
        if (currentCustom == customResourceNames.Length - 1)
        {
            currentCustom = 0;
        }
        else
        {
            currentCustom++;
        }
    }
    void UpdateCustoms()
    {
        for (int i = 0; i < customs.Length; i++)
        {
            if (i == currentCustom)
            {
                customs[i].SetActive(true);
            }
            else
            {
                customs[i].SetActive(false);
            }
        }
    }

    public void SaveCustom()
    {
        PlayerPrefs.SetInt("SelectedCustom", currentCustom);
        PlayerPrefs.Save();
    }

    public void LoadCustom()
    {
        if (PlayerPrefs.HasKey("SelectedCustom"))
        {
            currentCustom = PlayerPrefs.GetInt("SelectedCustom");
        }
    }

    void InstantiateCustom()
    {
        string resourceName = customResourceNames[currentCustom];
        GameObject customPrefab = Resources.Load<GameObject>(resourceName);
        GameObject customInstance = PhotonNetwork.Instantiate(customPrefab.name, Vector3.zero, Quaternion.identity);
    }
}