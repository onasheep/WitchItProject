
using System.Collections;
using UnityEngine;

public class Cross : MonoBehaviour
{
    public GameObject blockCollider;
    public GameObject particleObject;

    private float scale = 0f;
    private float time = 0f;
    private float maxRadius = 8f;
    // Start is called before the first frame update
    void Start()
    {

    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // 태그로 하니까 작동 안하던걸 레이어로 하니 작동함
        Physics.IgnoreLayerCollision(9, 9);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")
            ||collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // { 콜라이더 중복 충돌 예외 처리
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            //  콜라이더 중복 충돌 예외 처리 }

            this.gameObject.GetComponent<Rigidbody>().constraints
            = RigidbodyConstraints.FreezeAll;
            StartCoroutine(IncreaseCollider());
        }
        
    }
    private IEnumerator IncreaseCollider()
    {
        blockCollider.transform.localScale = Vector3.zero;
        while(scale < maxRadius)
        {
            time += Time.deltaTime;
            scale += time * 0.1f;

            particleObject.transform.localScale = new Vector3(scale, scale, scale);
            
            blockCollider.transform.localScale = new Vector3(scale, scale, scale);
            blockCollider.GetComponent<SphereCollider>().radius =
                blockCollider.GetComponent<MeshFilter>().mesh.bounds.extents.x;
            yield return null;

        }       // loop : 범위가 커지고 그 동안 범위를 마녀는 이동 할 수 없음
        time = 0f;
        StartCoroutine(DecreaseCollider());
    }       // IncreaseCollider()

    private IEnumerator DecreaseCollider()
    {
        while (0 < scale)
        {
            time += Time.deltaTime;
            scale -= time * 0.00005f;

            particleObject.transform.localScale = new Vector3(scale, scale, scale);

            blockCollider.transform.localScale = new Vector3(scale, scale, scale);
            blockCollider.GetComponent<SphereCollider>().radius =
                blockCollider.GetComponent<MeshFilter>().mesh.bounds.extents.x;
            yield return null;
        }       // loop : 범위가 작아지고 그 동안 범위를 마녀는 이동 할 수 없음
        this.gameObject.SetActive(false);
        time = 0f;
    }       // DecreaseCollider()

}
