using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skill_Cross : SkillBase
{

    public Skill_Cross(string skillType_ = RDefine.CROSS_OBJ, float moveSpeed_ = 15f)
    {
        SkillType = skillType_;
        MoveSpeed = moveSpeed_;
    }

    public override void ActivateSkill
        (GameObject object_)
    {
        object_.GetComponent<Rigidbody>().AddForce
            (Vector3.forward * MoveSpeed, ForceMode.Impulse);
    }
}

