using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;



public class WitchCustom : MonoBehaviour
{
    public GameObject PlayerObject;
    public GameObject[] rightCustoms;
    private int rightCurrentCustom;

    public GameObject[] backCustoms;
    private int backCurrentCustom;

    public Button saveButton;
    public Button switchRightButton;
    public Button switchBackButton;

    // Start is called before the first frame update
    void Start()
    {
        saveButton.onClick.AddListener(SaveCustom);

        switchRightButton.onClick.AddListener(SwitchRightCustom);
        switchBackButton.onClick.AddListener(SwitchBackCustom);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRightCustom();
        UpdateBackCustom();
    }

    public void SwitchRightCustom()
    {
        if (rightCustoms.Length > 0)
        {
            rightCurrentCustom = (rightCurrentCustom + 1) % rightCustoms.Length;
        }
    }

    public void SwitchBackCustom()
    {
        if (backCustoms.Length > 0)
        {
            backCurrentCustom = (backCurrentCustom + 1) % backCustoms.Length;
        }
    }


    void UpdateRightCustom()
    {
        for (int i = 0; i < rightCustoms.Length; i++)
        {
            if (i == rightCurrentCustom)
            {
                rightCustoms[i].SetActive(true);
            }
            else
            {
                rightCustoms[i].SetActive(false);
            }
        }
    }

    void UpdateBackCustom()
    {
        for (int i = 0; i < backCustoms.Length; i++)
        {
            if (i == backCurrentCustom)
            {
                backCustoms[i].SetActive(true);
            }
            else
            {
                backCustoms[i].SetActive(false);
            }
        }
    }

    public void SaveCustom()
    {
        //Debug.Log("SaveCustom 함수가 호출되었습니다.");

        string path = "Assets/Resources";
        Directory.CreateDirectory(path);

        // 커스텀 오브젝트를 합친 후 저장
        if (rightCurrentCustom >= 0 && rightCurrentCustom < rightCustoms.Length &&
            backCurrentCustom >= 0 && backCurrentCustom < backCustoms.Length)
        {
            //Debug.Log("SaveCustomObject 호출 전");
            SaveCustomObject(rightCustoms[rightCurrentCustom], backCustoms[backCurrentCustom]);
            //Debug.Log("SaveCustomObject 호출 후");
        }
        else
        {
            //Debug.Log("SaveCustomObject 호출 조건 미충족");
            //Debug.Log($"rightCurrentCustom: {rightCurrentCustom}, leftCurrentCustom: {leftCurrentCustom}");
            //Debug.Log($"rightCustoms.Length: {rightCustoms.Length}, leftCustoms.Length: {leftCustoms.Length}");
        }
    }

    void SaveCustomObject(GameObject rightCustomObject, GameObject backCustomObject)
    {
        rightCustomObject.transform.SetParent(PlayerObject.transform);
        backCustomObject.transform.SetParent(PlayerObject.transform);

        string prefabPath = Path.Combine("Assets/Resources", PlayerObject.name + ".prefab");

        // 프리팹 경로 확인
        //Debug.Log("prefabPath 값: " + prefabPath);

#if UNITY_EDITOR
        // 플레이어 오브젝트(이제 커스텀 오브젝트를 포함)를 프리팹으로 저장
        PrefabUtility.SaveAsPrefabAsset(PlayerObject, prefabPath);
#endif
        //Debug.Log("PrefabUtility.SaveAsPrefabAsset 함수가 호출되었습니다.");

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
}
