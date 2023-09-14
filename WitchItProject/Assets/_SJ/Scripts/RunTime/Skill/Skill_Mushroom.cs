using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Mushroom : SkillBase
{
    public string SkillType { get; private set; }
    public float MoveSpeed { get; private set; }

    public Skill_Mushroom(string skillType_ = "Mushroom", float moveSpeed_ = 15f)
    {
        SkillType = skillType_;
        MoveSpeed = moveSpeed_;
    }

    public override void ActivateSkill
        (GameObject object_)
    {
        object_.GetComponent<Rigidbody>().AddForce
            (Vector3.forward * MoveSpeed, ForceMode.Impulse);
        Debug.LogFormat("{0}", MoveSpeed);

    }
}
