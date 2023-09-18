using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscMenuControl : MonoBehaviour
{
    [SerializeField] GameObject firstPanel;
    //[SerializeField] GameObject secondPanel;
    [SerializeField] Canvas myCanvas;

    [SerializeField] private bool isEscMenu = false;

    private float btnMaxSize = 1.2f;
    private float btnMinSize = 1.0f;

    void Start()
    {
        //firstPanel = transform.GetChild(0).gameObject;
        //secondPanel = transform.GetChild(1).gameObject;
        myCanvas = GetComponent<Canvas>();
    }

    void Update()
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
}
