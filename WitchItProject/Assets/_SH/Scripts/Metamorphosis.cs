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

        witchBody = GameObject.Find("Character_Female_Witch");
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
                transform.position = lookPoint.transform.position;
                //witchBody.transform.position = lookPoint.transform.position;

                witchBody.SetActive(true);

                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Collider>().enabled = true;

                GameObject prevBody_ = lookPoint.transform.parent.gameObject;

                lookPoint.transform.SetParent(transform);
                lookPoint.transform.localPosition = new Vector3(0, 1.379f, 0);

                Destroy(prevBody_);

                currBody = witchBody;
            }
        }

        GetComponent<WitchController>().isMetamor = false;
    }

    private void PossesionToObj(GameObject obj_)
    {
        obj_.AddComponent<Cube>();

        if (currBody == witchBody)
        {
            currBody.SetActive(false);

            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Collider>().enabled = false;

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

        GetComponent<WitchController>().isMetamor = true;
    }

    private void MetamorphosisToObj(GameObject obj_)
    {
        GameObject newBody_ = Instantiate(obj_);

        newBody_.transform.position = lookPoint.transform.position;
        newBody_.AddComponent<Cube>();

        if (currBody == witchBody)
        {
            currBody.SetActive(false);

            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Collider>().enabled = false;

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

        GetComponent<WitchController>().isMetamor = true;
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
