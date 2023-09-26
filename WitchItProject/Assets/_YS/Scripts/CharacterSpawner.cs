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

        // Ȱ��ȭ�� ��ũ��Ʈ�� PhotonView�� ã���ϴ�.
        MonoBehaviourPun[] scriptsToActivate = player.GetComponents<MonoBehaviourPun>();
        //playerbase�� MonoBehaviourPun �Ἥ
        PhotonView photonViewToActivate = player.GetComponent<PhotonView>();

        // ��ũ��Ʈ�� PhotonView�� Ȱ��ȭ�մϴ�.
        foreach (MonoBehaviour script in scriptsToActivate)
        {
            script.enabled = true;
        }
        photonViewToActivate.enabled = true;
    }
}