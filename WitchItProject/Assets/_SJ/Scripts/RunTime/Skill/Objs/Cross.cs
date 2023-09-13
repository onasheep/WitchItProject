
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
        // �±׷� �ϴϱ� �۵� ���ϴ��� ���̾�� �ϴ� �۵���
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

        }       // loop : ������ Ŀ���� �� ���� ������ ����� �̵� �� �� ����
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
        }       // loop : ������ �۾����� �� ���� ������ ����� �̵� �� �� ����
        this.gameObject.SetActive(false);
        time = 0f;
    }       // DecreaseCollider()

}