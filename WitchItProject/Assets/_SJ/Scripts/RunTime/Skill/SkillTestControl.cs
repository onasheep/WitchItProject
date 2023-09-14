using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillTestControl : PlayerBase
{
    public GameObject chicken;
    public GameObject cross;
    public Transform barrel_W;
    public Transform barrel_C;

    void Start()
    {
        base.Init();        
    }
    

    // TEST : 
    // TODO : 
    // 플레이어와 합치게 된다면, 마우스 방향 벡터를 알아야 하기 때문에 그것을 PlayerBase가 저장하고, 
    // 십자가와, 늑대, 덫 모두 그 방향으로 나간다. 
    // 다만 시작지점이 다르다 ex) ( 늑대 => 내 오른쪽 , 마늘 => 카메라 정면, 덫 => 카메라 정면 )
    void Update()
    {
        // { 뭔가 닭 같은거 발사하는 기능 
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // Wolf Test
            GameObject skillObj = Instantiate(ResourceManager.objs[RDefine.WOLF_OBJ], barrel_W.position, Quaternion.identity);
            base.skillSlot.Slots[0].ActivateSkill(skillObj);
            
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Cross Test
            GameObject skillObj = Instantiate(ResourceManager.objs[RDefine.CROSS_OBJ], barrel_C.position, Quaternion.identity);
            base.skillSlot.Slots[1].ActivateSkill(skillObj);

        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            // Cross Test
            GameObject skillObj = Instantiate(ResourceManager.objs["Mushroom_Orb"/*RDefine.MUSHROOM_OBJ*/], barrel_C.position, Quaternion.identity);
            base.skillSlot.Slots[1].ActivateSkill(skillObj);

        }


        //  뭔가 닭 같은거 발사하는 기능 }
    }
}


