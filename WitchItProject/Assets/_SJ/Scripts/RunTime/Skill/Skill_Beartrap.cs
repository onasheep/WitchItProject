using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Beartrap : SkillBase
{
    public string SkillType { get; private set; }
    public float MoveSpeed { get; private set; }

    public Skill_Beartrap(string skillType_ = "Beartrap", float moveSpeed_ = 5f)
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
