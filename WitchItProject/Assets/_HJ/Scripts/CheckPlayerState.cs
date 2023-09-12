using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerState : MonoBehaviour
{
    [SerializeField] private WitchController witch;
    [SerializeField] private GameObject deathWitch;
    [SerializeField] private MeshRenderer witchRenderer;


    private bool testBool = false;

    // Start is called before the first frame update
    void Start()
    {
        witch = transform.GetChild(0).gameObject.GetComponent<WitchController>();
        deathWitch = GameObject.Find("DeathCamTestObj").gameObject;
        witchRenderer = deathWitch.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (witch.isDead && testBool == false)
        {
            witchRenderer.enabled = true;
            testBool = true;
        }
    }
}
