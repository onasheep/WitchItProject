using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ThreadManager : MonoBehaviour
{
    public static ThreadManager instance;
    
    private static List<Coroutine> routines;

    private List<Coroutine> runningCoroutine;

    private void Awake()
    {
        if(instance == null || instance == default)
        {
            instance = this;
            
            Init();
        }       // if : instance�� ����ִ� ��� ä���ְ�, �ʱ�ȭ �ʿ� O
        else
        {
            routines.Clear();
            return;
        }       // else : instance�� �̹� �����ϱ� ������ , routines�� ������
    }

    private void Init()
    {
        routines = new List<Coroutine>();
    }

    private void OnDestroy()
    {
        //routines.KillAllCoroutuine(this);

        //routines.Clear();
    }

    // TEST : 
    private void KillAll()
    {
        //if(Input.GetKeyDown(KeyCode.P))
        //{
        //    routines.KillAllCoroutuine(this);
        //    foreach(Coroutine routine in routines)
        //    {
        //        Debug.LogFormat("routine is default? : {0}", routine == default);
        //    }
        //}



    }
    public void Update()
    {
        KillAll();
        Debug.LogFormat("{0}",routines.Count);
    }

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
