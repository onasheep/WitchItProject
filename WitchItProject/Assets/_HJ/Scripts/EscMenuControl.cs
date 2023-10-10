using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscMenuControl : MonoBehaviour
{
    [SerializeField] GameObject firstPanel;
    // { SJ_ 231010 
    [SerializeField] GameObject graphicsCanvas;
    // } SJ_ 231010 

    //[SerializeField] GameObject settingPanel;
    [SerializeField] Canvas myCanvas;

    [SerializeField] Button continueBtn;
    //[SerializeField] Button settingBtn;
    [SerializeField] Button lobbyBtn;


    [SerializeField] private bool isEscMenu = false;
    [SerializeField] private bool isSetting = false;

    private float btnMaxSize = 1.2f;
    private float btnMinSize = 1.0f;

    void Start()
    {
        firstPanel = transform.GetChild(0).gameObject;
        //settingPanel = transform.GetChild(1).gameObject;
        myCanvas = GetComponent<Canvas>();

        continueBtn =  firstPanel.transform.GetChild(0).GetComponent<Button>();
        //settingBtn = firstPanel.transform.GetChild(1).GetComponent<Button>();
        lobbyBtn = firstPanel.transform.GetChild(4).GetComponent<Button>();

        //settingPanel.SetActive(false);

        continueBtn.onClick.AddListener(() => ContinueGame());
        //settingBtn.onClick.AddListener(() => OpenSetting());
        lobbyBtn.onClick.AddListener(() => GoLobby());
    }

    void Update()
    {
        OpenEscMenu();
        //OffSetting();
    }


    void OpenEscMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isEscMenu)
            {
                isEscMenu = false;
            }
            else if (!isEscMenu)
            {
                isEscMenu = true;
            }
        }

        if (isEscMenu)
        {
            myCanvas.enabled = true;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.DrawRay(Input.mousePosition, Vector3.forward, Color.blue);
                Debug.Log(hit.transform.gameObject.name);
                if (hit.transform.gameObject.layer == 5)
                {
                    StartCoroutine(ButtonSizeUp(hit.transform.gameObject));
                }
            }
        }
        else if (!isEscMenu)
        {
            myCanvas.enabled =false;
        }
    }


    IEnumerator ButtonSizeUp(GameObject myBtn)
    {
        RectTransform myButton = myBtn.GetComponent<RectTransform>();

        for (float i = 1.0f; i <btnMaxSize; i += 0.01f)
        {
            myButton.localScale = new Vector3(myButton.localScale.x, i, myButton.localScale.z);
            yield return null;
        }
        for (float i = btnMaxSize; i > btnMinSize; i -= 0.01f)
        {

            myButton.localScale = new Vector3(myButton.localScale.x, i, myButton.localScale.z);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
    }

    void OffSetting()
    {
        if (isSetting)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isSetting = false;
            }
        }
    }
    void ContinueGame()
    {
        isEscMenu = false;
    }

    void OpenSetting()
    {
        isEscMenu = false;
        firstPanel.SetActive(false);
        isSetting = true;
        //settingPanel.SetActive(true);
    }

    void GoLobby()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    // SJ_ 231010
    // { Graphics Setting On / Off
    public void OnGraphicsSetting()
    {

        graphicsCanvas.SetActive(true);
    }

    public void OffGraphicsSetting()
    {

        graphicsCanvas.SetActive(false);
    }
    // } Graphics Setting On / Off
}
