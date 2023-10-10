using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager
{
    public Dictionary<string,SkillBase> HunterSkill { get; private set; }
    public Dictionary<string,SkillBase> WitchSkill { get; private set; }
    // Start is called before the first frame update

    public SkillManager()
    {
        HunterSkill = new Dictionary<string, SkillBase>();
        WitchSkill = new Dictionary<string,SkillBase>();

        InitSkill();
    }
    void InitSkill()
    {
        // TODO : �ʱ�ȭ ���� �� List ���� ������ SkillSlot ���� ����
        // SOLUTION : SceneManger ���� ���� �̸� ���� ?
        Skill_Wolf skill_wolf = new Skill_Wolf();
        Skill_Cross skill_cross = new Skill_Cross();
        Skill_Beartrap skill_beartrap = new Skill_Beartrap();

        Skill_Mushroom skill_mushroom = new Skill_Mushroom();
        Skill_Possesion skill_possesion = new Skill_Possesion();


        HunterSkill.Add(skill_wolf.SkillType,skill_wolf);
        HunterSkill.Add(skill_cross.SkillType, skill_cross);
        HunterSkill.Add(skill_beartrap.SkillType, skill_beartrap);
    
        WitchSkill.Add(skill_mushroom.SkillType, skill_mushroom);
        WitchSkill.Add(skill_possesion.SkillType, skill_possesion);

    }
}
