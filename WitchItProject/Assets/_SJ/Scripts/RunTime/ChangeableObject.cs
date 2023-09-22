using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangeableObject : MonoBehaviour
{
    public bool OnRay = default;
    private Outlinable outline;
    // Start is called before the first frame update
    void Start()
    {
        outline =this.GetComponent<Outlinable>();
    }

    // Update is called once per frame
    void Update()
    {
        if(OnRay || OnRay == default)
        {
            OnRay = false;
            outline.enabled = OnRay;

        }

    }

    public void SetOutline()
    {
        OnRay = true;
        outline.enabled = OnRay;
    }


}
