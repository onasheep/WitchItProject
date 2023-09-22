using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestOutLine : MonoBehaviour
{
    // Start is called before the first frame update
    Ray ray;
    
    public LayerMask mask;
    public GameObject obj;
    void Start()
    {
        mask = LayerMask.GetMask("ChangeableObjects");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        ray = new Ray(obj.transform.position, obj.transform.forward);

        Debug.LogFormat("{0}", mask);
        if (Physics.Raycast(this.transform.position,this.transform.forward, out hit, Mathf.Infinity, mask))
        {
            Debug.LogFormat("{0}", hit.transform == null);

            Debug.LogFormat("{0}", hit.transform.name);

            Debug.LogFormat("{0}", hit.transform.GetComponent<MeshRenderer>() == null);
            //Debug.LogFormat("{0}", hit.transform.GetComponent<MeshRenderer>().material == null);
            //Debug.LogFormat("{0}", hit.transform.GetComponent<MeshRenderer>().materials[0] == null);
            //Debug.LogFormat("{0}", hit.transform.GetComponent<MeshRenderer>().materials[1] == null);

            //hit.transform.GetComponent<MeshRenderer>().materials[1].SetFloat("_Outline", 0.05f);

        }



    }
}
