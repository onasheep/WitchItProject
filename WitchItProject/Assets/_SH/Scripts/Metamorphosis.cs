using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metamorphosis : MonoBehaviour
{
    public Transform playerCamera;
    public GameObject lookPoint;

    private GameObject witchBody;
    private GameObject currBody;

    private RaycastHit hit;

    private void Start()
    {
        playerCamera = GameObject.Find("PlayerCamera").transform;
        lookPoint = GameObject.Find("CameraLookPoint");

        witchBody = GameObject.Find("Character_Female_Gypsy");
        currBody = witchBody;
    }

    private void Update()
    {
        Physics.Raycast(lookPoint.transform.position, (lookPoint.transform.position - playerCamera.position).normalized, out hit, Mathf.Infinity, LayerMask.GetMask("ChangeableObjects"));

        ShootRay();
    }

    private void MetamorphosisToObj(GameObject obj_)
    {
        GameObject newBody_ = Instantiate(obj_, transform);

        newBody_.transform.position = lookPoint.transform.position;
        newBody_.AddComponent<Cube>();

        lookPoint.transform.SetParent(newBody_.transform);

        if (currBody == witchBody)
        {
            currBody.SetActive(false);
            currBody.transform.parent.GetComponent<CapsuleCollider>().isTrigger = true;
            currBody.transform.parent.GetComponent<Rigidbody>().useGravity = false;
            currBody = newBody_;
        }
        else
        {
            GameObject prevBody_ = currBody;
            currBody = newBody_;
            Destroy(prevBody_);
        }
    }

    private void ShootRay()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if(hit.collider != null)
            {
                MetamorphosisToObj(hit.collider.gameObject);
            }
        }
    }
}
