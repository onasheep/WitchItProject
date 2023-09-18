using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Wolf : SkillBase
{

    public Skill_Wolf(string skillType_ = RDefine.WOLF_OBJ, float moveSpeed_ = 10f,float coolTime_ = 10f)
    {
        SkillType = skillType_;
        MoveSpeed = moveSpeed_;
        CoolTime = coolTime_;
    }    
    public override void ActivateSkill
        (GameObject object_, Vector3 dir)
    {        

        object_.GetComponent<Rigidbody>().AddForce
            (dir * MoveSpeed , ForceMode.Impulse);
    }    
}
