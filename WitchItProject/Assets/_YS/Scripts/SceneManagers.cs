using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    public void CustomHunter(int CustomHunterScene)
    {
        SceneManager.LoadScene(CustomHunterScene);
    }

    public void CustomWitch(int CustomWitchScene)
    {
        SceneManager.LoadScene(CustomWitchScene);
    }

    public void CustomLobby(int CustomLobbyScene)
    {
        SceneManager.LoadScene(CustomLobbyScene);
    }

    public void Lobby(int LobbyScene)
    {
        SceneManager.LoadScene(LobbyScene);
    }
}