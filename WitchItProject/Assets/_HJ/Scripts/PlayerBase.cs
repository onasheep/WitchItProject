using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// SJ_ 
public abstract class PlayerBase : MonoBehaviourPun
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

    protected const float MOVESPEED = 5f;
    protected const float JUMPFORCE = 5f;

    // 09/18 Jung
    protected float verticalMove = default;
    protected float horizontalMove = default;
    // 09/18 Jung

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
        //HJ 230920__
        //if (!photonView.IsMine)
        //{
        //    return;
        //}
        // TODO : �� ���� �Է� �޴� ����� �ִٸ� ����

        // TODO : ���� �Է� ����
        //if( isDead == true) {  return; }

        // 09/18 Jung
        verticalMove = Input.GetAxisRaw("Vertical");
        horizontalMove = Input.GetAxisRaw("Horizontal");
        // 09/18 Jung

        if (Input.GetButtonDown("Fire1"))
        {
            if (leftFunc == null)
            {
                return;
            }
            this.leftFunc.Invoke();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            this.rigthFunc.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.QFunc.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Space))
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

        // 9/18 Jung
        rigid.AddForce(transform.forward * verticalMove * 50, ForceMode.Force);
        rigid.AddForce(transform.right * horizontalMove * 50, ForceMode.Force);

        animator.SetFloat("InputVertical", verticalMove);
        animator.SetFloat("InputHorizontal", horizontalMove);
        // 9/18 Jung
    }

    // TODO : ����, ��ȯ 
    // �̿��̸� ������ �Ѵ� ū ���̰� �����Ƿ�,
    // �������� ������ �Լ��� ¥�� ���⿡�� ó�� �� �� �ֵ���
    protected virtual void Jump()
    {
        // TODO : default�� ��� ������ �⺻�� ������ �ֱ�
        //if (jumpFunc == default)
        //{
        //    this.jumpFunc = () =>
        //    {
        //rigid.AddForce(transform.up * JUMPFORCE, ForceMode.Impulse);

        //    };
        //}
    }

    protected IEnumerator Footfall()
    {
        while (true)
        {
            Effect footfall_ = ObjPool.GetEffect(ObjPool.EffectNames.Footfall);
            footfall_.transform.position = this.transform.position;

            yield return new WaitForSeconds(1.0f);
        }
    }
}
