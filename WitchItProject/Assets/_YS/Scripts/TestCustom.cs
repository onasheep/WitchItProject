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
        Debug.Log("SaveCustom �Լ��� ȣ��Ǿ����ϴ�."); // SaveCustom �Լ� ȣ�� Ȯ��

        if (currentCustom >= 0 && currentCustom < customs.Length)
        {
            GameObject customObject = customs[currentCustom];

            // �÷��̾� ������Ʈ�� �ڽ� ������Ʈ�� Ŀ���� ������Ʈ�� �߰�
            customObject.transform.SetParent(PlayerObject.transform);

            string path = "Assets/Resources"; //+ PlayerObject.name;
            Directory.CreateDirectory(path); // ��ο� ���丮 ����
            string prefabPath = Path.Combine(path, PlayerObject.name + ".prefab"); // ������ ���

            // ������ ��� Ȯ��
            Debug.Log("prefabPath ��: " + prefabPath);

#if UNITY_EDITOR
            // �÷��̾� ������Ʈ(���� Ŀ���� ������Ʈ�� ����)�� ���������� ����
            PrefabUtility.SaveAsPrefabAsset(PlayerObject, prefabPath);
#endif
            Debug.Log("PrefabUtility.SaveAsPrefabAsset �Լ��� ȣ��Ǿ����ϴ�."); // PrefabUtility.SaveAsPrefabAsset �Լ� ȣ�� Ȯ��

            //AssetDatabase.Refresh();
#if UNITY_EDITOR
            // AssetDatabase�� ����ϴ� �ڵ�
            AssetDatabase.Refresh();
#endif
        }
    }
}
