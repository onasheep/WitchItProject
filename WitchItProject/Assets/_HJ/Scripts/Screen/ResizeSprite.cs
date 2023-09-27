using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeSprite : MonoBehaviour
{
    // �⺻ �ػ� & ��������Ʈ ����� ���� ����
    private float defaultWidth = 1920f;
    private float defaultHeight = 1080f;
    private float defaultRate;
    Sprite sprite;

    private void Awake()
    {
        // �ڽ��� ��������Ʈ�� ������
        sprite = GetComponent<Sprite>();

        // �⺻ �ػ� ������ ���
        defaultRate = defaultWidth / defaultHeight;

        // ���� ��������Ʈ�� �������� �ϴ� �Լ��� ȣ��
        ResizeMineSprite();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // ���� ��������Ʈ�� �������� �ϴ� �Լ�
    private void ResizeMineSprite()
    {
        // ���� Ŭ���̾�Ʈ�� ũ�⸦ ������
        float clientWidth = Screen.width;
        float clientHeight = Screen.height;

        // 

    }
}
