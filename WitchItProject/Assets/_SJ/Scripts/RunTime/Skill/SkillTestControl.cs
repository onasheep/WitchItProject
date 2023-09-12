using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillTestControl : PlayerBase
{
    public GameObject chicken;
    public GameObject cross;
    public Transform barrel;

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
            GameObject cross = Instantiate(ResourceManager.resources[RDefine.WOLF_OBJ], barrel.position, Quaternion.identity);
            base.skillSlot.Slots[0].ActivateSkill(cross);
            
        }
        //  ���� �� ������ �߻��ϴ� ��� }
    }
}


