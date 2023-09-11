using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public GameObject followTarget;

    private Rigidbody rigid;

    private void Start()
    {
        followTarget = FindObjectOfType<WitchController>().gameObject;
        rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MoveByTorque();
    }

    void MoveByTorque()
    {
        Vector3 direction = (followTarget.transform.position - transform.position).normalized;

        Vector3 torqueAxis = new Vector3(direction.z, direction.y, -direction.x);

        rigid.AddTorque(torqueAxis * 10);
    }
}
