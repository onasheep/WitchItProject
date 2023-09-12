using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot
{
    private int slotCount = -1;
    public SkillBase[] Slots { get; private set; }
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
        slotCount = slotCount_;
        Slots = new SkillBase[slotCount];


        Skill_Wolf skill_wolf = new Skill_Wolf();
        Skill_Cross skill_cross = new Skill_Cross();

        skill_wolf.Init(playerController_);
        skill_cross.Init(playerController_);

        Slots[0] = skill_cross;
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
