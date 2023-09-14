using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    private Rigidbody rigid;

    private float existTime = 5f;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);
        Vector3 RandDir = new Vector3(x, this.transform.position.y + 1f, z);
        
        rigid.AddForce(RandDir * 4f, ForceMode.Impulse);

        Invoke("DestroyMushroom", existTime);
    }


    private void DestroyMushroom()
    {
        Destroy(this.gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        // LayerMask ����
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("Projectile"));
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")
           || collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {

            // { �ݶ��̴� �ߺ� �浹 ���� ó��
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;            
            //  �ݶ��̴� �ߺ� �浹 ���� ó�� }

            this.transform.up = collision.GetContact(0).normal;
            this.transform.localScale *= 2f;
            Instantiate(ResourceManager.effects[RDefine.EFFECT_EXPLOSION_GREEN], this.transform);
            rigid.constraints = RigidbodyConstraints.FreezeAll;
            rigid.velocity = Vector3.zero;
            rigid.useGravity = false;

        }
    }
}
 

