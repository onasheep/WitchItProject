using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Wolf : SkillBase
{
    public string SkillType { get; private set; }
    public float MoveSpeed { get; private set; }

    public Skill_Wolf(string skillType_ = "Wolf", float moveSpeed_ = 3f)
    {
        SkillType = skillType_;
        MoveSpeed = moveSpeed_;
    }
    
    public override void ActivateSkill
        (GameObject object_)
    {
        object_.GetComponent<Rigidbody>().AddForce
            (Vector3.forward * MoveSpeed , ForceMode.Impulse);
    }
}
