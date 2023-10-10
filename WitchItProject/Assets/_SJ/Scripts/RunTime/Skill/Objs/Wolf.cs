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

    //  자기에게 붙어 있으므로 바인딩이 유리할지도? 
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

    // TEST : 임시 프리팹 파괴 함수 
    // 추후 설계에 따라 오브젝트 풀 또는 깔끔한 방법으로 파괴
    // AnimEvent로 isEnd Smell 애니메이션 실행 이후 실행
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
        }       // if : 마녀가 범위 안에 있는 경우
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
        }       // if : 마녀가 범위 밖에 있는 경우
        else { /* Do Nothing */ }
    }
}
