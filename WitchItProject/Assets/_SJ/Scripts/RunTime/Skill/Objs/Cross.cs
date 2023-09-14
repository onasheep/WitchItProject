
using System.Collections;
using UnityEngine;

public class Cross : MonoBehaviour
{
    public GameObject blockCollider;

    private float scale = 0f;
    private float time = 0f;
    private float maxRadius = 6f;
    // Start is called before the first frame update
    void Start()
    {

    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // 태그로 하니까 작동 안하던걸 레이어로 하니 작동함
        Physics.IgnoreLayerCollision(9, 9);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Default")
            /*||collision.gameObject.layer == LayerMask.NameToLayer("Ground")*/)
        {
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
            scale += time * 0.2f;
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
            scale -= time * 0.0005f;
            blockCollider.transform.localScale = new Vector3(scale, scale, scale);
            blockCollider.GetComponent<SphereCollider>().radius =
                blockCollider.GetComponent<MeshFilter>().mesh.bounds.extents.x;
            yield return null;
        }       // loop : 범위가 작아지고 그 동안 범위를 마녀는 이동 할 수 없음
        this.gameObject.SetActive(false);
        time = 0f;
    }       // DecreaseCollider()

}
