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

    // TODO : 만약 Enum Witch Hunter를 받는다면 체크함수 구현할 것
    //public int CheckType()
    //{

    //}

    // LEGACY : 생성자로 대체 
    //// 스킬 정보를 초기화 
    //void Init()
    //{

    //}       // Init()





}
