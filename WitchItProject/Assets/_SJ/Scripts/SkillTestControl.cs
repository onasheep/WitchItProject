using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillTestControl : MonoBehaviour
{
    public GameObject cube;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // { 뭔가 닭 같은거 발사하는 기능 
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject projectile = Instantiate(cube, transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.Impulse);
        }
        //  뭔가 닭 같은거 발사하는 기능 }
    }
}


