using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
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
                    GameObject mushroom = PhotonNetwork.Instantiate(RDefine.MUSHROOM_OBJ, this.transform.position, Quaternion.identity);              
                }
                PhotonNetwork.Destroy(this.gameObject);
            }
            
        }        
    }
}
