using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot
{
    private int slotCount = -1;
    public SkillBase[] Slots { get; private set; }
    public SkillManager skillManager { get; private set; }


    // Enum Type 
    // Witch Hunter 

    // Enum Type
    // Skill 1, Skill 2, Skill 3


    // LEGACY:
    //public SkillSlot(int slotCount_ = 2)
    //{
    //    slotCount = slotCount_;
    //    Slots = new SkillAction[slotCount];
    //}

    public SkillSlot(PlayerBase playerController_, int slotCount_ = 2)
    {
        skillManager = new SkillManager();

        slotCount = slotCount_;
        Slots = new SkillBase[slotCount];
             

        // TODO : ���õ� Hunter Witch�� ���� �ٸ� Dictionary�� �������� 
        // ���õ� Skill �̸��� �޾� Key ���� ���� Skill �� �ҷ��� �ʱ�ȭ�ϰ� slot�� �־� �ش�.
        skillManager.HunterSkill["Beartrap"].Init(playerController_);
        skillManager.HunterSkill["Wolf"].Init(playerController_);
        skillManager.HunterSkill["Cross"].Init(playerController_);

        Slots[0] = skillManager.HunterSkill["Wolf"];

        Slots[1] = skillManager.HunterSkill["Cross"];
    }

    public void SelSkill()
    {

    }

    //public bool CheckValid()
    //{
    //    return Slots.IsValid();
    //}

    // TODO : ���� Enum Witch Hunter�� �޴´ٸ� üũ�Լ� ������ ��
    //public int CheckType()
    //{

    //}

    // LEGACY : �����ڷ� ��ü 
    //// ��ų ������ �ʱ�ȭ 
    //void Init()
    //{

    //}       // Init()





}
