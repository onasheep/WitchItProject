using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ThreadManager : MonoBehaviour
{
    public static ThreadManager instance;
    
    private static List<Coroutine> routines;

    
    private void Awake()
    {
        if(instance == null || instance == default)
        {
            instance = this;
            
            Init();
        }       // if : instance�� ����ִ� ��� ä���ְ�, �ʱ�ȭ �ʿ� O
        else
        {
            return;
        }       // else : instance�� �̹� �����ϱ� ������ , �ʱ�ȭ �ʿ� X
    }

    private void Init()
    {
        routines = new List<Coroutine>();
    }



    public void Update()
    {
        Debug.LogFormat("{0}",routines.Count);
    }

    

    // TODO : GFunc + Thread ���� ����Ǹ� ������ ���� 
    //private void OnDestroy()
    //{
    //    if( routines.Count > 1 )
    //    {
    //        foreach(IEnumerator routine in routines)
    //        {
    //            StopCoroutine(routine);
    //        }
    //    }
    //    else { /* Do Nothing */ }
        
    //}

    public void RemoveRoutine(Coroutine routine)
    {
        routines.Remove(routine);
        Debug.LogFormat("routine null or default ? : {0}", GFunc.IsCoroutineDead(routine));


    }


    public Coroutine DoRoutine(UnityAction action_, float time_)
    {
        Coroutine curruntRoutine = default;

        // TODO : ���� KillCoroutine ���� Ȯ�� �� �߰� ����
        curruntRoutine = StartCoroutine(SetTimer(action_, time_))/*.KillCoroutine(this,time_ + 0.01f)*/;
        routines.Add(curruntRoutine);
        return curruntRoutine;
    }

    private IEnumerator SetTimer(UnityAction action_, float time_)
    {

        while (0 <= time_)
        {
            time_ -= Time.deltaTime;
            yield return null;
        }
        action_();

    }

    private IEnumerator LateCallFunc(UnityAction action, float delay)
    {
        while (true)
        {
            yield return null;
        }
    }

    //private static IEnumerator KillTime(this IEnumerator co_, float time)
    //{

    //    return co_;
    //}
    // Start is called before the first frame update

}
