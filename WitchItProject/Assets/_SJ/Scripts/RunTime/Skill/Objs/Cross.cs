
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
        // �±׷� �ϴϱ� �۵� ���ϴ��� ���̾�� �ϴ� �۵���
        Physics.IgnoreLayerCollision(9, 9);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")
            ||collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // { �ݶ��̴� �ߺ� �浹 ���� ó��
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            //  �ݶ��̴� �ߺ� �浹 ���� ó�� }

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

        }       // loop : ������ Ŀ���� �� ���� ������ ����� �̵� �� �� ����
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
        }       // loop : ������ �۾����� �� ���� ������ ����� �̵� �� �� ����
        this.gameObject.SetActive(false);
        time = 0f;
    }       // DecreaseCollider()

}
