using UnityEngine;

public class Chicken : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag.Equals("Witch"))
        {
            Debug.LogFormat("¸¶³à ¹ß°ß");
        }
    }
}
