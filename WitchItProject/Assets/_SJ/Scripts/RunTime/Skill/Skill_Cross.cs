using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skill_Cross : SkillBase
{
    public string SkillType { get; private set; }
    public float MoveSpeed { get; private set; }

    public Skill_Cross(string skillType_ = "Cross", float moveSpeed_ = 15f)
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

