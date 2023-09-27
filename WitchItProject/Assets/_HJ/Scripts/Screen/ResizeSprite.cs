using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeSprite : MonoBehaviour
{
    // 기본 해상도 & 스프라이트 저장용 변수 선언
    private float defaultWidth = 1920f;
    private float defaultHeight = 1080f;
    private float defaultRate;
    Sprite sprite;

    private void Awake()
    {
        // 자신의 스프라이트를 가져옴
        sprite = GetComponent<Sprite>();

        // 기본 해상도 비율을 계산
        defaultRate = defaultWidth / defaultHeight;

        // 현재 스프라이트를 리사이즈 하는 함수를 호출
        ResizeMineSprite();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // 현재 스프라이트를 리사이즈 하는 함수
    private void ResizeMineSprite()
    {
        // 현재 클라이언트의 크기를 가져옴
        float clientWidth = Screen.width;
        float clientHeight = Screen.height;

        // 

    }
}
