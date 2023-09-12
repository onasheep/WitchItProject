using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : MonoBehaviour
{
    protected float hp = default;
    protected float maxHp = default;

    protected float damage = default;
    protected float moveSpeed = default;
    protected float jumpForce = default;

    protected SkillSlot skillSlot = default;

    protected enum TYPE
    {
        NONE = -1, WITCH, HUNTER
    }

    protected virtual void Init()
    {
        skillSlot = new SkillSlot(this);
    }

    protected virtual void InputPlayer()
    {

    }

    protected virtual void Move()
    {

    }
    
    protected virtual void Rotate()
    {

    }

    protected virtual void LeftClick()
    {

    }

    protected virtual void RightClick()
    {

    }

    protected virtual void QPress()
    {

    }
}
