using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Possesion : SkillBase
{
    
    public Skill_Possesion(string skillType_ = "Possesion", float coolTime_ = 10f)
    {
        SkillType = skillType_;
        CoolTime = coolTime_;
    }
    
    public override void ActivateSkill(GameObject object_)
    {
        GameObject witchBody = PlayerController.gameObject;
        GameObject currBody = witchBody;
        GameObject lookPoint = PlayerController.gameObject.FindChildObj("WitchCamera");

        object_.AddComponent<RollingMove>();
        
        
        if (currBody == witchBody)
        {
            currBody.SetActive(false);

            lookPoint.transform.SetParent(object_.transform);
            lookPoint.transform.localPosition = Vector3.zero;

            currBody = object_;
        }
        else
        {
            GameObject prevBody_ = lookPoint.transform.parent.gameObject;

            lookPoint.transform.SetParent(object_.transform);
            lookPoint.transform.localPosition = Vector3.zero;

            //Destroy(prevBody_);

            currBody = object_;
        }
    }
}
