using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// SJ_ 
public abstract class PlayerBase : MonoBehaviour
{

    protected float hp = default;
    protected float maxHp = default;

    protected float damage = default;

    // { Delegate로 사용될 UnityAction들 
    protected UnityAction moveFunc = default;
    protected UnityAction jumpFunc = default;
    protected UnityAction leftFunc = default;
    protected UnityAction rigthFunc = default;
    protected UnityAction QFunc = default;
    //  Delegate로 사용될 UnityAction들 } 

    // { Hunter Witch 공통 사항
    protected Transform myCamera = default;
    protected GameObject crossHair = default;
    protected Rigidbody rigid = default;
    protected Animator animator = default;
    protected SkillSlot skillSlot = default;

    protected float moveSpeed = default;
    protected float jumpForce = default;

    protected enum TYPE
    {
        NONE = -1, WITCH, HUNTER
    }

    protected TYPE type = TYPE.NONE;

    //  Hunter Witch 공통 사항 }

    protected virtual void Init()
    {
        skillSlot = new SkillSlot(this);
        
        rigid = this.GetComponent<Rigidbody>(); 
        animator = this.GetComponent<Animator>();
    }
    
    protected virtual void InputPlayer()
    {
        // TODO : 더 좋은 입력 받는 방식이 있다면 변경

        // TODO : 추후 입력 방지
        //if( isDead == true) {  return; }
        
        if(Input.GetButtonDown("Fire1"))
        {
            if(leftFunc == null)
            {
                return;
            }
            this.leftFunc.Invoke();            
        }
        if(Input.GetButtonDown("Fire2"))
        {
            this.rigthFunc.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            this.QFunc.Invoke();
        }        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            this.jumpFunc.Invoke();
        }

    }

    protected virtual void Move()
    {
        // TODO : default인 경우 방어로직 기본적 움직임 넣기
        //    if(moveFunc == default)
        //    {
        //        this.moveFunc = () => 
        //    }
        this.moveFunc.Invoke(); 
    }

    // TODO : 형준, 석환 
    // 이왕이면 점프는 둘다 큰 차이가 없으므로,
    // 공통적인 형태의 함수로 짜서 여기에서 처리 할 수 있도록
    protected virtual void Jump()
    {
        // TODO : default인 경우 방어로직 기본적 움직임 넣기
        //    if(jumpFunc == default)
        //    {
        //        this.jumpFunc = () => 
        //    }
    }
}
