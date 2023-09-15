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
    // �÷��̾�� ��ġ�� �ȴٸ�, ���콺 ���� ���͸� �˾ƾ� �ϱ� ������ �װ��� PlayerBase�� �����ϰ�, 
    // ���ڰ���, ����, �� ��� �� �������� ������. 
    // �ٸ� ���������� �ٸ��� ex) ( ���� => �� ������ , ���� => ī�޶� ����, �� => ī�޶� ���� )
    void Update()
    {
        // { ���� �� ������ �߻��ϴ� ��� 
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


        //  ���� �� ������ �߻��ϴ� ��� }
    }
}


