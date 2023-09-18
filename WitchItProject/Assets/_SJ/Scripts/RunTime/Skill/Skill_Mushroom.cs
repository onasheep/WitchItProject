using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Mushroom : SkillBase
{

    public Skill_Mushroom(string skillType_ = "Mushroom", float moveSpeed_ = 15f,float coolTime_ = 10f)
    {
        SkillType = skillType_;
        MoveSpeed = moveSpeed_;
        CoolTime = coolTime_;
    }

    public override void ActivateSkill
        (GameObject object_, Vector3 dir)
    {

        object_.GetComponent<Rigidbody>().AddForce
            (dir * MoveSpeed, ForceMode.Impulse);
        
    }
}
