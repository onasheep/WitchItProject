using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom_Orb : MonoBehaviour
{
    private float existTime = 2f;

    private bool isEnable = true;

    private void OnTriggerEnter(Collider other)
    {
        // LayerMask ¹«½Ã
        Physics.IgnoreLayerCollision(9, 9);
        Physics.IgnoreLayerCollision(0, 0);
        Physics.IgnoreLayerCollision(7, 7);

        GameObject effectOrb = Instantiate(ResourceManager.effects[RDefine.EFFECT_ORB],this.transform.position, Quaternion.identity);  
        Destroy(effectOrb, existTime);

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")
            || other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if(isEnable)
            {
                isEnable = false;

                for (int i = 0; i < 4; i++)
                {
                    GameObject mushroom = Instantiate(ResourceManager.objs[RDefine.MUSHROOM_OBJ], this.transform.position, Quaternion.identity);
                    float x = Random.Range(-1f, 1f);
                    float z = Random.Range(-1f, 1f);
                    Vector3 RandDir = (this.transform.position + new Vector3(x,  + 1f, z)).normalized;

                    mushroom.GetComponent<Rigidbody>().AddForce(RandDir * 3f, ForceMode.Impulse);

                }
                Destroy(this.gameObject);
            }
            
        }        
    }
}
