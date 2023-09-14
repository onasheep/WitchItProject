using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beartrap : MonoBehaviour
{
    // Start is called before the first frame update
    private List<GameObject> trapJaws;
    void Start()
    {
        trapJaws = this.gameObject.GetChildrenObjs();
    }
    void Update()
    {
            
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Witch"))
        {
            Debug.Log("got");
        }
    }
}
