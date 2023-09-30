using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeInOutHunterUi : MonoBehaviour
{
    [SerializeField] private GameObject winBase;
    [SerializeField] private GameObject hunterWin;

    [SerializeField] private SpriteRenderer winBaseSprite;
    [SerializeField] private SpriteRenderer winSprite;


    private void OnEnable()
    {
        winBase = gameObject.transform.GetChild(0).gameObject;
        hunterWin = gameObject.transform.GetChild(1).gameObject;

        winBaseSprite = winBase.GetComponent<SpriteRenderer>();
        winSprite = hunterWin.GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        winBaseSprite.color = FadeIn(winBaseSprite.color, 2f);
        winSprite.color = FadeOut(winSprite.color, 0.5f);
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

    Color FadeOut(Color color, float changeSpeed)
    {
        Color spriteColor = color;

        if (spriteColor.a < 1)
        {
            spriteColor.a += Time.deltaTime * changeSpeed;
        }
        return spriteColor;
    }
}
