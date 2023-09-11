using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAction : MonoBehaviour
{
    public List<SkillAction> actions;

    private void Start()
    {
        actions = new List<SkillAction>();        
    }
    //public class Skill_Wolf : SkillAction, ISkillModule
    //{
    //    string Type = "Wolf";
    //    string SkillImg = default;
    //    public void ActiveSkill(GameObject wolf_, float moveSpeed)
    //    {
    //        Instantiate(wolf_);
    //    }
    //}
   
    //public class Skill_Cross : SkillAction, ISkillModule
    //{
    //    string Type = "Cross";
    //    string SkillImg = default;  
    //    public void ActiveSkill(GameObject cross_, float moveSpeed)
    //    {
    //        Instantiate(cross_);
    //    }
    //}

    //public class Skill_Beartrap : SkillAction, ISkillModule
    //{
    //    string Type = "Beartrap";
    //    string skillImg = default;
    //    public void ActiveSkill(GameObject beartrap_, float moveSpeed)
    //    {
    //        Instantiate(beartrap_); 
    //    }
    //}
}
