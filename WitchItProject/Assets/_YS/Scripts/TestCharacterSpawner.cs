using Photon.Pun;
using UnityEngine;

public class TestCharacterSpawner : MonoBehaviourPunCallbacks
{
    public Transform[] spawnPoints;

    private void Start()
    {


        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.CreateHunter(); // ÇåÅÍ »ý¼º
                                        
        }

    }
}