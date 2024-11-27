using System.Collections.Generic;
using UnityEngine;


public abstract class SkillBase
{
    public PlayerBase PlayerController { get; protected set; }
    public string SkillType {  get; protected set; }
    public float MoveSpeed { get; protected set; }

    public float CoolTime { get; protected set; }

    public virtual void Init(PlayerBase player_)
    {
        PlayerController = player_;
    }
    public virtual void ActivateSkill(GameObject object_, Vector3 dir)
    {

    }

    public virtual void ActivateSkill(GameObject object_)
    {

    }
 
}

