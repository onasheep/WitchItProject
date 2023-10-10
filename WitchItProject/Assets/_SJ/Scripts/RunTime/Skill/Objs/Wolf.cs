using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    private Rigidbody rigid = default;
    private Animator animator = default;

    private Ray ray;
    private Transform rayStart = default;
    private float rayDist = 1.5f;
    private float existTime = 10f;

    private bool isFind = default;
    private bool isRun = default;

    //  �ڱ⿡�� �پ� �����Ƿ� ���ε��� ����������? 
    public GameObject effect_Question;
    public GameObject effect_Skull;

    private Vector3 smokeEffectOffset = default;

    // Start is called before the first frame update
    void Start()
    {
        Init();

        Invoke("PlayDieAnim", existTime);
    }

    private void Init()
    {
        rigid = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();
        rayStart = this.gameObject.FindChildObj("CG").GetComponent<Transform>();

        float width = this.GetComponentInChildren<BoxCollider>().size.z;
        float height = this.GetComponentInChildren<BoxCollider>().size.y;

        smokeEffectOffset = new Vector3(0f, height / 2,width / 2);

        effect_Question.SetActive(true);

        isRun = true;
        isFind = false;
        ray = new Ray(rayStart.position, rayStart.forward);
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
        
        GameObject effect = Instantiate(ResourceManager.effects[RDefine.EFFECT_SMOKE], this.transform.position+ smokeEffectOffset, Quaternion.identity);
        Destroy(effect, 2f);
        Destroy(this.gameObject);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Witch"))
        {
            effect_Skull.SetActive(true);
            effect_Question.SetActive(false);

            isFind = true;
            animator.SetBool("isFind", isFind);
        }       // if : ���డ ���� �ȿ� �ִ� ���
        else { /* Do Nothing */ }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Witch"))
        {
            effect_Question.SetActive(true);
            effect_Skull.SetActive(false);

            isFind = false;
            animator.SetBool("isFind", isFind) ;
        }       // if : ���డ ���� �ۿ� �ִ� ���
        else { /* Do Nothing */ }
    }
}
