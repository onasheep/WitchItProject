using UnityEngine;
using UnityEngine.UI;

public abstract class SkillBase : MonoBehaviour
{
    private Image skillImg = default;
    private string skillDesc = default;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ��ų ������ �ʱ�ȭ 
    protected void InitInfo(Image skillImg, string skillDesc)
    {
        this.skillImg = skillImg;
        this.skillDesc = skillDesc;
    }       // InitInfo()

    
    // ������ �߻��ϴ� ���
    protected void ShootObject()
    {

    }       // ShootObject()

    
}
