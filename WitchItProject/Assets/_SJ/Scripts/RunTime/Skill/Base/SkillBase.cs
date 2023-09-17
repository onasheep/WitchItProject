using System.Collections.Generic;
using UnityEngine;


public class SkillBase
{
    private PlayerBase playerController = default;
    public string SkillType {  get; protected set; }
    public float MoveSpeed { get; protected set; }

    public virtual void Init(PlayerBase player_)
    {
        playerController = player_;
    }
    public virtual void ActivateSkill(GameObject object_)
    {
        /* Skill_"스킬"이 실행됨 */
    }

}

