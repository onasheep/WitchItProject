using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ExitGames.Client.Photon;

public class LobbyMenuControl : MonoBehaviour
{
    [SerializeField] GameObject firstPanel;
    [SerializeField] GameObject secondPanel;
    [SerializeField] Canvas myCanvas;

    private float btnMaxSize = 1.2f;
    private float btnMinSize = 1.0f;

    //[SerializeField] List<GameObject> myBtnObjects = new List<GameObject>();
    

    void Start()
    {
        firstPanel = transform.GetChild(0).gameObject;
        //secondPanel = transform.GetChild(1).gameObject;
        myCanvas = GetComponent<Canvas>();

        //myBtnObjects = gameObject.transform.GetChild(0).gameObject.GetChildrenObjs();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //Debug.DrawRay(Input.mousePosition, Vector3.forward, Color.blue);
            //Debug.Log(hit.transform.gameObject.name);
            if (hit.transform.gameObject.layer == 5)
            {
                StartCoroutine(ButtonSizeUp(hit.transform.gameObject));
            }
        }
    }
    public void PlayButton()
    {
        firstPanel.SetActive(false);
        //secondPanel.SetActive(true);
    }

    public void BackButton()
    {
        secondPanel.SetActive(false);
        firstPanel.SetActive(true);
    }

    public void ExitGame()
    {
        //���� ����
        Debug.Log("������ �����մϴ�");
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

    //TODO �ϴ��� ���ܳ��µ� ���߿� ���� �����Դϴ�.
    //public void ChangeBtnScale()
    //{
    //    StartCoroutine(ButtonSizeUp("CollectionButton"));
    //}
    //�̸����� ã�Ƽ� ��ư ũ�� �ٲٴ°̴ϴ�. 
    //IEnumerator ButtonSizeUp(string buttonName)
    //{
    //    RectTransform myButton = default;

    //    for (int i = 0; i < myBtnObjects.Count; i++)
    //    {
    //        if (myBtnObjects[i].name == buttonName)
    //        {
    //            myButton = myBtnObjects[i].GetComponent<RectTransform>(); 
    //        }
    //    }

    //    Debug.Log("�̰� ��?");
    //    for(float i = 1.0f; i <btnMaxSize; i += 0.01f)
    //    {

    //        myButton.localScale = new Vector3(myButton.localScale.x, i, myButton.localScale.z);
    //        yield return null;
    //    }
    //    //yield return new WaitForSeconds(1f);

    //    for (float i = btnMaxSize; i > btnMinSize; i -= 0.01f)
    //    {

    //        myButton.localScale = new Vector3(myButton.localScale.x, i, myButton.localScale.z);
    //        yield return null;
    //    }
    //    yield return new WaitForSeconds(1f);
    //}


}
