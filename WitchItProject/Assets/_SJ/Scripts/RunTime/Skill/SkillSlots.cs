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
             

        // TODO : 선택된 Hunter Witch에 따라 다른 Dictionary를 가져오고 
        // 선택된 Skill 이름을 받아 Key 값을 통해 Skill 을 불러내 초기화하고 slot에 넣어 준다.
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
