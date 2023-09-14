using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public static partial class GFunc
{
    //! ���� ��Ʈ ������Ʈ�� ��ġ�ؼ� ã�� �ִ� �Լ�
    public static GameObject GetRootObj(string objName_)
    {
        Scene activeScene = GetActiveScene();
        GameObject[] rootObjs_ = activeScene.GetRootGameObjects();

        GameObject targetObj_ = default;
        foreach (GameObject rootObj in rootObjs_)
        {
            if (rootObj.name == objName_)
            {
                targetObj_ = rootObj;
                return targetObj_;
            }
            else { continue; }
        }       // loop 

        return targetObj_;
    }       // GetRootObj

    //! Ư�� ������Ʈ�� �ڽ� ������Ʈ�� ��� ����Ʈ�� �����ϴ� �Լ�
    public static List<GameObject> GetChildrenObjs
        (this GameObject targetObj_)
    {
        List<GameObject> objs = new List<GameObject>();
        GameObject searchTarget = default;

        for (int i = 0; i < targetObj_.transform.childCount; i++)
        {
            searchTarget = targetObj_.transform.GetChild(i).gameObject;
            objs.Add(searchTarget);
        }

        if (objs.IsValid()) { return objs; }
        else { return default(List<GameObject>); }
    }       // GetChildrenObjs()

    
    //! Ư�� ������Ʈ�� �ڽ� ������Ʈ�� ��ġ�ؼ� ã�� �Լ� 
    public static GameObject FindChildObj
        (this GameObject targetObj_, string objName_)
    {
        GameObject searchResult = default;
        GameObject searchTarget = default;
        for (int i = 0; i < targetObj_.transform.childCount; i++)
        {
            searchTarget = targetObj_.transform.GetChild(i).gameObject;
            if (searchTarget.name.Equals(objName_))
            {
                searchResult = searchTarget;
                return searchResult;
            }
            else
            {
                searchResult = FindChildObj(searchTarget, objName_);

                // ������
                if (searchResult == null || searchResult == default) { /* Pass */ }
                else { return searchResult; }
            }
        }       // loop

        return searchResult;
    }       // FindChildObj()

    //! ���� ��Ʈ ������Ʈ�� ���� �ڽ� ������Ʈ�� ���۳�Ʈ�� �������� �Լ�
    public static T FindChildComponent<T>
        (this GameObject targetObj_, string objName_) where T : Component
    {
        T searchResultComponent = default(T);
        GameObject searchResultObj = default(GameObject);

        searchResultObj = targetObj_.FindChildObj(objName_);
        if (searchResultObj.IsValid())
        {
            searchResultComponent = searchResultObj.GetComponent<T>();
        }

        return searchResultComponent;
    }       // FindChildComponent()


    public static Scene GetActiveScene()
    {
        Scene activeScene_ = SceneManager.GetActiveScene();
        return activeScene_;
    }       // GetActiveScene()
}
