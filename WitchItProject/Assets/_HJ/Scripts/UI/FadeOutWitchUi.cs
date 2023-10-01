using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeOutWitchUi : MonoBehaviour
{

    [SerializeField] private GameObject winBase; //처음에 나타날 기본 승리 UI
    [SerializeField] private GameObject witchWin; //페이드 a값 높여줄 오브젝트입니다.

    [SerializeField] private SpriteRenderer winBaseSprite;
    [SerializeField] private SpriteRenderer winSprite;
    [SerializeField] private TextMeshProUGUI winText;

    // Start is called before the first frame update
    private void OnEnable()
    {
        winBase = gameObject.transform.GetChild(0).gameObject; 
        witchWin = gameObject.transform.GetChild(1).gameObject;
        winBaseSprite = winBase.GetComponent<SpriteRenderer>();
        winSprite = witchWin.GetComponent<SpriteRenderer>();
        winText = witchWin.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        winBaseSprite.color = FadeIn(winBaseSprite.color, 2f);
        winSprite.color = FadeOut(winSprite.color,1.5f);
        winText.color = FadeOut(winText.color, 0.3f);
    }

    Color FadeIn(Color color, float changeSpeed)
    {
        Color spriteColor = color;

        if (spriteColor.a > 0)
        {
            spriteColor.a -= Time.deltaTime * changeSpeed;
        }
        return spriteColor;

    }

    Color FadeOut(Color color,float changeSpeed)
    {
        Color spriteColor = color;

        if (spriteColor.a < 1)
        {
            spriteColor.a += Time.deltaTime * changeSpeed;
        }
        return spriteColor;
    }
}
