using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


// SJ_ 
public abstract class PlayerBase : MonoBehaviourPun
{

    #region Variables
    protected float hp = default;
    protected float maxHp = default;

    protected float damage = default;

    // { Delegate UnityAction 
    protected UnityAction moveFunc = default;
    protected UnityAction jumpFunc = default;
    protected UnityAction leftFunc = default;
    protected UnityAction rigthFunc = default;
    protected UnityAction QFunc = default;
    // } Delegate UnityAction 

    // { Hunter Witch Common
    protected Transform myCamera = default;
    protected GameObject crossHair = default;
    protected Rigidbody rigid = default;
    protected Animator animator = default;
    protected SkillSlot skillSlot = default;

    protected const float MOVESPEED = 5f;
    protected const float JUMPFORCE = 5f;

    // { Skill Q, RightMouse bool
    [SerializeField]
    protected bool isSkillQ_On = default;
    [SerializeField]
    protected bool isSkillRM_On = default;
    // } Skill Q, RightMouse bool


    // } Hunter Witch Common

    // 09/18 Jung
    protected float verticalMove = default;
    protected float horizontalMove = default;

    protected bool canSpawnFootfall = true;
    // 09/18 Jung

    protected enum TYPE
    {
        NONE = -1, WITCH, HUNTER
    }

    protected TYPE type = TYPE.NONE;

    //  Hunter Witch ���� ���� }
    #endregion

    protected virtual void Init()
    {
        skillSlot = new SkillSlot(this);

        rigid = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();


        // { Skill bool Init
        isSkillQ_On = true;
        isSkillRM_On = true;
        // } Skill bool Init
    }

    protected virtual void InputPlayer()
    {
     
        verticalMove = Input.GetAxisRaw("Vertical");
        horizontalMove = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Fire1"))
        {
            if (leftFunc == null)
            {
                return;
            }
            this.leftFunc.Invoke();
        }
        // SJ_230926 isSkillOn add 
        if (Input.GetButtonDown("Fire2") && isSkillRM_On)
        {
            isSkillRM_On = false;

            this.rigthFunc.Invoke();

            ThreadManager.instance.DoRoutine(() => OnSkill(ref isSkillRM_On), skillSlot.Slots[0].CoolTime);

        }
        if (Input.GetKeyDown(KeyCode.Q) && isSkillQ_On)
        {
            isSkillQ_On = false;

            this.QFunc.Invoke();

            ThreadManager.instance.DoRoutine(() => OnSkill(ref isSkillQ_On), skillSlot.Slots[1].CoolTime);

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.jumpFunc.Invoke();
        }
        
    }

    protected virtual void Move()
    {
        // TODO : 통일성 위해서 일단 수정 가능하다면 하고
        // 시간이 없어서 아니면 날리자!
        //    if(moveFunc == default)
        //    {
        //        this.moveFunc = () => 

        //    }
        this.moveFunc.Invoke();

        rigid.AddForce(transform.forward * verticalMove * 50, ForceMode.Force);
        rigid.AddForce(transform.right * horizontalMove * 50, ForceMode.Force);

        animator.SetFloat("InputVertical", verticalMove);
        animator.SetFloat("InputHorizontal", horizontalMove);
    }

    protected virtual void Jump()
    {
        // TODO : 통일성 위해서 일단 수정 가능하다면 하고
        // 시간이 없어서 아니면 날리자!
        //if (jumpFunc == default)
        //{
        //    this.jumpFunc = () =>
        //    {
        //rigid.AddForce(transform.up * JUMPFORCE, ForceMode.Impulse);

        //    };
        //}
    }

    // SJ_230925
    protected void OnSkill(ref bool isSkillOn_)
    {
        isSkillOn_ = true;
    }

    protected IEnumerator Footfall()
    {
        yield return new WaitForSeconds(0.25f);

        canSpawnFootfall = true;
    }
}
