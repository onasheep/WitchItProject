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

    // 스킬 정보를 초기화 
    protected void InitInfo(Image skillImg, string skillDesc)
    {
        this.skillImg = skillImg;
        this.skillDesc = skillDesc;
    }       // InitInfo()

    
    // 뭔가를 발사하는 기능
    protected void ShootObject()
    {

    }       // ShootObject()

    
}
