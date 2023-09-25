
using System.Collections;
using UnityEngine;

public class Cross : MonoBehaviour
{
    public GameObject blockCollider;
    public GameObject particleObject;

    private float scale = 0f;
    private float time = 0f;
    private float maxRadius = 8f;

    private bool isEnable = true;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        // 태그로 하니까 작동 안하던걸 레이어로 하니 작동함
        Physics.IgnoreLayerCollision(9, 9);
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")
            || other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (isEnable)
            {
                isEnable = false;
                this.gameObject.GetComponent<Rigidbody>().constraints
                 = RigidbodyConstraints.FreezeAll;

                this.transform.up = other.gameObject.transform.up;

                StartCoroutine(IncreaseCollider());
            }

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
            scale -= time * 0.005f;

            Vector3 scaleVector = new Vector3(scale,scale,scale);

            particleObject.transform.localScale = scaleVector;
            blockCollider.transform.localScale = scaleVector;
            
            blockCollider.GetComponent<SphereCollider>().radius =
                blockCollider.GetComponent<MeshFilter>().mesh.bounds.extents.x;
            yield return null;
        }       // loop : 범위가 작아지고 그 동안 범위를 마녀는 이동 할 수 없음

        GameObject effect = 
            Instantiate(ResourceManager.effects[RDefine.EFFECT_EXPLOSION_YELLOW], this.transform.position + new Vector3(0f, 0.5f,0f), Quaternion.identity);
        Destroy(effect,1f);

        Destroy(this.gameObject);        
    }       // DecreaseCollider()

}
