using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rigid;
    public GameObject flash;

    private Vector3 fireDirection;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        if (flash != null)
        {
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
    }

    private void OnEnable()
    {
        Invoke("DestroyThis", 3f);
    }
    private void FixedUpdate()
    {
        //fireDirection = transform.forward;
        fireDirection = transform.up;
        ShootThis();
    }
    private void ShootThis()
    {
        rigid.AddForce(fireDirection * -50, ForceMode.Force);
    }

    public void DestroyThis()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        BulletPool.ReturnObject(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Witch"))
        {
            collision.gameObject.GetComponent<WitchController>().TakeDamage();
        }

        DestroyThis();
    }
}
