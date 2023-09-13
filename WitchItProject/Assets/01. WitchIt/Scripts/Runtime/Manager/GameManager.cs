using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // TODO : 씬 전환 기타 사유로 오류 날 수 있음
        // 방어로직 한개 추가 예정 IsValid()
        ResourceManager.Init();                                        
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}


