using System.Collections.Generic;
using UnityEngine;


public class SkillBase
{
    public PlayerBase playerController = default;

    public virtual void Init(PlayerBase player_)
    {
        playerController = player_;
    }
    public virtual void ActivateSkill(GameObject object_)
    {
        /* Skill_"��ų"�� ����� */
    }

}

