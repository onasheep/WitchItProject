using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Wolf : SkillBase
{

    public Skill_Wolf(string skillType_ = RDefine.WOLF_OBJ, float moveSpeed_ = 10f)
    {
        SkillType = skillType_;
        MoveSpeed = moveSpeed_;
    }    
    public override void ActivateSkill(GameObject object_)
    {        
        object_.GetComponent<Rigidbody>().AddForce
            (Vector3.forward * MoveSpeed , ForceMode.Impulse);
    }    
}
