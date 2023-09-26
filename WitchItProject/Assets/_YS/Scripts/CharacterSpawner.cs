using UnityEngine;
using Photon.Pun;

public class CharacterSpawner : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public Transform[] spawnPoints;

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[spawnPointIndex].position, Quaternion.identity);

        // 활성화할 스크립트와 PhotonView를 찾습니다.
        MonoBehaviourPun[] scriptsToActivate = player.GetComponents<MonoBehaviourPun>();
        //playerbase가 MonoBehaviourPun 써서
        PhotonView photonViewToActivate = player.GetComponent<PhotonView>();

        // 스크립트와 PhotonView를 활성화합니다.
        foreach (MonoBehaviour script in scriptsToActivate)
        {
            script.enabled = true;
        }
        photonViewToActivate.enabled = true;
    }
}