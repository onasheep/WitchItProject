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
        playerCamera = GameObject.Find("WitchCamera").transform;
        lookPoint = GameObject.Find("CameraLookPoint");

        witchBody = GameObject.Find("WitchCharacter");
        currBody = witchBody;
    }

    private void Update()
    {
        Physics.Raycast(lookPoint.transform.position, (lookPoint.transform.position - playerCamera.position).normalized, out hit, Mathf.Infinity, LayerMask.GetMask("ChangeableObjects"));

        ShootRay();
        CancelMetamorphosis();
    }

    private void CancelMetamorphosis()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (witchBody.activeInHierarchy)
            {
                return;
            }
            else if (!witchBody.activeInHierarchy)
            {
                witchBody.transform.position = lookPoint.transform.position;

                witchBody.SetActive(true);

                GameObject prevBody_ = lookPoint.transform.parent.gameObject;

                lookPoint.transform.SetParent(witchBody.transform);
                lookPoint.transform.localPosition = new Vector3(0, 1.379f, 0);

                Destroy(prevBody_);

                currBody = witchBody;
            }
        }
    }

    private void PossesionToObj(GameObject obj_)
    {
        obj_.AddComponent<Cube>();

        if (currBody == witchBody)
        {
            currBody.SetActive(false);

            lookPoint.transform.SetParent(obj_.transform);
            lookPoint.transform.position = obj_.GetComponent<Renderer>().bounds.center;

            currBody = obj_;
        }
        else
        {
            GameObject prevBody_ = lookPoint.transform.parent.gameObject;

            lookPoint.transform.SetParent(obj_.transform);
            lookPoint.transform.position = obj_.GetComponent<Renderer>().bounds.center;

            Destroy(prevBody_);

            currBody = obj_;
        }
    }

    private void MetamorphosisToObj(GameObject obj_)
    {
        GameObject newBody_ = Instantiate(obj_, transform);

        newBody_.transform.position = lookPoint.transform.position;
        newBody_.AddComponent<Cube>();

        if (currBody == witchBody)
        {
            currBody.SetActive(false);

            lookPoint.transform.SetParent(newBody_.transform);
            lookPoint.transform.position = newBody_.GetComponent<Renderer>().bounds.center;

            currBody = newBody_;
        }
        else
        {
            GameObject prevBody_ = lookPoint.transform.parent.gameObject;

            lookPoint.transform.SetParent(newBody_.transform);
            lookPoint.transform.position = newBody_.GetComponent<Renderer>().bounds.center;

            Destroy(prevBody_);

            currBody = newBody_;
        }
    }

    private void ShootRay()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (hit.collider != null)
            {
                MetamorphosisToObj(hit.collider.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hit.collider != null)
            {
                PossesionToObj(hit.collider.gameObject);
            }
        }
    }
}
