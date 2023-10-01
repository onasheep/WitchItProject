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
        //Debug.Log("SaveCustom �Լ��� ȣ��Ǿ����ϴ�.");

        string path = "Assets/Resources";
        Directory.CreateDirectory(path);

        // Ŀ���� ������Ʈ�� ��ģ �� ����
        if (rightCurrentCustom >= 0 && rightCurrentCustom < rightCustoms.Length &&
            backCurrentCustom >= 0 && backCurrentCustom < backCustoms.Length)
        {
            //Debug.Log("SaveCustomObject ȣ�� ��");
            SaveCustomObject(rightCustoms[rightCurrentCustom], backCustoms[backCurrentCustom]);
            //Debug.Log("SaveCustomObject ȣ�� ��");
        }
        else
        {
            //Debug.Log("SaveCustomObject ȣ�� ���� ������");
            //Debug.Log($"rightCurrentCustom: {rightCurrentCustom}, leftCurrentCustom: {leftCurrentCustom}");
            //Debug.Log($"rightCustoms.Length: {rightCustoms.Length}, leftCustoms.Length: {leftCustoms.Length}");
        }
    }

    void SaveCustomObject(GameObject rightCustomObject, GameObject backCustomObject)
    {
        rightCustomObject.transform.SetParent(PlayerObject.transform);
        backCustomObject.transform.SetParent(PlayerObject.transform);

        string prefabPath = Path.Combine("Assets/Resources", PlayerObject.name + ".prefab");

        // ������ ��� Ȯ��
        //Debug.Log("prefabPath ��: " + prefabPath);

#if UNITY_EDITOR
        // �÷��̾� ������Ʈ(���� Ŀ���� ������Ʈ�� ����)�� ���������� ����
        PrefabUtility.SaveAsPrefabAsset(PlayerObject, prefabPath);
#endif
        //Debug.Log("PrefabUtility.SaveAsPrefabAsset �Լ��� ȣ��Ǿ����ϴ�.");

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
}
