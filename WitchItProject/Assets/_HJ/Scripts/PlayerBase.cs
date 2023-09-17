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

    // { Delegate�� ���� UnityAction�� 
    protected UnityAction moveFunc = default;
    protected UnityAction jumpFunc = default;
    protected UnityAction leftFunc = default;
    protected UnityAction rigthFunc = default;
    protected UnityAction QFunc = default;
    //  Delegate�� ���� UnityAction�� } 

    // { Hunter Witch ���� ����
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

    //  Hunter Witch ���� ���� }

    protected virtual void Init()
    {
        skillSlot = new SkillSlot(this);
        
        rigid = this.GetComponent<Rigidbody>(); 
        animator = this.GetComponent<Animator>();
    }
    
    protected virtual void InputPlayer()
    {
        // TODO : �� ���� �Է� �޴� ����� �ִٸ� ����

        // TODO : ���� �Է� ����
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
        // TODO : default�� ��� ������ �⺻�� ������ �ֱ�
        //    if(moveFunc == default)
        //    {
        //        this.moveFunc = () => 
        //    }
        this.moveFunc.Invoke(); 
    }

    // TODO : ����, ��ȯ 
    // �̿��̸� ������ �Ѵ� ū ���̰� �����Ƿ�,
    // �������� ������ �Լ��� ¥�� ���⿡�� ó�� �� �� �ֵ���
    protected virtual void Jump()
    {
        // TODO : default�� ��� ������ �⺻�� ������ �ֱ�
        //    if(jumpFunc == default)
        //    {
        //        this.jumpFunc = () => 
        //    }
    }
}
