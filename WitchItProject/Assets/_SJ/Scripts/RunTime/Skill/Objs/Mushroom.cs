using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    private Rigidbody rigid;

    private float existTime = 5f;

    private bool isEnable = default;

    void Start()
    {

        rigid = GetComponent<Rigidbody>();

        isEnable = true;

        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);
        Vector3 RandDir = new Vector3(x, this.transform.position.y + 2f, z);

        rigid.AddForce(RandDir * 3f, ForceMode.Impulse);

        Invoke("DestroyMushroom", existTime);
    }

    private void DestroyMushroom()
    {
        GameObject effect = Instantiate(ResourceManager.effects[RDefine.EFFECT_NOVA_YELLOW], transform.position, Quaternion.LookRotation(transform.up));
        Destroy(effect, 1f);
        Destroy(this.gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        // LayerMask ¹«½Ã
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("Projectile"));

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")
           || collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if(isEnable)
            {
                isEnable = false;


                this.transform.up = collision.GetContact(0).normal;

                this.transform.localScale *= 1.3f;
                Instantiate(ResourceManager.effects[RDefine.EFFECT_EXPLOSION_GREEN], this.transform);

                rigid.constraints = RigidbodyConstraints.FreezeAll;
                rigid.velocity = Vector3.zero;
                rigid.useGravity = false;
            }
           

        }
    }
}
 

