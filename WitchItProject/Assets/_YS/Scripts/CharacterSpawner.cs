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

        // Hunter 스크립트를 추가합니다.
        System.Type hunterScriptType = System.Type.GetType(hunterScriptName);
        if (hunterScriptType != null)
        {
            MonoBehaviour hunterScriptInstance = (MonoBehaviour)player.AddComponent(hunterScriptType);
            hunterScriptInstance.enabled = true;
        }
        else
        {
            Debug.LogError("스크립트를 찾을 수 없습니다: " + hunterScriptName);
        }

        // PhotonView, PhotonTransformView, PhotonAnimatorView 컴포넌트를 추가합니다.
        PhotonView photonView = player.AddComponent<PhotonView>();
        PhotonTransformView photonTransformView = player.AddComponent<PhotonTransformView>();
        PhotonAnimatorView photonAnimatorView = player.AddComponent<PhotonAnimatorView>();

        // 필요한 경우 추가 설정을 여기에 작성합니다.
    }

}