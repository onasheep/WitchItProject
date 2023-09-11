using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    private Image skillImg = default;
    private string skillDesc = default;

    private List<SkillAction> slots = default;


    // Enum Type 
    // Witch Hunter 

    // Enum Type
    // Skill 1, Skill 2, Skill 3

    // Start is called before the first frame update
    void Start()
    {
        slots = new List<SkillAction>();        
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

    
    

    
}
