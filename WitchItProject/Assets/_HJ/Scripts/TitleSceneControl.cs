using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneControl : MonoBehaviour
{
    [SerializeField] private GameObject introCanvas;
    public bool isPress = default;
    private
    // Start is called before the first frame update
    void Start()
    {
        introCanvas = GameObject.Find("IntroCanvas");
        
    }

    private void FixedUpdate()
    {
        isPress = introCanvas.GetComponent<TitleOnOffControl>().isIntro;

    }
    // Update is called once per frame
    void Update()
    {
        if (isPress == false)
        {
            SceneManager.LoadScene("LobbyScene");
        }
    }
}
