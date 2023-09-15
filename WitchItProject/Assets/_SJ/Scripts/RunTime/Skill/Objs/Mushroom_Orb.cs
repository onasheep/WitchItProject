using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom_Orb : MonoBehaviour
{
    private float existTime = 2f;
    private void OnTriggerEnter(Collider other)
    {
        // LayerMask ����
        Physics.IgnoreLayerCollision(9, 9);

        GameObject effectOrb = Instantiate(ResourceManager.effects[RDefine.EFFECT_ORB],this.transform.position, Quaternion.identity);  
        Destroy(effectOrb, existTime);

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")
            || other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject mushroom = Instantiate(ResourceManager.objs[RDefine.MUSHROOM_OBJ], this.transform.position, Quaternion.identity);              
            }
            Destroy(this.gameObject);
        }        
    }
}