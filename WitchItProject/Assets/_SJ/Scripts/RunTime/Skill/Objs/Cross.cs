
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
        // �±׷� �ϴϱ� �۵� ���ϴ��� ���̾�� �ϴ� �۵���
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

        }       // loop : ������ Ŀ���� �� ���� ������ ����� �̵� �� �� ����
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
        }       // loop : ������ �۾����� �� ���� ������ ����� �̵� �� �� ����

        GameObject effect = 
            Instantiate(ResourceManager.effects[RDefine.EFFECT_EXPLOSION_YELLOW], this.transform.position + new Vector3(0f, 0.5f,0f), Quaternion.identity);
        Destroy(effect,1f);

        Destroy(this.gameObject);        
    }       // DecreaseCollider()

}
