using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;



public class TestCustom : MonoBehaviour
{
    public GameObject PlayerObject;
    public GameObject[] customs;
    private int currentCustom;

    public Button saveButton;
    public Button switchButton;

    // Start is called before the first frame update
    void Start()
    {
        saveButton.onClick.AddListener(SaveCustom);

        switchButton.onClick.AddListener(SwitchCustom);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCustoms();

        //for (int i = 0; i < customs.Length; i++)
        //{
        //    if (i == currentCustom)
        //    {
        //        customs[i].SetActive(true);
        //    }
        //    else
        //    {
        //        customs[i].SetActive(false);
        //    }
        //}
    }

    public void SwitchCustom()
    {
        //if (currentCustom == customs.Length - 1)
        //{
        //    currentCustom = 0;
        //}
        //else
        //{
        //    currentCustom++;
        //}

        if (customs.Length > 0)
        {
            currentCustom = (currentCustom + 1) % customs.Length;
        }


    }


    void UpdateCustoms()
    {
        for (int i = 0; i < customs.Length; i++)
        {
            if (i == currentCustom)
            {
                customs[i].SetActive(true);
            }
            else
            {
                customs[i].SetActive(false);
            }
        }
    }

    public void SaveCustom()
    {
        Debug.Log("SaveCustom 함수가 호출되었습니다."); // SaveCustom 함수 호출 확인

        if (currentCustom >= 0 && currentCustom < customs.Length)
        {
            GameObject customObject = customs[currentCustom];

            // 플레이어 오브젝트의 자식 오브젝트로 커스텀 오브젝트를 추가
            customObject.transform.SetParent(PlayerObject.transform);

            string path = "Assets/Resources"; //+ PlayerObject.name;
            Directory.CreateDirectory(path); // 경로에 디렉토리 생성
            string prefabPath = Path.Combine(path, PlayerObject.name + ".prefab"); // 프리팹 경로

            // 프리팹 경로 확인
            Debug.Log("prefabPath 값: " + prefabPath);

#if UNITY_EDITOR
            // 플레이어 오브젝트(이제 커스텀 오브젝트를 포함)를 프리팹으로 저장
            PrefabUtility.SaveAsPrefabAsset(PlayerObject, prefabPath);
#endif
            Debug.Log("PrefabUtility.SaveAsPrefabAsset 함수가 호출되었습니다."); // PrefabUtility.SaveAsPrefabAsset 함수 호출 확인

            //AssetDatabase.Refresh();
#if UNITY_EDITOR
            // AssetDatabase를 사용하는 코드
            AssetDatabase.Refresh();
#endif
        }
    }
}
