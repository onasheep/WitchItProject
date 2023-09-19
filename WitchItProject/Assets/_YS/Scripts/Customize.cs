using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Customize : MonoBehaviour
{
    public GameObject[] customs;
    private int currentCustom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i <customs.Length; i++)
        {
            if(i == currentCustom)
            {
                customs[i].SetActive(true);
            }
            else
            {
                customs[i].SetActive(false);
            }
        }
    }

    public void SwitchCustom()
    {
        if(currentCustom == customs.Length - 1)
        {
            currentCustom = 0;
        }
        else
        {
            currentCustom++;
        }
    }
}
