using Photon.Pun;
using UnityEngine;

public class CharacterSpawner : MonoBehaviourPunCallbacks
{
    public GameObject customHunterPrefab;
    public Transform[] spawnPoints;

    public string hunterScriptName = "Hunter";

    private void Start()
    {
        customHunterPrefab = Resources.Load<GameObject>("CustomHunter");

        if (PhotonNetwork.IsConnectedAndReady)
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        int hunterSpawnPoint = 2;
        GameObject player = PhotonNetwork.Instantiate(customHunterPrefab.name, spawnPoints[hunterSpawnPoint].position, spawnPoints[hunterSpawnPoint].rotation);

        // Hunter ��ũ��Ʈ�� �߰��մϴ�.
        System.Type hunterScriptType = System.Type.GetType(hunterScriptName);
        if (hunterScriptType != null)
        {
            MonoBehaviour hunterScriptInstance = (MonoBehaviour)player.AddComponent(hunterScriptType);
            hunterScriptInstance.enabled = true;
        }
        else
        {
            Debug.LogError("��ũ��Ʈ�� ã�� �� �����ϴ�: " + hunterScriptName);
        }

        // PhotonView, PhotonTransformView, PhotonAnimatorView ������Ʈ�� �߰��մϴ�.
        PhotonView photonView = player.AddComponent<PhotonView>();
        PhotonTransformView photonTransformView = player.AddComponent<PhotonTransformView>();
        PhotonAnimatorView photonAnimatorView = player.AddComponent<PhotonAnimatorView>();

        // �ʿ��� ��� �߰� ������ ���⿡ �ۼ��մϴ�.
    }

}