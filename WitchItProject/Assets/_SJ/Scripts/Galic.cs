
using System.Collections;
using UnityEngine;

public class Galic : MonoBehaviour
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
        if(collision.gameObject.layer != LayerMask.NameToLayer("Witch"))
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

        }       // loop : 범위가 커지고 일정 시간 후 작아지며 그 동안 범위를 마녀는 이동 할 수 없음
        time = 0f;
        StartCoroutine(DecreaseCollider());
    }

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
        }
        this.gameObject.SetActive(false);
        time = 0f;
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (!other.gameObject.tag.Equals("Witch")
    //     || !other.gameObject.tag.Equals("Hunter"))
    //    {
    //        Debug.Log("뭔가 점점 커지는 공간");
    //    }
    //}
}
