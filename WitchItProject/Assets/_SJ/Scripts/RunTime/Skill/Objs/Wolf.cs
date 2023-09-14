using UnityEngine;

public class Wolf : MonoBehaviour
{
    private Rigidbody rigid;
    private Animator animator;

    private Ray ray;
    private Transform rayStart;
    private float rayDist = 1.5f;
    private float existTime = 10f;

    private bool isFind = default;
    private bool isRun = default;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rigid = this.GetComponent<Rigidbody>(); 
        animator = this.GetComponent<Animator>();
        rayStart = this.gameObject.FindChildObj("CG").GetComponent<Transform>();

        isRun = true;
        isFind = false;
        ray = new Ray(rayStart.position, rayStart.forward);

        Invoke("PlayDieAnim", existTime);
    }

    void Update()
    {
        CheckRay();
    }

    private void CheckRay()
    {
        if (Physics.Raycast(ray, rayDist, LayerMask.NameToLayer("Default")))
        {
            rigid.velocity = Vector3.zero;
            animator.SetFloat("velocity", rigid.velocity.magnitude);
            isRun = false;
        }
        else
        {
            animator.SetFloat("velocity",rigid.velocity.magnitude);
        }
    }
    private void PlayDieAnim()
    {     
        animator.SetTrigger("isEnd");        
    }

    // TEST : �ӽ� ������ �ı� �Լ� 
    // ���� ���迡 ���� ������Ʈ Ǯ �Ǵ� ����� ������� �ı�
    // AnimEvent�� isEnd Smell �ִϸ��̼� ���� ���� ����
    private void DestroyWolf()
    {
        Destroy(this.gameObject);
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag.Equals("Witch"))
        {
            isFind = true;
            animator.SetBool("isFind", isFind);
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag.Equals("Witch"))
        {
            isFind = false;
            animator.SetBool("isFind", isFind) ;
        }
    }
}
