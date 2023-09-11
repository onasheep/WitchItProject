using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillTestControl : MonoBehaviour
{
    public GameObject chicken;
    public GameObject galic;
    public Transform barrel;

    private ISkillModule activeSkillModule;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // { ���� �� ������ �߻��ϴ� ��� 
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //GameObject projectile = Instantiate(chicken, transform.position, Quaternion.identity);
            GameObject projectile = Instantiate(galic, barrel.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().AddForce(transform.forward * 15f, ForceMode.Impulse);
        }
        //  ���� �� ������ �߻��ϴ� ��� }
    }
}


