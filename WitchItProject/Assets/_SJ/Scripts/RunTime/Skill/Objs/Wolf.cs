using System.Collections.Generic;
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

    //  �ڱ⿡�� �پ� �����Ƿ� ���ε��� ����������? 
    public GameObject effect_Question;
    public GameObject effect_Skull;
    

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
        Destroy(this.gameObject);
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag.Equals("Witch"))
        {
            effect_Skull.SetActive(true);
            effect_Question.SetActive(false);

            isFind = true;
            animator.SetBool("isFind", isFind);
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag.Equals("Witch"))
        {
            effect_Question.SetActive(true);
            effect_Skull.SetActive(false);
            isFind = false;
            animator.SetBool("isFind", isFind) ;
        }
    }
}
