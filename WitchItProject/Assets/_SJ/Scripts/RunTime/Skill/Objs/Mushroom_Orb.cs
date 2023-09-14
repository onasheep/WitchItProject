using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom_Orb : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // LayerMask ¹«½Ã
        Physics.IgnoreLayerCollision(9, 9);

        if (other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject mushroom = Instantiate(ResourceManager.resources[RDefine.MUSHROOM_OBJ], this.transform.position, Quaternion.identity);              
            }
            Destroy(this.gameObject);
        }        
    }
}
