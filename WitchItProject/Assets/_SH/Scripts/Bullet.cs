using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rigid;

    private Vector3 fireDirection;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Invoke("DestroyThis", 3f);
    }
    private void FixedUpdate()
    {
        fireDirection = transform.forward;
        //fireDirection = transform.up;
        ShootThis();
    }
    private void ShootThis()
    {
        rigid.AddForce(fireDirection * 50, ForceMode.Force);
    }

    public void DestroyThis()
    {

        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        BulletPool.ReturnObject(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<WitchController>() != null)
        {
            collision.gameObject.GetComponent<WitchController>().TakeDamage();
        }

        Effect effect_ = BulletPool.GetEffect(BulletPool.GetPrefab(0));
        effect_.transform.position = collision.GetContact(0).point;
        effect_.transform.forward = collision.GetContact(0).normal;

        DestroyThis();
    }
}
