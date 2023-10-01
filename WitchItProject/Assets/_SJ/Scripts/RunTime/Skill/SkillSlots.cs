using System;
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

    public SkillSlot(PlayerBase playerController_,int slotCount_ = 2)
    {
        skillManager = new SkillManager();

        slotCount = slotCount_;
        Slots = new SkillBase[slotCount];
             

        // TODO : 선택된 Hunter Witch에 따라 다른 Dictionary를 가져오고 
        // 선택된 Skill 이름을 받아 Key 값을 통해 Skill 을 불러내 초기화하고 slot에 넣어 준다.
        
        skillManager.HunterSkill["Wolf"].Init(playerController_);
        skillManager.HunterSkill["Cross"].Init(playerController_);

        skillManager.WitchSkill["Mushroom"].Init(playerController_);
        skillManager.WitchSkill["Possesion"].Init(playerController_);

       
    }

    

    public void SelSkill(int type)
    {    
        if(type == 0)
        {
            Slots[0] = skillManager.WitchSkill["Mushroom"];
            Slots[1] = skillManager.WitchSkill["Possesion"];
        
        }
        else if(type == 1)
        {
            Slots[0] = skillManager.HunterSkill["Wolf"];
            Slots[1] = skillManager.HunterSkill["Cross"];
        }
    }

    //public bool CheckValid()
    //{
    //    return Slots.IsValid();
    //}

    // TODO : 만약 Enum Witch Hunter를 받는다면 체크함수 구현할 것
    //public int CheckType()
    //{

    //}





}
